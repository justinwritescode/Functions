/*
 * HttpFunction.cs
 *
 *   Created: 2022-11-11-06:39:53
 *   Modified: 2022-11-11-07:10:12
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using JustinWritesCode.Payloads;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Host.Diagnostics;
using Microsoft.Extensions.Logging;

namespace JustinWritesCode.AzureFunctions;

public abstract class HttpFunction : JustinWritesCode.Abstractions.ILog
{
    public ILogger Logger {get;}
    public string Name => GetType().Name;

    public HttpFunction(ILogger<HttpFunction> logger)
    {
        Logger = logger;
    }

    public static IActionResult Ok<T>(T? value = default) => value is null ? new OkResult() : new OkObjectResult(value);
    public static IActionResult NotFound<T>(T? value = default) => value is null ? new NotFoundResult() : new NotFoundObjectResult(value);
    public static IActionResult Created<T>(string? location = null, T? value = default) =>
        location is not null  && value is not null ? new CreatedResult(location, value) :
        location is not null ? new CreatedResult(location, null) : new CreatedResult(null as string, null);
    public static IActionResult Accepted<T>(string? location = null, T? value = default) =>
        location is not null  && value is not null ? new AcceptedResult(location, value) :
        location is not null ? new AcceptedResult(location, null) : new AcceptedResult(null as string, null);
    public static IActionResult BadRequest<T>(T? value = default) => value is null ? new BadRequestResult() : new BadRequestObjectResult(value);
    public static IActionResult Unauthorized<T>(T? value = default) => value is null ? new UnauthorizedResult() : new UnauthorizedObjectResult(value);
    public static IActionResult Forbidden<T>(T? value = default) =>
        value is null ? new ForbidResult() :
        value is string s ? new ForbidResult(s) :
        value is IList<string> l ? new ForbidResult(l) :
        value is AuthenticationProperties ap ? new ForbidResult(ap) :
        new ForbidResult();
    public static IActionResult Conflict<T>(T? value = default) => value is null ? new ConflictResult() : new ConflictObjectResult(value);
    public static IActionResult NoContent() => new NoContentResult();
    public static IActionResult NotModified() => new StatusCodeResult(304);
    public static IActionResult UnprocessableEntity<T>(T? value = default) => value is null ? new UnprocessableEntityResult() : new UnprocessableEntityObjectResult(value);
    public static IActionResult InternalServerError<TEx>(TEx? value = default) where TEx : Exception =>
        value is null ? new StatusCodeResult(500) : new ObjectResult(new ErrorResponsePayload(value)) { StatusCode = 500 };
}
