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
    public class AddWalletHandler : IRequestHandler<AddWalletCommand, int>
    {
        private readonly WalletDataAccess _walletDataAccess;
        public AddWalletHandler(WalletDataAccess walletDataAccess) => _walletDataAccess = walletDataAccess;
        public async Task<int> Handle(AddWalletCommand request, CancellationToken cancellationToken)
        {
            return await _walletDataAccess.Add(request.Wallet);
        }
    }
}
