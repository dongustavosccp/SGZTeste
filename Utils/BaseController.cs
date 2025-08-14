using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Utils.DomainNotification;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Classe base dos controladores contendo métodos relevantes ao consumo da api
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected readonly INotifier _notifier;
        protected readonly IHttpContextAccessor _httpContext;

        public BaseController(INotifier notifier, IHttpContextAccessor httpContext)
        {
            _notifier = notifier;
            _httpContext = httpContext;
        }

        protected bool IsValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected IActionResult ApiReturn<T>(Func<T> func, int status = StatusCodes.Status200OK)
        {
            try
            {
                if (!_notifier.HasNotification())
                {
                    var modelState = _httpContext.HttpContext?.Items["ModelState"] as ModelStateDictionary;
                    if (modelState != null && !modelState.IsValid)
                    {
                        NotifyModelError(modelState);
                    }
                }

                var result = func();
                if (result is Task task && result.GetType().IsGenericType)
                {
                    var resultAsync = result.GetType().GetProperty("Result")?.GetValue(result);
                    return ApiTypedReturn(resultAsync, status);
                }
                return ApiTypedReturn(result, status);
            }
            catch (Exception ex)
            {
                var objError = new ObjectResult(_notifier.GetNotifications());
                if (ex is ArgumentException)
                {
                    objError.StatusCode = StatusCodes.Status400BadRequest;
                }
                else
                {
                    objError.StatusCode = StatusCodes.Status500InternalServerError;
                }

                if (!_notifier.HasNotification())
                {
                    _notifier.AddNotification(ex.Message);
                }
                return objError;
            }
        }

        protected ActionResult ApiTypedReturn<T>(T result, int status = StatusCodes.Status200OK)
        {
            if (result is ActionResult ar)
                return ar;

            if (IsValidOperation())
            {
                return new ObjectResult(result) { StatusCode = status };
            }
            return BadRequestAPI();
        }

        protected ActionResult CreatedAPI<T>(T result)
        {
            if (IsValidOperation())
            {
                return Created(string.Empty, result);
            }
            return BadRequestAPI();
        }

        /// <summary>
        /// Retorno de erro da api com lista de notificações
        /// </summary>
        protected ObjectResult BadRequestAPI()
        {
            return BadRequest(_notifier.GetNotifications().ToList());
        }

        protected ObjectResult InternalServerErrorAPI()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, _notifier.GetNotifications().ToList());
        }

        protected void NotifyModelError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string message)
        {
            _notifier.AddNotification(message);
        }
    }
}