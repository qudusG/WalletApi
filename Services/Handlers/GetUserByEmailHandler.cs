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
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly UserDataAccess _userDataAccess;
        public GetUserByEmailHandler(UserDataAccess userDataAccess) => _userDataAccess = userDataAccess;
        public async Task<User> Handle(GetUserByEmailQuery request,
            CancellationToken cancellationToken) => await _userDataAccess.GetUserByEmail(request.email);
    }
}
