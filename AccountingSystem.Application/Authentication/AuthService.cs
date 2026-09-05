using AccountingSystem.Application.Authentication.DTOs;

namespace AccountingSystem.Application.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IAuthRepository authRepository,
            IJwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request)
        {
            var user =
                await _authRepository.GetUserAsync(
                    request.Username);

            if (user == null)
                return null;

            var passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!passwordValid)
                return null;

            var token =
                _jwtTokenService.GenerateToken(
                    user.Id,
                    user.Username,
                    user.Roles);

            return new LoginResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}