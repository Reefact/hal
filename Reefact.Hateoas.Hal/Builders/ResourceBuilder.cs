namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents that the implemented classes are the HAL resource
    ///     builders that simply returned the built HAL resource.
    /// </summary>
    /// <seealso cref="Hal.Builders.IBuilder" />
    public interface IResourceBuilder : IBuilder { }

    /// <summary>
    ///     Represents the HAL resource builder.
    /// </summary>
    /// <seealso cref="Hal.Builders.IResourceBuilder" />
    public sealed class ResourceBuilder : IResourceBuilder {

        #region Fields declarations

        private readonly Resource resource = new();

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceBuilder" /> class.
        /// </summary>
        public ResourceBuilder() { }

        #endregion

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <returns>The <see cref="Resource" /> instance to be built.</returns>
        public Resource Build() {
            return resource;
        }

    }

}