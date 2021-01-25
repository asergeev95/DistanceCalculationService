using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DCS.Host.Boostrap
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.Result = context.Exception switch
            {
                ValidationException ex => new ObjectResult(ex.Errors) {StatusCode = StatusCodes.Status400BadRequest},
                _ => context.Result
            };
            switch (context.Exception)
            {
                case ValidationException:
                    break;
            }
        }
    }
}
