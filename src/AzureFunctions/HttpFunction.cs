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

namespace JustinWritesCode.AzureFunctions;

public abstract class HttpFunction : JustinWritesCode.Abstractions.ILog
{  
    public ILogger Logger {get;}
    public string Name => GetType().Name;

    public HttpFunction(ILogger<HttpFunction> logger)
    {
        Logger = logger;
    }

    public OkObjectResult Ok<T>(T value) => new OkObjectResult(value);
    public NotFoundObjectResult NotFound<T>(T value) => new NotFoundObjectResult(value);
    public CreatedResult Created<T>(string location, T value) => new CreatedResult(location, value);
}
