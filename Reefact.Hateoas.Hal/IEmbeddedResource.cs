namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents that the implemented classes are embedded resources.
    /// </summary>
    public interface IEmbeddedResource {

        /// <summary>
        ///     Gets or sets the name of the embedded resource.
        /// </summary>
        /// <value>
        ///     The name of the embedded resource.
        /// </value>
        string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the collection of resources that is represented by current embedded resource
        ///     instance.
        /// </summary>
        /// <value>
        ///     The collection of resources that is represented by current embedded resource
        ///     instance.
        /// </value>
        ResourceCollection Resources { get; set; }

        /// <summary>
        ///     Indicates whether the embedded resource state should be always converted as an array
        ///     even if there is only one state for that embedded resource.
        /// </summary>
        bool EnforcingArrayConverting { get; set; }

    }

}