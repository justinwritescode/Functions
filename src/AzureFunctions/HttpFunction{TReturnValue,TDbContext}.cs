/*
 * HttpFunction{TReturnValue,TDbContext}.cs
 *
 *   Created: 2022-11-11-07:12:18
 *   Modified: 2022-11-11-11:48:41
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022-2023 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace JustinWritesCode.AzureFunctions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public abstract class HttpFunction<TReturnValue, TDbContext> : HttpFunction<TDbContext>
    where TDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public HttpFunction(ILogger<HttpFunction<TReturnValue, TDbContext>> logger, TDbContext db) : base(logger, db)
    {
    }

    public abstract Task<ActionResult<TReturnValue>> RunAsync(HttpRequest req, CancellationToken cancellationToken);
}
