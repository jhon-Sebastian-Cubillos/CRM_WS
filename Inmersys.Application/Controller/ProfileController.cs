using Inmersys.Domain.DB.Schema.Profile;
using Inmersys.Domain.Interface.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmersys.Application.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ITokenManager _sec_repo;

        public ProfileController(ITokenManager sec_repo)
        {
            _sec_repo = sec_repo;
        }

        [HttpPost("Claim/{id_rol}"), ActionName("Token")]
        public async Task Post(ulong id_rol) => await _sec_repo.ClaimTokenAsync(new Pro_Profile(), id_rol);

        [HttpGet]
        public async Task Get() => await _sec_repo.ValidateTokenAsync();
    }
}
