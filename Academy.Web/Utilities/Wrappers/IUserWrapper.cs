using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Academy.Web.Utilities.Wrappers
{
    public interface IUserWrapper
    {
        string GetUserId(ClaimsPrincipal user);
    }
}
