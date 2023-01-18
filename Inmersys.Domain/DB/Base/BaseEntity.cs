using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inmersys.Domain.DB.Base
{
    public abstract class BaseEntity
    {
        public ulong id { get; set; }
    }
}
