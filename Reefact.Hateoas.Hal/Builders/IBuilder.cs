namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents that the implemented classes are the builders that can
    ///     build the HAL <see cref="Resource" /> instance.
    /// </summary>
    public interface IBuilder {

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <returns>The <see cref="Resource" /> instance to be built.</returns>
        Resource Build();

    }

}