using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class UserDataAccess
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        public UserDataAccess(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<IdentityResult> CreateUser(UserDTO userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                WalletType = userDto.WalletType
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            return result;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task Update(User user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task<(string token, DateTime? expiryDate)> SignInEmail(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await GenerateJWT(user);
            }
            return (string.Empty, null);
        }
        public async Task<(string token, DateTime? expiryDate)> GenerateJWT(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
            var userClaims = await _userManager.GetClaimsAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(type: "Email", user.Email ?? ""),
                    new Claim(type: "RegistrationDate", value: user.RegistrationDate.ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString())
                }.Union(userClaims).Union(userRoles)),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            await _context.SaveChangesAsync();
           
            return (tokenHandler.WriteToken(token), tokenDescriptor.Expires);
        }
    }
}
