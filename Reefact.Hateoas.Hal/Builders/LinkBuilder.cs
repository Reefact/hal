#region Usings declarations

using System.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents that the implemented classes are HAL resource builders
    ///     that are responsible for adding the <see cref="ILink" /> instance
    ///     to the HAL resource.
    /// </summary>
    /// <seealso cref="Hal.Builders.IBuilder" />
    public interface ILinkBuilder : IBuilder {

        /// <summary>
        ///     Gets the relation of the resource location.
        /// </summary>
        /// <value>
        ///     The relation of the resource location.
        /// </value>
        string Rel { get; }

        /// <summary>
        ///     Gets a value indicating whether the generated Json representation should be in an array
        ///     format, even if the number of items is only one.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the generated Json representation should be in an array
        ///     format; otherwise, <c>false</c>.
        /// </value>
        bool EnforcingArrayConverting { get; }

    }

    /// <summary>
    ///     Represents an internal implementation of <see cref="ILinkBuilder" /> interface.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.ILinkBuilder" />
    internal sealed class LinkBuilder : Builder, ILinkBuilder {

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkBuilder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rel">The relation of the resource location.</param>
        /// <param name="enforcingArrayConverting">
        ///     The value indicating whether the generated Json representation should be in an array
        ///     format, even if the number of items is only one.
        /// </param>
        public LinkBuilder(IBuilder context, string rel, bool enforcingArrayConverting)
            : base(context) {
            Rel                      = rel;
            EnforcingArrayConverting = enforcingArrayConverting;
        }

        #endregion

        /// <summary>
        ///     Gets the relation of the resource location.
        /// </summary>
        /// <value>
        ///     The relation of the resource location.
        /// </value>
        public string Rel { get; }

        /// <summary>
        ///     Gets a value indicating whether the generated Json representation should be in an array
        ///     format, even if the number of items is only one.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the generated Json representation should be in an array
        ///     format; otherwise, <c>false</c>.
        /// </value>
        public bool EnforcingArrayConverting { get; }

        /// <summary>
        ///     Builds the <see cref="Resource" /> instance.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>
        ///     The <see cref="Resource" /> instance to be built.
        /// </returns>
        protected override Resource DoBuild(Resource resource) {
            if (resource.Links == null) {
                resource.Links = new LinkCollection();
            }

            ILink? link = resource.Links.FirstOrDefault(x => x.Rel.Equals(Rel));
            if (link == null) {
                resource.Links.Add(new Link(Rel));
            }

            return resource;
        }

    }

}