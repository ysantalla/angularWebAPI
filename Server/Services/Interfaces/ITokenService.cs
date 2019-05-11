using Server.Models;

namespace Server.Services.Interfaces
{
    public interface ITokenService
    {
        string Generate(ApplicationUser user);
    }
}