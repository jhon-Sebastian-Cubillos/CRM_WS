using Inmersys.Domain.DB;
using Inmersys.Domain.Interface.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Inmersys.Infrastructure.Base.Extensions;
using Inmersys.Domain.DB.Schema.Profile;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Inmersys.Domain.DB.Schema.History;
using Inmersys.Domain.DB.Schema.Definition;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Inmersys.Domain.DB.Schema.Security;

namespace Inmersys.Infrastructure.Base.Repo
{
    public class TokenManager : DB_BaseManager, ITokenManager
    {
        private readonly IHttpContextAccessor _application;
        private readonly IConfiguration _config;

        public TokenManager(CRM_DB db, IHttpContextAccessor application, IConfiguration conf) : base(db)
        {
            _application = application;
            _config = conf;
        }

        #region His_Session

        public async Task<Task> ClaimTokenAsync(Pro_Profile profile, ulong rol_id)
        {
            IConfigurationSection jwt_vars = _config.GetSection("AppSettings:Security:Jwt");

            string? client_ip = _application
                .HttpContext?
                .Connection
                .RemoteIpAddress?
                .MapToIPv4()
                .ToString();

            Def_Rol? rol = await base.FindAsync<Def_Rol>(rol_id);
            //Def_Rol? rol = new Def_Rol { name = "agg" };

            if (rol == null) return Task.FromException(new ArgumentException("No existe un rol para el identificador entregado"));

            if (!await base.ExistsAsync<Sec_Rol>(sec => sec.id.Equals(profile.id) && sec.rol_id.Equals(rol_id))) return Task.FromException(new BadHttpRequestException("No posee permisos para el rol especificado"));

            ClaimsIdentity claims_identity = new ClaimsIdentity(new[]
            {
                new Claim("jti", App_Utilities.NewJti()),
                new Claim("user_id", profile.id.ToString()),
                new Claim("user_name", string.Join(" ", new[] { profile.f_name, profile.l_name })),
                new Claim("rol_id", rol_id.ToString()),
                new Claim("rol_name", rol.name),
            });

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(jwt_vars.GetValue<string>("location_url").Replace("{{ip}}", client_ip));
            request.UserAgent = "ipapi.co/#c-sharp-v1.03";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Dictionary<string, object>? data = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(response.GetResponseStream());

            IEnumerable<KeyValuePair<string,string>>? data2 = data?
                .Where(el => new[] { "ip", "network", "version","latitude", "longitude", "utc_offset", "asn" }
                .Any(i => i == el.Key))
                .Select(el => new KeyValuePair<string,string>(el.Key, el.Value.ToString()));

            foreach(KeyValuePair<string,string> iterable in data2)
            {
                claims_identity.AddClaim(new Claim(iterable.Key, iterable.Value));
            }
            
            DateTime exp = DateTime.UtcNow.AddMinutes(jwt_vars.GetValue<int>("Conf:Expire_min"));

            His_Session session = new His_Session
            {
                id = 0UL,
                Jti = claims_identity.FindFirst("jti")?.Value ?? "",
                req_ip = claims_identity.FindFirst("ip")?.Value ?? "",
                client_loc = JsonSerializer.Serialize(data2) ?? "",
                user_id = ulong.Parse(claims_identity.FindFirst("user_id")?.Value ?? "0"),
                rol_id = ulong.Parse(claims_identity.FindFirst("rol_id")?.Value ?? "0"),
                registred_date = DateTime.UtcNow,
                exp_date = exp,
            };

            await base.AddAsync(session);

            SecurityTokenDescriptor token_desc = new SecurityTokenDescriptor
            {
                Subject = claims_identity,
                Expires = exp,
                Issuer = jwt_vars.GetValue<string>("Issuer"),
                Audience = jwt_vars.GetValue<string>("Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt_vars.GetValue<string>("Key"))), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken sec_token = handler.CreateToken(token_desc);
            
            string encoded_token = handler.WriteToken(sec_token);

            if (_application.HttpContext.Response.Headers.Any(el => el.Key == "Token"))
                _application.HttpContext.Response.Headers.Remove("Token");

            _application.HttpContext.Response.Headers.Add("Token", new StringValues(encoded_token));

            return Task.CompletedTask;
        }

        public async Task<Task> ValidateTokenAsync()
        {   
            if (_application.HttpContext?.Request.Headers.Authorization.Count == 0) return Task.FromException(new ArgumentNullException("esquema de autenticación no identificado"));

            string[]? vars = _application.HttpContext?.Request.Headers.Authorization[0]?.Split(" ");
            
            var Auth = new
            {
                Scheme = vars?[0],
                Parameter = vars?[1]
            };

            if(Auth.Scheme != "Bearer")
            {
                return Task.FromException(new BadHttpRequestException("esquema de autenticación erroneo"));
            }
            else
            {
                if(string.IsNullOrEmpty(Auth.Parameter)) return Task.FromException(new BadHttpRequestException("Token no identificado"));

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                TokenValidationParameters parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config.GetSection("AppSettings:Security:Jwt:Issuer").Value,
                    ValidAudience = _config.GetSection("AppSettings:Security:Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Security:Jwt:Key").Value)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                TokenValidationResult result = await handler.ValidateTokenAsync(Auth.Parameter, parameters);

                if (!result.IsValid) return Task.FromException(new SecurityTokenNotYetValidException("Token no válido"));

                IEnumerable<KeyValuePair<string, string>>? client_location = result
                    .ClaimsIdentity
                    .FindAll(claim => new[] { "ip", "network", "version","latitude", "longitude", "utc_offset", "asn" }.Any(i => i == claim.Type))
                    .Select(el => new KeyValuePair<string,string>(el.Type, el.Value));
                
                
                His_Session session = new His_Session
                {
                    id = 0UL,
                    Jti = result.ClaimsIdentity.FindFirst("jti")?.Value ?? "",
                    req_ip = result.ClaimsIdentity.FindFirst("ip")?.Value ?? "",
                    client_loc = JsonSerializer.Serialize(client_location) ?? "",
                    user_id = ulong.Parse(result.ClaimsIdentity.FindFirst("user_id")?.Value ?? "0"),
                    rol_id = ulong.Parse(result.ClaimsIdentity.FindFirst("rol_id")?.Value ?? "0"),
                    exp_date = result.SecurityToken.ValidTo,
                };

                if (await base.ExistsAsync<His_Session>(
                    data => data.Jti.Equals(session.Jti)
                    && data.req_ip.Equals(session.req_ip)
                    && data.client_loc.Equals(session.client_loc)
                    && data.user_id.Equals(session.user_id)
                    && data.rol_id.Equals(session.rol_id)
                    && data.exp_date.Equals(session.exp_date)))
                {
                    return Task.CompletedTask;
                }
                else
                {
                    return Task.FromException(new HttpListenerException((int)HttpStatusCode.Unauthorized, "Token no registrado"));
                }
            }
        }

        public async Task<Task> RefreshTokenAsync(Action<His_Session>? opt = null)
        {
            if (_application.HttpContext?.Request.Headers.Authorization.Count == 0) return Task.FromException(new ArgumentNullException("esquema de autenticación no identificado"));

            string[]? vars = _application.HttpContext?.Request.Headers.Authorization[0]?.Split(" ");

            var Auth = new
            {
                Scheme = vars?[0],
                Parameter = vars?[1]
            };

            if (Auth.Scheme != "Bearer")
            {
                return Task.FromException(new BadHttpRequestException("esquema de autenticación erroneo"));
            }
            else
            {
                if (string.IsNullOrEmpty(Auth.Parameter)) return Task.FromException(new BadHttpRequestException("Token no identificado"));

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                TokenValidationParameters parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config.GetSection("AppSettings:Security:Jwt:Issuer").Value,
                    ValidAudience = _config.GetSection("AppSettings:Security:Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Security:Jwt:Key").Value)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                TokenValidationResult result = await handler.ValidateTokenAsync(Auth.Parameter, parameters);
                
                if (!result.IsValid) return Task.FromException(new SecurityTokenNotYetValidException("Token no válido"));

                IEnumerable<KeyValuePair<string, string>>? client_location = result
                    .ClaimsIdentity
                    .FindAll(claim => new[] { "ip", "network", "version","latitude", "longitude", "utc_offset", "asn" }.Any(i => i == claim.Type))
                    .Select(el => new KeyValuePair<string, string>(el.Type, el.Value));


                His_Session session = new His_Session
                {
                    id = 0UL,
                    Jti = result.ClaimsIdentity.FindFirst("jti")?.Value ?? "",
                    req_ip = result.ClaimsIdentity.FindFirst("ip")?.Value ?? "",
                    client_loc = JsonSerializer.Serialize(client_location) ?? "",
                    user_id = ulong.Parse(result.ClaimsIdentity.FindFirst("user_id")?.Value ?? "0"),
                    rol_id = ulong.Parse(result.ClaimsIdentity.FindFirst("rol_id")?.Value ?? "0"),
                    exp_date = result.SecurityToken.ValidTo,
                };

                if (await base.ExistsAsync<His_Session>(
                    data => data.Jti.Equals(session.Jti)
                    && data.req_ip.Equals(session.req_ip)
                    && data.client_loc.Equals(session.client_loc)
                    && data.user_id.Equals(session.user_id)
                    && data.rol_id.Equals(session.rol_id)
                    && data.exp_date.Equals(session.exp_date)))
                {
                    His_Session? db_session = await base.FindAsync<His_Session>(
                        data => data.Jti.Equals(session.Jti)
                        && data.req_ip.Equals(session.req_ip)
                        && data.client_loc.Equals(session.client_loc)
                        && data.user_id.Equals(session.user_id)
                        && data.rol_id.Equals(session.rol_id)
                        && data.exp_date.Equals(session.exp_date));

                    DateTime exp = DateTime.UtcNow.AddMinutes(_config.GetSection("AppSettings:Security:Jwt:Conf").GetValue<int>("Expire_min"));
                    
                    db_session.exp_date = exp;

                    if (opt != null) opt(db_session);

                    Def_Rol? rol = /*await base.FindAsync<Def_Rol>(db_session.rol_id);*/ new Def_Rol { name = "agg" };
                    if (rol == null) throw new ArgumentException(nameof(db_session.rol_id));

                    ClaimsIdentity claims_identity = new ClaimsIdentity(new[]
                    {
                        new Claim("jti", db_session.Jti),
                        new Claim("user_id", db_session.user_id.ToString()),
                        new Claim("user_name", result.ClaimsIdentity.FindFirst("user_name")?.Value ?? ""),
                        new Claim("rol_id", db_session.rol_id.ToString()),
                        new Claim("rol_name", (await base.FindAsync<Def_Rol>(db_session.rol_id))?.name ?? ""),
                    });

                    IEnumerable<KeyValuePair<string, string>>? data = JsonSerializer.Deserialize<IEnumerable<KeyValuePair<string, string>>>(db_session.client_loc)?
                        .Where(el => new[] { "ip", "network", "version","latitude", "longitude", "utc_offset", "asn" }.Any(i => i == el.Key))
                        .Select(el => new KeyValuePair<string, string>(el.Key, el.Value.ToString()));

                    foreach (KeyValuePair<string, string> iterable in data)
                    {
                        claims_identity.AddClaim(new Claim(iterable.Key, iterable.Value));
                    }

                    IConfigurationSection jwt_vars = _config.GetSection("AppSettings:Security:Jwt");

                    SecurityTokenDescriptor token_desc = new SecurityTokenDescriptor
                    {
                        Subject = claims_identity,
                        Expires = exp,
                        Issuer = jwt_vars.GetValue<string>("Issuer"),
                        Audience = jwt_vars.GetValue<string>("Audience"),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt_vars.GetValue<string>("Key"))), SecurityAlgorithms.HmacSha512Signature)
                    };

                    SecurityToken sec_token = handler.CreateToken(token_desc);
                    string encoded_token = handler.WriteToken(sec_token);
                    if (_application.HttpContext.Response.Headers.Any(el => el.Key == "Token"))
                        _application.HttpContext.Response.Headers.Remove("Token");

                    _application.HttpContext.Response.Headers.Add("Token", new StringValues(encoded_token));

                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromException(new HttpListenerException((int)HttpStatusCode.Unauthorized, "Token no registrado"));
                }
            }
        }

        #endregion
    }
}
