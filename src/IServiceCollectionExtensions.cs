using System.Reflection;
using AutoOptionRegistration.MarkerAttributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoOptionRegistration;

public static class IServiceCollectionExtensions {
    /// <summary>
    ///     Registers Options to ServiceCollection
    /// </summary>
    /// <param name="this">The <see cref="IServiceCollection" /> to register to</param>
    /// <param name="configuration"></param>
    /// <param name="assemblies">The <see cref="Assembly" /> to scan for <see cref="RegisterOptionAttribute" /> attributes</param>
    /// <returns>The modified <see cref="IServiceCollection" /> to enable method chaining</returns>
    public static IServiceCollection RegisterOptions(this IServiceCollection @this, IConfiguration configuration,
        params Assembly[] assemblies) {
        foreach (var assembly in assemblies.Distinct()) {
            IEnumerable<(Type OptionType, string? OptionName)> options = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<RegisterOptionAttribute>() is not null)
                .Select(t => (t, t.GetCustomAttribute<RegisterOptionAttribute>()!.OptionName));

            foreach (var option in options) {
                // We want this exact same functionality, but with reflection, because these methods do not have a
                // non-generic version
                // @this.AddOptions<OptionType>()
                // .Bind(configuration.GetSection(OptionName))
                // .ValidateDataAnnotations() // Optional
                // .ValidateOnStart(); // Optional

                var optionName = option.OptionName ?? option.OptionType.Name;

                var optionsBuilder = GetAddOptionsMethod().MakeGenericMethod(option.OptionType).Invoke(null, [@this])!;

                // Bind configuration
                GetBindMethod(option.OptionType).MakeGenericMethod(option.OptionType)
                    .Invoke(null, [optionsBuilder, configuration.GetSection(optionName)]);

                var validationType = option.OptionType.GetCustomAttribute<RegisterOptionAttribute>()!.Validate;
                if (validationType is RegisterOptionAttribute.ValidationType.ValidateDataAnnotationsOnStart
                    or RegisterOptionAttribute.ValidationType.ValidateDataAnnotationsWhenAccess)
                    // Validate DataAnnotations
                    GetValidateAnnotationsMethod(optionsBuilder.GetType()).MakeGenericMethod(option.OptionType)
                        .Invoke(null, [optionsBuilder]);

                if (validationType is RegisterOptionAttribute.ValidationType.ValidateDataAnnotationsOnStart)
                    // Validate on start
                    GetValidateOnStartMethod(optionsBuilder.GetType()).MakeGenericMethod(option.OptionType)
                        .Invoke(null, [optionsBuilder]);
            }
        }

        return @this;
    }

    /// <summary>
    ///     Assumes that the collection contains only one method, and retrieves it
    /// </summary>
    /// <param name="this">The collection which assumed to be contains only one element </param>
    /// <param name="methodName">Optional for more verbose exception messages</param>
    /// <returns>The <see cref="MethodInfo" /> which was the only element in <paramref name="this" /> </returns>
    /// <exception cref="AmbiguousMatchException">In case of multiple items found in <paramref name="this" /> or it was empty</exception>
    private static MethodInfo SingleOrException(this IEnumerable<MethodInfo> @this, string? methodName = null) {
        MethodInfo? onlyMatchedMethod = null;


        var n = 0;
        foreach (var methodInfo in @this) {
            n++;
            if (n > 1) {
                throw new AmbiguousMatchException("More than one" + methodName + "method found");
            }

            onlyMatchedMethod = methodInfo;
        }

        return onlyMatchedMethod ?? throw new AmbiguousMatchException("No " + methodName + "method found");
    }

    private static MethodInfo GetAddOptionsMethod() {
        return typeof(OptionsServiceCollectionExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => {
                var parameterInfos = m.GetParameters();
                return m is { Name: nameof(OptionsServiceCollectionExtensions.AddOptions), IsGenericMethod: true }
                       && m.GetGenericArguments().Length == 1
                       && parameterInfos.Length == 1
                       && parameterInfos[0].ParameterType == typeof(IServiceCollection);
            }).SingleOrException(nameof(OptionsServiceCollectionExtensions.AddOptions));
    }

    private static MethodInfo GetBindMethod(Type optionsType) {
        return typeof(OptionsBuilderConfigurationExtensions).GetMethods(
                BindingFlags.Public | BindingFlags.Static)
            .Where(m => {
                var parameterInfos = m.GetParameters();
                return m is { Name: nameof(OptionsBuilderConfigurationExtensions.Bind), IsGenericMethod: true }
                       && m.GetGenericArguments().Length == 1
                       && parameterInfos.Length == 2
                       // We don't validate the first argument type because it contains Type constraints, and to be honest
                       // I don't know how to do it
                       && parameterInfos[1].ParameterType == typeof(IConfiguration);
            }).SingleOrException(nameof(OptionsBuilderConfigurationExtensions.Bind));
    }

    private static MethodInfo GetValidateAnnotationsMethod(Type optionBuilderType) {
        return typeof(OptionsBuilderDataAnnotationsExtensions).GetMethods(
                BindingFlags.Public | BindingFlags.Static)
            .Where(m => {
                var parameterInfos = m.GetParameters();
                return m is
                       {
                           Name: nameof(OptionsBuilderDataAnnotationsExtensions.ValidateDataAnnotations),
                           IsGenericMethod: true
                       }
                       && m.GetGenericArguments().Length == 1
                       && parameterInfos.Length == 1;
            }).SingleOrException(nameof(OptionsBuilderDataAnnotationsExtensions.ValidateDataAnnotations));
    }

    private static MethodInfo GetValidateOnStartMethod(Type optionBuilderType) {
        return typeof(OptionsBuilderExtensions).GetMethods(
                BindingFlags.Public | BindingFlags.Static)
            .Where(m => {
                var parameterInfos = m.GetParameters();
                return m is
                       {
                           Name: nameof(OptionsBuilderExtensions.ValidateOnStart),
                           IsGenericMethod: true
                       }
                       && m.GetGenericArguments().Length == 1
                       && parameterInfos.Length == 1;
            }).SingleOrException(nameof(OptionsBuilderExtensions.ValidateOnStart));
    }
}