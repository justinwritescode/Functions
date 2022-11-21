using System.Net.Mime;
/*
 * IsEven.cs
 *
 *   Created: 2022-11-19-11:38:37
 *   Modified: 2022-11-19-11:38:37
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */


namespace JustinWritesCode.Functions.Numbers;
using Microsoft.Extensions.Logging;
using JustinWritesCode.Http.Extensions;

    public class IsEven
    {
        private readonly ILogger<IsEven> _logger;

        public IsEven(ILogger<IsEven> log)
        {
            _logger = log;
        }

        [Function(nameof(IsEven))]
        [Op(operationId: nameof(IsEven), tags: new[] { "name" })]
        [Param(name: Microsoft.Net.Http.Headers.HeaderNames.ContentType, In = Header, Required = true, Type = typeof(string), Description = "The type of the request payload")]
		[Param(name: Microsoft.Net.Http.Headers.HeaderNames.Accept, In = Header, Required = true, Type = typeof(string), Description = "The type of the request payload")]
        [Response(statusCode: OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, string Accept = Text.Plain)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var number =
				req.ContentType switch
				{
					Application.Json => (await req.ReadRequestBodyAsAsync<NumericPayload>().ConfigureAwait(false)).Value,
					Text.Plain => decimal.Parse(await new StreamReader(req.Body).ReadToEndAsync()),
					_ => 0
				};

			var responseMessage = number % 2 == 0 ? "Even" : "Odd";

			return new OkObjectResult(new BooleanPayload(number % 2 == 0));
		}
	}
