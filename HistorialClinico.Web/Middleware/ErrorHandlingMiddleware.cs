using HistorialClinico.Common.Configuration;
using HistorialClinico.Common.Exceptions;
using HistorialClinico.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace HistorialClinico.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger _logger;
        private readonly IEmailSender _email;
        private readonly AppSettings _appSettings;

        public ErrorHandlingMiddleware(RequestDelegate next, IEmailSender email, ILoggerFactory loggerFactory, IOptions<AppSettings> appSettings)
        {
            _email = email;
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
            _appSettings = appSettings.Value;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (!(ex is CustomException))
                {
                    _logger.LogError(ex, "An unexpected error has ocurred");

//#if !DEBUG
                    //try
                    //{
                    //    await _email.SendEmailAsync(_appSettings.AdminEmail, "Historial Clínico - Application Error", $"Error: {ex.Message}");
                    //}
                    //catch(Exception ex2)
                    //{
                    //    _logger.LogError(ex2, "An unexpected error has ocurred to send Error email");
                    //}
//#endif
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error_msg = "Error inesperado. Si persiste, favor contactar con el administrador del Sistema";

            if (exception is CustomException)
            {
                error_msg = exception.Message;
            }

            var isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (!isAjax)
            {
                return Task.Run(() => context.Response.Redirect($"/Home/Error?error={error_msg}"));
            }
            else
            {
                string result = JsonConvert.SerializeObject(new { Success = false, ErrorMessage = error_msg });
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(result);
            }
        }
    }

}
