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
    public class SignInHandler : IRequestHandler<SignInCommand, (string token, DateTime? expiryDate)>
    {
        private readonly UserDataAccess _userDataAccess;
        public SignInHandler(UserDataAccess userDataAccess) => _userDataAccess = userDataAccess;
        public async Task<(string token, DateTime? expiryDate)> Handle(SignInCommand request,
            CancellationToken cancellationToken) => await _userDataAccess.SignInEmail(request.login);
    }
}
