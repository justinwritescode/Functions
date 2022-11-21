/*
 * FormatOutputExtensions.cs
 *
 *   Created: 2022-11-11-01:11:56
 *   Modified: 2022-11-11-01:11:56
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
namespace JustinWritesCode.AzureFunctions;
using JustinWritesCode.Payloads;
public static class FormatOutputExtensions
{
    public static IActionResult FormatOutput<T>(this object? value, string acceptContentType, int pageNumber = 1, int pageSize = 10, int totalItems = int.MaxValue)
    {
        if (value is null || (value is IQueryable<T> queryable && !queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).Any()))
        {
            return new NotFoundObjectResult("There were no results for the specified query");
        }
        if (value is IQueryable<T> queryableT/* || value is IEnumerable<T> enumerableT*/)
        {
            return acceptContentType switch
            {
				Application.Json =>
                    pageSize == 1 ?
                    new OkObjectResult(new SingleItemPager<T>(queryableT.Skip(pageNumber - 1).FirstOrDefault(), pageNumber, queryableT.Count())) :
                    new OkObjectResult(new Pager<T>(queryableT.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, queryableT.Count())),
				ContentType.JsonPlainText.DisplayName =>
                    pageSize == 1 ?
                    new OkObjectResult(new SingleItemPager<string>(queryableT.Skip(pageNumber - 1).FirstOrDefault().ToString(), pageNumber, queryableT.Count())) :
                    new OkObjectResult(new SingleItemPager<string>(string.Join("\n", queryableT.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(i => i.ToString())), pageNumber, queryableT.Count() / pageSize)),
				ContentType.Text.DisplayName
                    => new OkObjectResult(string.Join("\n", queryableT.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(i => i.ToString()))),
                _ => new BadRequestObjectResult("Invalid Accept header")
            };
        }
        else if (value is T)
        {
            return acceptContentType switch
            {
				Application.Json => new OkObjectResult(new SingleItemPager<T>((T)value, pageNumber, 1)),
				ContentType.JsonPlainText.DisplayName => new OkObjectResult(new SingleItemPager<string>(value.ToString(), pageNumber, 1)),
				ContentType.Text.DisplayName => new OkObjectResult(value.ToString()),
                _ => new BadRequestObjectResult("Invalid Accept header")
            };
        }

        else throw new NotSupportedException();
    }
}
