using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quizza.Common.PipelineBehaviours;

public static class PipelineBehaviourExtensions
{
    public static IServiceCollection RegisterPipelineBehaviours(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var behaviours = assemblies.SelectMany(x => x.GetTypes()
            .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
                && !x.IsAbstract && !x.IsInterface)).ToList();
        foreach(var  behaviour in behaviours)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), behaviour);
        }
        return services;
    }
}
