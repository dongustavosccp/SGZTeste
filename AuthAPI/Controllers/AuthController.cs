using AuthAPI.Application.Contracts;
using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Utils.DomainNotification;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// controller for user authentication and registration.
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(INotifier notifier, IAuthService authService, IHttpContextAccessor httpContext) : base(notifier, httpContext)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login endpoint for user authentication.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            return ApiReturn(async () =>
            {
                return await _authService.Login(request);
            }, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Register endpoint for creating a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterUserRequest request)
        {
            return ApiReturn(async () =>
            {
                return await _authService.Register(request);
            }, StatusCodes.Status201Created);
        }
    }
}