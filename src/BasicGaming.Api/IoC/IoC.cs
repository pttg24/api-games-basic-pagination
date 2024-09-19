using BasicGaming.Api.Services;
using BasicGaming.Api.Validators;
using FluentValidation;

namespace BasicGaming.Api.IoC;

public static class IoC
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddHttpClient<IGamesService, GamesService>();
        services.AddValidatorsFromAssemblyContaining<PaginationInputValidator>();
    }
}
