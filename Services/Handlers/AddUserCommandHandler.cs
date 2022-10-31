using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Commands;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Handlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, IdentityResult>
    {
        private readonly UserDataAccess _userDataAccess;
        public AddUserCommandHandler(UserDataAccess userDataAccess) => _userDataAccess = userDataAccess;
        public async Task<IdentityResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userDataAccess.CreateUser(request.user);

            return result;
        }
    }
}
