using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Exceptions;

namespace CipherLock.Presentation.Middlewares;

public static class ApiExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;

                    context.Response.StatusCode = exception switch
                    {
                        CredentialNotFoundException =>
                            StatusCodes.Status404NotFound,
                        
                        VaultNotFoundException =>
                            StatusCodes.Status404NotFound,

                        InvalidPasswordLengthException =>
                            StatusCodes.Status400BadRequest,
                        
                        InvalidCredentialsException =>
                            StatusCodes.Status400BadRequest,
                        
                        UserNotFoundException =>
                            StatusCodes.Status404NotFound,

                        _ =>
                            StatusCodes.Status500InternalServerError
                    };

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = env.IsDevelopment()
                            ? contextFeature.Error.StackTrace
                            : null
                    }.ToString());
                }
            });
        });
    }
}
