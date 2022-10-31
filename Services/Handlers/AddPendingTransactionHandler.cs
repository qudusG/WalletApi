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
    public class AddPendingTransactionHandler : IRequestHandler<AddPendingTransactionCommand, Unit>
    {
        private readonly PendingTransactionDataAccess _pendingTransactionDataAccess;
        public AddPendingTransactionHandler(PendingTransactionDataAccess pendingTransactionDataAccess) => _pendingTransactionDataAccess = pendingTransactionDataAccess;
        public async Task<Unit> Handle(AddPendingTransactionCommand request, CancellationToken cancellationToken)
        {
            await _pendingTransactionDataAccess.Add(request.PendingTransaction);

            return Unit.Value;
        }
    }
}
