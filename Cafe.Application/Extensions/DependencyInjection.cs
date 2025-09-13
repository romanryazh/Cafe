using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}