/*
 * HttpFunction.cs
 *
 *   Created: 2022-11-11-06:39:53
 *   Modified: 2022-11-11-07:10:12
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022-2023 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */


namespace JustinWritesCode.AzureFunctions;
using JustinWritesCode.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public abstract class HttpFunction<TDbContext> : HttpFunction, IHaveADbContext<TDbContext>
    where TDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    DbContext IHaveADbContext.Db => Db;
    public TDbContext Db {get;}

    public HttpFunction(ILogger<HttpFunction<TDbContext>> logger, TDbContext db) : base(logger) => Db = db;
}
