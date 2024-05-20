#region Usings declarations

using System.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents an internal implementation of <see cref="LinkBuilder" /> interface.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.LinkBuilder" />
    public sealed class LinkBuilder : Builder {

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

            Link? link = resource.Links.FirstOrDefault(x => x.Rel.Equals(Rel));
            if (link == null) {
                resource.Links.Add(new Link(Rel));
            }

            return resource;
        }

    }

}