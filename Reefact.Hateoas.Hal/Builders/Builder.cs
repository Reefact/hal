namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents the base class for all the HAL builders.
    /// </summary>
    /// <seealso cref="Hal.Builders.IBuilder" />
    public abstract class Builder : IBuilder {

        #region Fields declarations

        private readonly IBuilder context;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="Builder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected Builder(IBuilder context) {
            this.context = context;
        }

        #endregion

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <returns>
        ///     The <see cref="Resource" /> instance to be built.
        /// </returns>
        public Resource Build() {
            Resource resource = context.Build();

            return DoBuild(resource);
        }

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <returns>
        ///     The <see cref="Resource" /> instance to be built.
        /// </returns>
        protected abstract Resource DoBuild(Resource resource);

    }

}