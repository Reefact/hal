#region Usings declarations

using System.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents that the implemented classes are HAL resource builders
    ///     that will initialize the <see cref="EmbeddedResource" /> collection
    ///     on the building resource.
    /// </summary>
    /// <seealso cref="Hal.Builders.IBuilder" />
    public interface IEmbeddedResourceBuilder : IBuilder {

        /// <summary>
        ///     Gets the name of the embedded resource collection.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        string Name { get; }

    }

    /// <summary>
    ///     Represents an internal implementation of the <see cref="IEmbeddedResourceBuilder" /> class.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.IEmbeddedResourceBuilder" />
    internal sealed class EmbeddedResourceBuilder : Builder, IEmbeddedResourceBuilder {

        #region Fields declarations

        private readonly bool enforcingArrayConverting;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbeddedResourceBuilder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the embedded resource collection.</param>
        /// <param name="enforcingArrayConverting">
        ///     The <see cref="bool" /> value which indicates whether the embedded resource state
        ///     should be always converted as an array even if there is only one state for that embedded resource.
        /// </param>
        public EmbeddedResourceBuilder(IBuilder context, string name, bool enforcingArrayConverting = false) : base(context) {
            Name                          = name;
            this.enforcingArrayConverting = enforcingArrayConverting;
        }

        #endregion

        /// <summary>
        ///     Gets the name of the embedded resource collection.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>
        ///     The <see cref="Resource" /> instance to be built.
        /// </returns>
        protected override Resource DoBuild(Resource resource) {
            if (resource.EmbeddedResources == null) {
                resource.EmbeddedResources = new EmbeddedResourceCollection(enforcingArrayConverting);
            }

            EmbeddedResource? embeddedResource = resource.EmbeddedResources.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name!.Equals(Name));
            if (embeddedResource == null) {
                embeddedResource = new EmbeddedResource {
                    Name = Name
                };

                resource.EmbeddedResources.Add(embeddedResource);
            }

            return resource;
        }

    }

}