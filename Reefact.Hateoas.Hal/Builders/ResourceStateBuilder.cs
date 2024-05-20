namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents an internal implementation of <see cref="ResourceStateBuilder" /> interface.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.ResourceStateBuilder" />
    public sealed class ResourceStateBuilder : Builder {

        #region Fields declarations

        private readonly object state;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceStateBuilder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="state">The state of the resource.</param>
        public ResourceStateBuilder(IBuilder context, object state) : base(context) {
            this.state = state;
        }

        #endregion

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>
        ///     The <see cref="Resource" /> instance to be built.
        /// </returns>
        protected override Resource DoBuild(Resource resource) {
            resource.State = state;

            return resource;
        }

    }

}