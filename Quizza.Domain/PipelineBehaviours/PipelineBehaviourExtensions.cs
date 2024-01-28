using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quizza.Common.PipelineBehaviours;

public static class PipelineBehaviourExtensions
{
    public static IServiceCollection AddPipelineBehaviours(
               this IServiceCollection services,
                      params Assembly[] assemblies)
    {
        //scan for all types that implement IPipelineBehavior<,>
        var behaviours = assemblies.SelectMany(x => x.GetTypes()
                   .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
                                  && !x.IsAbstract && !x.IsInterface)).ToList();
        // add all found behaviours to the service collection
        foreach (var behaviour in behaviours)
        {
            // if generic type, register as transient
            if (behaviour.IsGenericType)
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), behaviour);
                continue;
            }
            else
            {
                // if not generic, find the IPipelineBehavior<,> interfaces and register them as transient
                var interfaces = behaviour.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>));
                foreach (var @interface in interfaces)
                {
                    services.AddTransient(@interface, behaviour);
                }
            }
        }
        return services;
    }
}
