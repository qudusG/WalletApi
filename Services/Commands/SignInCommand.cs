using Core.DTOs;
using Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public record SignInCommand(LoginDTO login) : IRequest<(string token, DateTime? expiryDate)>;
}
