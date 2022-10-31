using MediatR;
using Services.Commands;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Handlers
{
    public class AddTransactionHandler : IRequestHandler<AddTransactionCommand, Unit>
    {
        private readonly TransactionDataAccess _transactionDataAccess;
        public AddTransactionHandler(TransactionDataAccess transactionDataAccess) => _transactionDataAccess = transactionDataAccess;
        public async Task<Unit> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            await _transactionDataAccess.Add(request.Transaction);

            return Unit.Value;
        }
    }
}
