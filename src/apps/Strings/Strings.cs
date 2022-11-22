using System.Security.AccessControl;
namespace Backroom.Functions.Helpers.Api.Strings;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Backroom.Extensions;
using Microsoft.Extensions.Logging;
using static System.Text.RegularExpressions.Regex;
using System.Text.RegularExpressions;

public class Strings
{
    private readonly ILogger logger;

    public Strings(ILogger<Strings> logger) => this.logger = logger;

    [FunctionName("Echo")]
    [OpenApiOperation(operationId: "Echo", tags: new[] { "strings" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "The string to echo")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The echoed string")]
    public async Task<IActionResult> EchoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "strings/echo")] HttpRequest req)
    {
        this.logger.LogInformation("Echoing string");
        var payload = await req.ReadRequestBodyAsAsync<StringPayload>().ConfigureAwait(false);
        return new OkObjectResult(payload);
    }

    [FunctionName("RegexIsMatch")]
    [OpenApiOperation(operationId: "RegexIsMatch", tags: new[] { "strings" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "The string to echo")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The echoed string")]
    public async Task<IActionResult> RegexIsMatch(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "strings/regex/is-match")] HttpRequest req)
    {
        this.logger.LogInformation("Echoing string");
        var payload = await req.ReadRequestBodyAsAsync<StringWithRegexPayload>().ConfigureAwait(false);
        return new OkObjectResult(new BooleanPayload(Regex.IsMatch(payload.Value, payload.Regex)));
    }

    [FunctionName("IsUsername")]
    [OpenApiOperation(operationId: "IsUsername", tags: new[] { "strings" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The echoed string")]
    public IActionResult IsUsername(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "strings/{username}/is-username/")] HttpRequest req, string username)
    {
        this.logger.LogInformation("Testing if string is a username");
        return new OkObjectResult(new BooleanPayload(Regex.IsMatch(username, "@[a-zA-Z0-9_]+")));
    }
}

public record StringPayload(string Value);
public record StringWithSuccessPayload(string Value, bool Success);
public record StringWithRegexPayload(string Value, string Regex) : StringPayload(Value);
