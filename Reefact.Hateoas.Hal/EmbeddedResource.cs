namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents the embedded resource in HAL.
    /// </summary>
    /// <seealso cref="Hal.IEmbeddedResource" />
    public sealed class EmbeddedResource : IEmbeddedResource {

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbeddedResource" /> class.
        /// </summary>
        public EmbeddedResource() {
            Resources = new ResourceCollection();
        }

        #endregion

        /// <summary>
        ///     Gets or sets the name of the embedded resource.
        /// </summary>
        /// <value>
        ///     The name of the embedded resource.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the collection of resources that is represented by current embedded resource
        ///     instance.
        /// </summary>
        /// <value>
        ///     The collection of resources that is represented by current embedded resource
        ///     instance.
        /// </value>
        public ResourceCollection Resources { get; set; }

        /// <summary>
        ///     Indicates whether the embedded resource state should be always converted as an array
        ///     even if there is only one state for that embedded resource.
        /// </summary>
        public bool EnforcingArrayConverting { get; set; }

    }

}