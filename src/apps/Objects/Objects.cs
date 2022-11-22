namespace Backroom.Functions.Helpers.Api.Objects;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Backroom.Functions.Helpers.Api.Objects.Models;
using Backroom.Extensions;
using Backroom.Functions.Helpers.Api.Objects.EntityFrameworkCore;
using CompleteObjectPayload = Dictionary<string, string>;

public class Objects
{
    private readonly ObjectsDbContext db;

    public Objects(ObjectsDbContext db) => this.db = db;

    [FunctionName("SetProperty")]
    [OpenApiOperation(operationId: "SetProperty", tags: new[] { "objects", "properties", "mutation" })]
    [OpenApiParameter(name: "key", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The key of the property to set")]
    [OpenApiParameter(name: "objectId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID of the object whose property to set")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "The property value to set")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetObjectPropertyPayload), Description = "The object with the new property added")]
    public async Task<IActionResult> SetPropertyAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "objects/{objectId}/{key}")] HttpRequest req, string objectId, string key)
    {
        var payload = await req.ReadRequestBodyAsAsync<string>().ConfigureAwait(false);

        var objectProperty = this.db.ObjectProperties?.FirstOrDefault(o => o.ObjectId == objectId && o.Key == key);
        if (objectProperty == null)
        {
            objectProperty = new ObjectPropertyDbModel
            {
                ObjectId = objectId,
                Key = key,
                Value = payload
            };
            _ = (this.db?.ObjectProperties?.Add(objectProperty));
            _ = this.db?.SaveChanges();
            return new CreatedResult($"objects/{objectId}/{key}", new GetObjectPropertyPayload { Key = key, Value = payload });
        }
        else
        {
            objectProperty.Value = payload;
            _ = this.db?.ObjectProperties?.Update(objectProperty);
            _ = this.db?.SaveChanges();
            return new OkObjectResult(payload);
        }
    }

#pragma warning disable IDE0060 // Remove unused parameter
    [FunctionName("GetPropertyAsync")]
    [OpenApiOperation(operationId: "GetProperty", tags: new[] { "objects", "properties", "query" })]
    [OpenApiParameter(name: "key", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The key of the property to get")]
    [OpenApiParameter(name: "objectId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID of the object whose property to get")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetObjectPropertyPayload), Description = "The object with the new property added")]
    public async Task<IActionResult> GetPropertyAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "objects/{objectId}/{key}")] HttpRequest req, string objectId, string key)
    {
        var objectProperty = await Task.FromResult(this.db?.ObjectProperties?.FirstOrDefault(o => o.ObjectId == objectId && o.Key == key)).ConfigureAwait(false);
        if (objectProperty == null)
        {
            return new NotFoundResult();
        }
        else
        {
            return new OkObjectResult(objectProperty.Value);
        }
    }

    [FunctionName("DeletePropertyAsync")]
    [OpenApiOperation(operationId: "DeleteProperty", tags: new[] { "objects", "properties", "delete", "mutation" })]
    [OpenApiParameter(name: "key", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The key of the property to set")]
    [OpenApiParameter(name: "objectId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID of the object whose property to set")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetObjectPropertyPayload), Description = "The object with the new property added")]
    public async Task<IActionResult> DeletePropertyAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "objects/{objectId}/{key}")] HttpRequest req, string objectId, string key)
    {
        var objectProperty = await Task.FromResult(this.db?.ObjectProperties?.FirstOrDefault(o => o.ObjectId == objectId && o.Key == key)).ConfigureAwait(false);
        if (objectProperty == null)
        {
            return new NotFoundResult();
        }
        else
        {
            _ = this.db?.ObjectProperties?.Remove(objectProperty);
            _ = this.db?.SaveChanges();
            return new OkResult();
        }
    }

    [FunctionName("SetObjectAsync")]
    [OpenApiOperation(operationId: "SetObject", tags: new[] { "objects", "properties", "mutation" })]
    [OpenApiParameter(name: "objectId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID of the object whose property to set")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CompleteObjectPayload), Required = true, Description = "The property value to set")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetObjectPropertyPayload), Description = "The object with the new property added")]
    public async Task<IActionResult> SetObjectAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "objects/{objectId}")] HttpRequest req, string objectId)
    {
        var payload = await req.ReadRequestBodyAsAsync<CompleteObjectPayload>().ConfigureAwait(false);

        var objectProperties = this.db.ObjectProperties?.Where(o => o.ObjectId == objectId).AsEnumerable();
        if (objectProperties == null)
        {
            objectProperties = payload.Select(p => new ObjectPropertyDbModel
            {
                ObjectId = objectId,
                Key = p.Key,
                Value = p.Value
            });
            this.db?.ObjectProperties?.AddRange(objectProperties);
            _ = this.db?.SaveChanges();
            return new CreatedResult($"objects/{objectId}", payload);
        }
        else
        {
            foreach (var property in objectProperties)
            {
                if (payload.ContainsKey(property.Key))
                {
                    property.Value = payload[property.Key];
                }
                else
                {
                    _ = this.db?.ObjectProperties?.Remove(property);
                }
            }
            this.db?.ObjectProperties?.UpdateRange(objectProperties);
            _ = this.db?.SaveChanges();
            return new OkObjectResult(payload);
        }
    }

    [FunctionName("GetAsync")]
    [OpenApiOperation(operationId: "GetObject", tags: new[] { "objects", "properties", "query" })]
    [OpenApiParameter(name: "objectId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The ID of the object whose property to get")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CompleteObjectPayload), Description = "The object with the new property added")]
    public async Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "objects/{objectId}")] HttpRequest req, string objectId)
    {
        try
        {
            var objectProperties = await Task.FromResult(this.db?.ObjectProperties?.Where(o => o.ObjectId == objectId).AsEnumerable()).ConfigureAwait(false);
            if (objectProperties == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(objectProperties.ToDictionary(o => o.Key, o => o.Value));
            }
        }
#pragma warning disable IDE0055 // Fix formatting
        catch(Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }
}
