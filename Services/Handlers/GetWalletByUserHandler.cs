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
    public class GetWalletByUserHandler : IRequestHandler<GetWalletByUserIdQuery, Wallet>
    {
        private readonly WalletDataAccess _walletDataAccess;
        public GetWalletByUserHandler(WalletDataAccess walletDataAccess) => _walletDataAccess = walletDataAccess;
        public async Task<Wallet> Handle(GetWalletByUserIdQuery request,
            CancellationToken cancellationToken) => await _walletDataAccess.GetWalletByUserId(request.userId);
    }
}
