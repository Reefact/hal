#region Usings declarations

using System.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents that the implemented classes are HAL resource builders that
    ///     will add an embedded resource to the embedded resource collection of
    ///     the building HAL resource.
    /// </summary>
    /// <seealso cref="Hal.Builders.IBuilder" />
    public interface IEmbeddedResourceItemBuilder : IBuilder {

        /// <summary>
        ///     Gets the name of the embedded resource collection.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        string Name { get; }

    }

    /// <summary>
    ///     Represents an internal implementation of the <see cref="IEmbeddedResourceItemBuilder" /> class.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.IEmbeddedResourceItemBuilder" />
    internal sealed class EmbeddedResourceItemBuilder : Builder, IEmbeddedResourceItemBuilder {

        #region Fields declarations

        private readonly IBuilder resourceBuilder;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbeddedResourceItemBuilder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name of the embedded resource collection.</param>
        /// <param name="resourceBuilder">The resource builder that will build the embedded resource.</param>
        public EmbeddedResourceItemBuilder(IBuilder context, string name, IBuilder resourceBuilder) : base(context) {
            Name                 = name;
            this.resourceBuilder = resourceBuilder;
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
            EmbeddedResource? embeddedResource = resource.EmbeddedResources?.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name!.Equals(Name));
            if (embeddedResource == null) {
                embeddedResource = new EmbeddedResource {
                    Name = Name,
                    Resources = new ResourceCollection {
                        resourceBuilder.Build()
                    }
                };
                resource.EmbeddedResources?.Add(embeddedResource);
            } else {
                embeddedResource.Resources.Add(resourceBuilder.Build());
            }

            return resource;
        }

    }

}