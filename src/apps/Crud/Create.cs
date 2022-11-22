// 
// Create.cs
// 
//   Created: 2022-11-12-09:46:31
//   Modified: 2022-11-12-09:46:55
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

namespace JustinWritesCode.AzureFunctions.Crud;

public class Create<T, TContext, TCreateDto, TId> : HttpFunction<TContext>, IValidator<T>
	where T : class, IIdentifiable<TId>, new()
	where TContext : DbContext
	where TId : IComparable, IEquatable<TId>
	where T : class, IIdentifiable<TId>
	where TContext : DbContext
	where TId : notnull
{
	private readonly IMapper _mapper;
	private readonly IValidator<T> _validator;

	public Create(
		ILogger<Create<T, TContext, TId>> logger,
		TContext context,
		IMapper mapper,
		IValidator<T> validator)
		: base(logger, context)
	{
	}

	public async Task<ActionResult<OneOf<> Run(
		[HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/{entityName}")] HttpRequest req,
		string entityName,
		CancellationToken cancellationToken)
	{
		var entity = await GetEntityFromBodyAsync(req, cancellationToken);
		if (entity is null)
		{
			return new BadRequestObjectResult("Invalid request body");
		}

		var validationResult = await ValidateAsync(entity, cancellationToken);
		if (!validationResult.IsValid)
		{
			return new BadRequestObjectResult(validationResult.Errors);
		}

		var entityToAdd = Mapper.Map<T>(entity);
		await Context.Set<T>().AddAsync(entityToAdd, cancellationToken);
		await Context.SaveChangesAsync(cancellationToken);

		return new OkObjectResult(entityToAdd);
	}
}
