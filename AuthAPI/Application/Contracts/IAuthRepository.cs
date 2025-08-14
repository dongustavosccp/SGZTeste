using AuthAPI.Domain;

namespace AuthAPI.Application.Contracts
{
    public interface IAuthRepository
    {
        Task<User> GetUserByUserName(string username);
        Task<bool> VerifyUserExists(string username);
        Task AddAsync(User user);
    }
}