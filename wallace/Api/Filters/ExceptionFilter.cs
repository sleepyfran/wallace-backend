using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Wallace.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, Action<ExceptionContext>> _handlers;

        public ExceptionFilter()
        {
            _handlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                [typeof(ValidationException)] = HandleValidationException
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

        private void HandleServerError(ExceptionContext context)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}