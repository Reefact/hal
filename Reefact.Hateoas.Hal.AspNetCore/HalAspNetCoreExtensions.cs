#region Usings declarations

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace Reefact.Hateoas.Hal.AspNetCore {

    /// <summary>
    ///     Represents the class that holds the extension methods for HAL support in ASP.NET Core.
    /// </summary>
    public static class HalAspNetCoreExtensions {

        #region Statics members declarations

        /// <summary>
        ///     Adds the HAL support to the ASP.NET Core application.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> instance to which the HAL support is added.</param>
        /// <param name="options">The HAL options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHalSupport(this IServiceCollection serviceCollection, Action<SupportsHalOptions> options) {
            serviceCollection.Configure(options);
            serviceCollection.Configure<JsonSerializerSettings>(settings => {
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting        = Formatting.None;
                settings.ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
            serviceCollection.AddScoped<SupportsHalAttribute>();

            return serviceCollection;
        }

        /// <summary>
        ///     Adds the HAL support to the ASP.NET Core application.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> instance to which the HAL support is added.</param>
        /// <param name="options">The HAL options.</param>
        /// <param name="jsonSerializerSettings">
        ///     The settings of the Json serializer that determines how the result JSON should be
        ///     generated.
        /// </param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHalSupport(this IServiceCollection serviceCollection, Action<SupportsHalOptions> options, Action<JsonSerializerSettings> jsonSerializerSettings) {
            serviceCollection.Configure(options);
            serviceCollection.Configure(jsonSerializerSettings);
            serviceCollection.AddScoped<SupportsHalAttribute>();

            return serviceCollection;
        }

        /// <summary>
        ///     Adds the HAL support to the ASP.NET Core application with default options.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> instance to which the HAL support is added.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHalSupport(this IServiceCollection serviceCollection) {
            serviceCollection.Configure<SupportsHalOptions>(options => {
                options.Enabled        = true;
                options.IdPropertyName = "Id";
            });
            serviceCollection.Configure<JsonSerializerSettings>(settings => {
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting        = Formatting.None;
                settings.ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
            serviceCollection.AddScoped<SupportsHalAttribute>();

            return serviceCollection;
        }

        /// <summary>
        ///     Adds the HAL support to the ASP.NET Core application.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" /> instance to which the HAL support is added.</param>
        /// <param name="configSection">The configuration section that holds the HAL configuration options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHalSupport(this IServiceCollection serviceCollection, IConfigurationSection configSection) {
            serviceCollection.Configure<SupportsHalOptions>(configSection);
            serviceCollection.Configure<JsonSerializerSettings>(settings => {
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting        = Formatting.None;
                settings.ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });
            serviceCollection.AddScoped<SupportsHalAttribute>();

            return serviceCollection;
        }

        /// <summary>
        ///     Adds the HAL support to the ASP.NET Core application.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection" />; instance to which the HAL support is added.</param>
        /// <param name="configSection">The configuration section that holds the HAL configuration options.</param>
        /// <param name="jsonSerializerSettings">
        ///     The settings of the Json serializer that determines how the result JSON should be
        ///     generated.
        /// </param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHalSupport(this IServiceCollection serviceCollection, IConfigurationSection configSection, Action<JsonSerializerSettings> jsonSerializerSettings) {
            serviceCollection.Configure<SupportsHalOptions>(configSection);
            serviceCollection.Configure(jsonSerializerSettings);
            serviceCollection.AddScoped<SupportsHalAttribute>();

            return serviceCollection;
        }

        #endregion

    }

}