using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inmersys.Infrastructure.Base.Extensions
{
    public static class App_Utilities
    {
        public static string NewJti()
        {
            DateTime now = DateTime.UtcNow;
            Guid guid = Guid.NewGuid();

            return string.Join(":", new[]
            {
                guid.ToString(),
                now.ToString("'y'yyyyy-'m'MM-'d'dd-'mm'mm-'ss'ss.ff-'zz'zz"),
            });
        }
    }
}
