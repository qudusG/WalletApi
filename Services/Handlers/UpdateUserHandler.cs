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
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly UserDataAccess _userDataAccess;
        public UpdateUserHandler(UserDataAccess userDataAccess) => _userDataAccess = userDataAccess;
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userDataAccess.Update(request.user);

            return Unit.Value;
        }
    }
}
