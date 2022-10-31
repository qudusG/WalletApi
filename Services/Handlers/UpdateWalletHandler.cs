using Core.Models;
using MediatR;
using Services.Commands;
using Services.Queries;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Handlers
{
    public class UpdateWalletHandler : IRequestHandler<UpdateWalletCommand, Unit>
    {
        private readonly WalletDataAccess _walletDataAccess;
        public UpdateWalletHandler(WalletDataAccess walletDataAccess) => _walletDataAccess = walletDataAccess;
        public async Task<Unit> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            await _walletDataAccess.Update(request.Wallet);

            return Unit.Value;
        }
    }
}
