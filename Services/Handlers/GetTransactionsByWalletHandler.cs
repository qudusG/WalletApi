using Core.Models;
using MediatR;
using Services.Queries;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Handlers
{
    public class GetTransactionsByWalletHandler : IRequestHandler<GetTransactionsByWalletIdQuery, IEnumerable<Transaction>>
    {
        private readonly TransactionDataAccess _transactionDataAccess;
        public GetTransactionsByWalletHandler(TransactionDataAccess transactionDataAccess) => _transactionDataAccess = transactionDataAccess;
        public async Task<IEnumerable<Transaction>> Handle(GetTransactionsByWalletIdQuery request,
            CancellationToken cancellationToken) => await _transactionDataAccess.GetByWalletId(request.walletId, request.start, request.end);
    }
}
