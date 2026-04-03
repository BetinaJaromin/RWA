using RWA.Api.Models;

namespace RWA.Api.Services.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
}
