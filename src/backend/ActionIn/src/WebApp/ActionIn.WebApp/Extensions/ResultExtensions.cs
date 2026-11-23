using ActionIn.Core.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ActionIn.WebApp.Extensions;

public static class ResultExtensions
{
    extension(Result result)
    {
        public IActionResult ToActionResult(ControllerBase controller, Func<IActionResult>? onSuccess = null)
        {
            return result.Match(
                onSuccess: onSuccess ?? (() => controller.Ok()),
                onFailure: error => error.Type == ErrorType.Validation
                    ? ToValidationProblem(controller, error)
                    : ToProblem(controller, error));
        }
    }

    private static ObjectResult ToProblem(ControllerBase controller, Error error)
    {
        var problemDetails = controller.ProblemDetailsFactory.CreateProblemDetails(
                controller.HttpContext,
                statusCode: ToStatusCode(error.Type),
                detail: error.Message
        );

        problemDetails.Extensions["code"] = error.Code;

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    private static ObjectResult ToValidationProblem(ControllerBase controller, Error error)
    {
        var modelState = new ModelStateDictionary();

        foreach (var fieldError in error.FieldErrors)
            modelState.AddModelError(fieldError.Field, fieldError.Message);

        var problemDetails = controller.ProblemDetailsFactory.CreateValidationProblemDetails(
                controller.HttpContext,
                modelState,
                statusCode: StatusCodes.Status400BadRequest,
                detail: error.Message
        );

        problemDetails.Extensions["code"] = error.Code;

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    private static int ToStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

}


