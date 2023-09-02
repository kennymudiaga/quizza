using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quizza.Common.Web.Configuration;

public static class OptionsExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        var options = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(OptionBase))).ToList();
        foreach (var optionType in options)
        {
            var attribute = optionType.GetCustomAttribute<ConfigSectionNameAttribute>();
            string configName = attribute is null ? optionType.Name : attribute.Name;
            services.AddSingleton(optionType, configuration.GetSection(configName).Get(optionType)!);
        }

        return services;
    }
}
