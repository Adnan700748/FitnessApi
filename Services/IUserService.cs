namespace FitnessApi.Services;

public interface IUserService
{
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<string> GetUserRoleAsync(string email);
}