using Inmersys.Core.Authorization.Attribute;
using Inmersys.Domain.Interface.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Inmersys.Core.Authorization.Handler
{
    public class AuthorizationEngineHandler : AuthorizationHandler<AuthorizationEngineAttribute>
    {
        private readonly ITokenManager _token_manager;
        private readonly IHttpContextAccessor _application;

        public AuthorizationEngineHandler(IServiceProvider provider)
        {
            _token_manager = provider.GetRequiredService<ITokenManager>();
            _application = provider.GetRequiredService<IHttpContextAccessor>();
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationEngineAttribute requirement)
        {
            Task validation = await _token_manager.ValidateTokenAsync();

            if (validation.IsCompletedSuccessfully)
            {
                Task refresh = await _token_manager.RefreshTokenAsync();

                if (refresh.IsCompletedSuccessfully)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    if (refresh.Exception != null) _application.HttpContext.Response.Headers.Add("FailReason", new Microsoft.Extensions.Primitives.StringValues(refresh.Exception.Message));
                }
            }
            else
            {
                if (validation.Exception != null) _application.HttpContext.Response.Headers.Add("FailReason", new Microsoft.Extensions.Primitives.StringValues(validation.Exception.Message));
            }

            return Task.CompletedTask;
        }
    }
}
