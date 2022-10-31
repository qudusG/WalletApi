using Core.DTOs;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Commands;
using Services.Queries;

namespace WalletApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;  
        }
    
        [HttpPost("SignUpByEmail")]
        public async Task<IActionResult> RegisterWithEmail([FromBody] UserDTO userDto)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(userDto.Email));
            if (user != null)
            {
                return BadRequest(new { Message = "User with this email already exist." });
            }
            var result = await _mediator.Send(new AddUserCommand(userDto));
            if (result.Succeeded)
            {
                user = await _mediator.Send(new GetUserByEmailQuery(userDto.Email));
                await CreateWallet(user);
                return Ok(new { Message = "user created." });
            }
            return BadRequest(new { Message = "Failed to create user." });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _mediator.Send(new GetUserByEmailQuery(login.Email));
            if (user == null)
                return BadRequest(new { Message = "This user does not exist." });
            if (!user.IsWalletCreated)
            {
                var walletResult = await CreateWallet(user);
                if (walletResult != 1)
                    return BadRequest(new { Message = "Unable to login because user " +
                        "does not have an active wallet and attempting to create a new wallet failed." });
            }
            var result = await _mediator.Send(new SignInCommand(login));
            if (string.IsNullOrEmpty(result.token))
                return BadRequest(new { Message = "Failed to login." });

            return Ok(result);
        }
        private async Task<int> CreateWallet(User user)
        {
            var wallet = new Wallet
            {
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Type = user.WalletType,
                Balance = 0
            };
            var addResult = await _mediator.Send(new AddWalletCommand(wallet));
            if (addResult == 1)
            {
                user.IsWalletCreated = true;
                await _mediator.Send(new UpdateUserCommand(user));
            }
            return addResult;
        }
    }
}
