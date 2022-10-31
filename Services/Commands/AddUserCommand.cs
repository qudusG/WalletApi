using Core.DTOs;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public record AddUserCommand(UserDTO user) : IRequest<IdentityResult>;
}
