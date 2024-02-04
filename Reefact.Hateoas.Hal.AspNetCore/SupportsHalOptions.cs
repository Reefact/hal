namespace Reefact.Hateoas.Hal.AspNetCore {

    /// <summary>
    ///     Represents the options to configure the HAL support in ASP.NET Core Web API applications.
    /// </summary>
    public sealed class SupportsHalOptions {

        /// <summary>
        ///     Gets or sets a <see cref="bool" /> value which indicates whether
        ///     the HAL feature should be enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets the name of the Id property of the object that represents
        ///     the embedded resource.
        /// </summary>
        public string IdPropertyName { get; set; } = "Id";

        /// <summary>
        ///     Gets or sets a <see cref="bool" /> value which indicates if the HTTPS scheme
        ///     should be used when generating the href links. By default, the value will
        ///     be inferred from the type of the hosting environment. If it is Production,
        ///     the HTTPS scheme will be used, otherwise, it will be HTTP.
        /// </summary>
        public bool? UseHttpsScheme { get; set; }

    }

}