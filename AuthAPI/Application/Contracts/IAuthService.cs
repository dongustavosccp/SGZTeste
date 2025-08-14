using AuthAPI.Models;
using Utils;

namespace AuthAPI.Application.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<ResponseClass> Register(RegisterUserRequest request);
    }
}