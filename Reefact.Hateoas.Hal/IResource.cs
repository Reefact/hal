namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents that the implemented classes are HAL resources.
    /// </summary>
    public interface IResource {

        /// <summary>
        ///     Gets or sets the state of the resource, usually it is the object
        ///     that holds the domain information.
        /// </summary>
        /// <value>
        ///     The state of the resource.
        /// </value>
        object? State { get; set; }

        /// <summary>
        ///     Gets or sets the links.
        /// </summary>
        /// <value>
        ///     The links.
        /// </value>
        LinkCollection? Links { get; set; }

        /// <summary>
        ///     Gets the embedded resources.
        /// </summary>
        /// <value>
        ///     The embedded resources.
        /// </value>
        EmbeddedResourceCollection? EmbeddedResources { get; }

    }

}