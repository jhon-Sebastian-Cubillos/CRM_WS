using Inmersys.Domain.DB.Schema.History;
using Inmersys.Domain.DB.Schema.Profile;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inmersys.Domain.Interface.Repo
{
    public interface ITokenManager
    {
        public Task<Task> ClaimTokenAsync(Pro_Profile profile, ulong rol_id);

        public Task<Task> ValidateTokenAsync();

        public Task<Task> RefreshTokenAsync(Action<His_Session>? opt = null);
    }
}
