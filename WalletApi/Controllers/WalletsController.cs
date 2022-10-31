using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Commands;
using Services.Messages;
using Services.Queries;

namespace WalletApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly string userId;
        private readonly IMessageProducer _messagePublisher;
        public WalletsController(IMediator mediator, IMessageProducer messageProducer)
        {
            userId = Helper.GetLoggedUserId(User);
            _mediator = mediator;
            _messagePublisher = messageProducer;
        }
        [HttpPost]
        public async Task<IActionResult> Fund(decimal amount)
        {
            var wallet = await _mediator.Send(new GetWalletByUserIdQuery(userId));

            var transaction = new Core.Models.Transaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                BalanceAfter = wallet.Balance + amount,
                BalanceBefore = wallet.Balance,
                TransactionDate = DateTime.UtcNow,
                TransactionType = TransactionType.Credit
            };
            wallet.Balance += amount;
            wallet.UpdatedDate = DateTime.UtcNow;
            await _mediator.Send(new AddTransactionCommand(transaction));
            await _mediator.Send(new UpdateWalletCommand(wallet));
            return Ok(new { Message = "account funded." });
        }
        [HttpPost]
        public async Task<IActionResult> Withdraw(decimal amount)
        {
            var wallet = await _mediator.Send(new GetWalletByUserIdQuery(userId));

            //To prevent double spending, transaction is added to a queue and processed sequentially for that user
            var pendingTransaction = new PendingTransaction
            {
                Amount = amount,
                WalletId = wallet.Id
            };
            await _mediator.Send(new AddPendingTransactionCommand(pendingTransaction));
            _messagePublisher.SendMessage(pendingTransaction);
            return Ok("transaction created and will be processed shortly.");
        }
        [HttpGet]
        public async Task<IActionResult> BalanceAndTransactionHistory(DateTime? startDate, DateTime? endDate)
        {
            var wallet = await _mediator.Send(new GetWalletByUserIdQuery(userId));
            var transactions = await _mediator.Send(new GetTransactionsByWalletIdQuery(wallet.Id, startDate, endDate));
            return Ok(new { Balance = wallet.Balance, Transactions = transactions }) ;
        }
    }
}
