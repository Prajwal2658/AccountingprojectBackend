namespace AccountingSystem.Application.Authentication
{
    public interface IAuthRepository
    {
        Task<AuthUser?> GetUserAsync(string username);
    }
}