using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wallace.Domain.Exceptions;

namespace Wallace.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, Action<ExceptionContext>> _handlers;

        public ExceptionFilter()
        {
            _handlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                [typeof(ValidationException)] = HandleValidationException,
                [typeof(UserNotFoundException)] = context => 
                    HandleCodeResultException(
                        context,
                        StatusCodes.Status404NotFound
                    ),
                [typeof(InvalidCredentialException)] = context => 
                    HandleCodeResultException(
                        context,
                        StatusCodes.Status401Unauthorized
                    )
            };
        }

        public override void OnException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_handlers.ContainsKey(type))
            {
                _handlers[type].Invoke(context);
            }
            else
            {
                HandleServerError(context);
            }
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;
            var mappedErrors = exception
                .Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .Select(e => e.ErrorMessage)
                        .ToArray()
                );
            
            var details = new ValidationProblemDetails(mappedErrors)
            {
                Title = "Validation error",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Generic handler for all exceptions that simply need to return a
        /// custom status code.
        /// </summary>
        private void HandleCodeResultException(
            ExceptionContext context,
            int resultCode
        )
        {
            context.Result = new StatusCodeResult(resultCode);
            context.ExceptionHandled = true;
        }

        private void HandleServerError(ExceptionContext context)
        {
            Console.WriteLine(context.Exception);
            
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}