using System.Collections.Generic;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface ITokenService
    {
        string Generate(ApplicationUser user, List<string> roles);
    }
}