using AccountingSystem.Application.Authentication.DTOs;

namespace AccountingSystem.Application.Authentication
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}