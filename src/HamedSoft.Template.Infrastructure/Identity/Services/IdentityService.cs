using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamedSoft.Template.Infrastructure.Identity.Services
{
    public sealed class IdentityService : IAuthenticationService
    {
        public Task<Result<LoginResult>> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
