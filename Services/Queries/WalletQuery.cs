using Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public record GetWalletByUserIdQuery(string userId) : IRequest<Wallet>;
    public record GetTransactionsByWalletIdQuery(long walletId, DateTime? start, DateTime? end) : IRequest<IEnumerable<Transaction>>;
}
