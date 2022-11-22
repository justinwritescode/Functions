namespace JustinWritesCode.Functions.Arrays;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using JustinWritesCode.Functions.Arrays.Models;
using Backroom.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

public class Arrays
{
    [FunctionName("Insert")]
    [OpenApiOperation(operationId: "Insert", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayValuePayload), Required = true, Description = "The array to insert the value into")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The sorted array with the value inserted")]
    public static async Task<IActionResult> InsertAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/insert")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayValuePayload>().ConfigureAwait(false);
        payload.Array.Add(payload.Value);
        payload.Array = payload.Array.Distinct().OrderBy(x => x).ToList();

        return new OkObjectResult(new ArrayPayload { Array = payload.Array });
    }

    [FunctionName("Sort")]
    [OpenApiOperation(operationId: "Sort", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/string", bodyType: typeof(List<string>), Required = true, Description = "The array to be sorted")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ArrayPayload), Description = "The sorted array")]
    public static async Task<IActionResult> SortAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/sort")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var payload = await req.ReadRequestBodyAsAsync<ArrayPayload>().ConfigureAwait(false);
        payload.Array?.Sort();

        return new OkObjectResult(payload);
    }

    [FunctionName("Reverse")]
    [OpenApiOperation(operationId: "Reverse", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/string", bodyType: typeof(List<string>), Required = true, Description = "The array to be reversed")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ArrayPayload), Description = "The reversed array")]
    public static async Task<IActionResult> ReverseAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/reverse")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var payload = await req.ReadRequestBodyAsAsync<ArrayPayload>().ConfigureAwait(false);
        payload.Array?.Reverse();

        return new OkObjectResult(payload);
    }

    [FunctionName("Remove")]
    [OpenApiOperation(operationId: "Remove", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayValuePayload), Required = true, Description = "The array to remove the value from")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The sorted array with the value removed")]
    public static async Task<IActionResult> RemoveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/remove")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayValuePayload>().ConfigureAwait(false);
        _ = payload.Array.Remove(payload.Value);
        payload.Array.Sort();

        return new OkObjectResult(new ArrayPayload { Array = payload.Array });
    }

    [FunctionName("Contains")]
    [OpenApiOperation(operationId: "Contains", tags: new[] { "arrays", "query" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayValuePayload), Required = true, Description = "The array in which to check for the value")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(bool), Description = "True if the value is in the array, false otherwise")]
    public static async Task<IActionResult> ContainsAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/contains")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayValuePayload>().ConfigureAwait(false);
        var contains = payload.Array.Contains(payload.Value);

        return new OkObjectResult(contains);
    }

    [FunctionName("IndexOf")]
    [OpenApiOperation(operationId: "IndexOf", tags: new[] { "arrays", "query" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayValuePayload), Required = true, Description = "The array to check for the value")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(int), Description = "The sorted array with the value removed")]
    public static async Task<IActionResult> IndexOfAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/indexOf")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayValuePayload>().ConfigureAwait(false);
        var index = payload.Array.IndexOf(payload.Value);

        return new OkObjectResult(index);
    }

    [FunctionName("LastIndexOf")]
    [OpenApiOperation(operationId: "LastIndexOf", tags: new[] { "arrays", "query" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayValuePayload), Required = true, Description = "The array to check for the value")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The sorted array with the value removed")]
    public static async Task<IActionResult> LastIndexOfAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/lastIndexOf")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayValuePayload>().ConfigureAwait(false);
        var index = payload.Array.LastIndexOf(payload.Value);

        return new OkObjectResult(index);
    }


    [FunctionName("AddRange")]
    [OpenApiOperation(operationId: "AddRange", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayRangePayload), Required = true, Description = "The array to insert the value into")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ArrayPayload), Description = "The sorted array with the value inserted")]
    public static async Task<IActionResult> AddRangeAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/addRange")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayRangePayload>().ConfigureAwait(false);
        payload.Array.AddRange(payload.Range);
        payload.Array.Sort();

        return new OkObjectResult(new ArrayPayload { Array = payload.Array });
    }

    [FunctionName("Distinct")]
    [OpenApiOperation(operationId: "Distinct", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayPayload), Required = true, Description = "The array in which to find distinct values")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The sorted array with only distinct values")]
    public static async Task<IActionResult> DistinctAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/distinct")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayPayload>().ConfigureAwait(false);
        payload.Array = payload.Array.Distinct().ToList();
        payload.Array.Sort();

        return new OkObjectResult(payload);
    }

    [FunctionName("Count")]
    [OpenApiOperation(operationId: "Count", tags: new[] { "arrays", "query" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayPayload), Required = true, Description = "The array whose values to count")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(int), Description = "The number of values in the array")]
    public static async Task<IActionResult> CountAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/count")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayPayload>().ConfigureAwait(false);
        var count = payload.Array.Count;

        return new OkObjectResult(count);
    }

    [FunctionName("Clear")]
    [OpenApiOperation(operationId: "Clear", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayPayload), Required = true, Description = "The array to clear")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ArrayPayload), Description = "The cleared array")]
    public static async Task<IActionResult> ClearAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/clear")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayPayload>().ConfigureAwait(false);
        payload.Array.Clear();

        return new OkObjectResult(payload);
    }

    [FunctionName("RemoveRange")]
    [OpenApiOperation(operationId: "RemoveRange", tags: new[] { "arrays", "mutation" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ArrayRangePayload), Required = true, Description = "The array to remove the values from")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ArrayPayload), Description = "The array with the values removed")]
    public static async Task<IActionResult> RemoveRangeAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "arrays/removeRange")] HttpRequest req)
    {
        var payload = await req.ReadRequestBodyAsAsync<ArrayRangePayload>().ConfigureAwait(false);
        _ = payload.Array.RemoveAll(payload.Range.Contains);

        return new OkObjectResult(new ArrayPayload { Array = payload.Array });
    }
}
