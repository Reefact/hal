#region Usings declarations

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents an internal implementation of <see cref="LinkItemBuilder" />.
    /// </summary>
    /// <seealso cref="Hal.Builders.Builder" />
    /// <seealso cref="Hal.Builders.LinkItemBuilder" />
    public sealed class LinkItemBuilder : Builder {

        #region Fields declarations

        private readonly string                       href;
        private readonly string?                      name;
        private readonly bool?                        templated;
        private readonly string?                      type;
        private readonly string?                      deprecation;
        private readonly string?                      profile;
        private readonly string?                      title;
        private readonly string?                      hreflang;
        private readonly IDictionary<string, object>? additionalProperties;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkItemBuilder" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rel">The relation of the resource location.</param>
        /// <param name="href">The href attribute of a link item.</param>
        /// <param name="name">The name attribute of a link item.</param>
        /// <param name="templated">
        ///     The <see cref="bool" /> value which indicates if the <c>Href</c> property
        ///     is a URI template.
        /// </param>
        /// <param name="type">The media type expected when dereferencing the target source.</param>
        /// <param name="deprecation">The URL which provides further information about the deprecation.</param>
        /// <param name="profile">The URI that hints about the profile of the target resource.</param>
        /// <param name="title">
        ///     The <see cref="string" /> value which is intended for labelling
        ///     the link with a human-readable identifier.
        /// </param>
        /// <param name="hreflang">
        ///     The <see cref="string" /> value which is intending for indicating
        ///     the language of the target resource.
        /// </param>
        /// <param name="enforcingArrayConverting">
        ///     The value indicating whether the generated Json representation should be in an array
        ///     format, even if the number of items is only one.
        /// </param>
        /// <param name="additionalProperties">The additional properties.</param>
        public LinkItemBuilder(IBuilder                     context,                     string  rel,                             string  href,
                               string?                      name                 = null, bool?   templated                = null, string? type  = null,
                               string?                      deprecation          = null, string? profile                  = null, string? title = null,
                               string?                      hreflang             = null, bool    enforcingArrayConverting = false,
                               IDictionary<string, object>? additionalProperties = null) : base(context) {
            Rel                       = rel;
            this.href                 = href;
            this.name                 = name;
            this.templated            = templated;
            this.type                 = type;
            this.deprecation          = deprecation;
            this.profile              = profile;
            this.title                = title;
            this.hreflang             = hreflang;
            EnforcingArrayConverting  = enforcingArrayConverting;
            this.additionalProperties = additionalProperties;
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
            Link? link = resource.Links?.FirstOrDefault(x => x.Rel.Equals(Rel));
            if (link == null) {
                link = new Link(Rel);
                resource.Links?.Add(link);
            }

            if (link.Items == null) {
                link.Items = new LinkItemCollection(EnforcingArrayConverting);
            }

            LinkItem linkItem = new(href) {
                Deprecation = deprecation,
                Hreflang    = hreflang,
                Name        = name,
                Profile     = profile,
                Templated   = templated,
                Title       = title,
                Type        = type
            };

            if (additionalProperties != null && additionalProperties.Count > 0) {
                foreach (KeyValuePair<string, object> property in additionalProperties) {
                    linkItem.AddProperty(property.Key, property.Value);
                }
            }

            link.Items.Add(linkItem);

            return resource;
        }

    }

}