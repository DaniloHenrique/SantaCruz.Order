using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace SantaCruz.Order.Controllers
{
    public class DetailedController: ControllerBase
    {
        private NotFoundObjectResult HandleNotFound(Error error) => NotFound(
            new ProblemDetails { 
                Type = "https://tools.ietf.org/html/rfc7231#section-6",
                Status = StatusCodes.Status404NotFound,
                Title = error.Code, 
                Detail = error.Description 
            });

        private ConflictObjectResult HandleConflict(Error error) => Conflict(
            new ProblemDetails { 
                Type = "https://tools.ietf.org/html/rfc7231#section-6",
                Status = StatusCodes.Status409Conflict,
                Title = error.Code, 
                Detail = error.Description 
            });

        private ObjectResult HandleFailure(Error error) => Problem(
            error.Description,
            statusCode: StatusCodes.Status500InternalServerError, 
            title: error.Code, 
            type: "https://tools.ietf.org/html/rfc7231#section-6"
        );

        private ActionResult HandleValidation(Error error) => ValidationProblem(
            error.Description,
            title: error.Code, 
            type: "https://tools.ietf.org/html/rfc7231#section-6"
        );

        private ActionResult HandleError(Error error)=>error.Type switch
            {
                ErrorType.NotFound => HandleNotFound(error),
                ErrorType.Conflict => HandleConflict(error),
                ErrorType.Failure => HandleFailure(error),
                ErrorType.Validation => HandleValidation(error),
                _ => HandleFailure(error)
            };

        protected async Task<IActionResult> Handle<T>(Func<Task<ErrorOr<T>>> method, Func<T,IActionResult> successResult)
        {
            var result = await method();

            if (result.IsError) return HandleError(result.FirstError);

            return successResult(result.Value);
        }

        protected async Task<IActionResult> Handle<T> (Func<Task<ErrorOr<Created>>> method) 
        {
            var result = await method();

            if (result.IsError) return HandleError(result.FirstError);

            return Created();
        }

    }
}
