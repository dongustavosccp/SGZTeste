using AuthAPI.Application.Contracts;
using AuthAPI.Domain;
using AuthAPI.Infrastructure.Helpers;
using AuthAPI.Models;
using Utils;
using Utils.DomainNotification;
using Utils.Helpers;

namespace AuthAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly INotifier _notifier;
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;
        public AuthService(INotifier notifier, IAuthRepository authRepository, JwtHelper jwtHelper)
        {
            _notifier = notifier;
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                LoginResponse response = new();

                if (string.IsNullOrWhiteSpace(request.NmUser) || string.IsNullOrWhiteSpace(request.DsPassword))
                {
                    _notifier.AddNotification(new ErrorMessage
                    {
                        Message = "Username and password must be provided."
                    });

                    return response;
                }

                // validar usuário
                User user = await _authRepository.GetUserByUserName(request.NmUser);
                if (user == null || user.DsPassword != PassEncryptor.Encrypt(request.DsPassword))
                {
                    _notifier.AddNotification(new ErrorMessage
                    {
                        Message = "Invalid username or password."
                    });
                    return response;
                }

                response.Success = true;
                response.ExpiresAt = DateTime.UtcNow.AddHours(1);
                response.UserId = user.Id;
                response.NmUser = user.NmUser;
                response.Token = _jwtHelper.GenerateToken(user);
                return response;
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage
                {
                    Message = $"An error occurred while processing your request: {ex.Message}"
                });

                return new();
            }
        }

        public async Task<ResponseClass> Register(RegisterUserRequest request)
        {
            try
            {
                ResponseClass response = new();
                if (string.IsNullOrWhiteSpace(request.NmUser) || string.IsNullOrWhiteSpace(request.DsPassword))
                {
                    _notifier.AddNotification(new ErrorMessage
                    {
                        Message = "Username and password must be provided."
                    });
                    return response;
                }

                if (_authRepository.VerifyUserExists(request.NmUser).Result)
                {
                    _notifier.AddNotification(new ErrorMessage
                    {
                        Message = "Username already exists."
                    });
                    return response;
                }

                User newUser = new()
                {
                    NmUser = request.NmUser,
                    DsPassword = PassEncryptor.Encrypt(request.DsPassword),
                    Role = request.Role
                };

                await _authRepository.AddAsync(newUser);
                response.Success = true;
                response.Message = "User registered successfully.";
                return response;
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage
                {
                    Message = $"An error occurred while processing your request: {ex.Message}"
                });
                return new();
            }
        }
    }
}
