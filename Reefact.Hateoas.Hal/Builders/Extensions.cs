#region Usings declarations

using System.Collections.Generic;

#endregion

namespace Reefact.Hateoas.Hal.Builders {

    /// <summary>
    ///     Represents the class that provides the extension methods to form
    ///     the Fluent API of the building of HAL resources.
    /// </summary>
    public static class Extensions {

        #region Statics members declarations

        /// <summary>
        ///     Assigns the object state to the resource that is going to be built.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="state">The object state.</param>
        /// <returns></returns>
        public static IResourceStateBuilder WithState(this IResourceBuilder builder, object state) {
            return new ResourceStateBuilder(builder, state);
        }

        /// <summary>
        ///     Adds a link to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rel">The relation of the resource location.</param>
        /// <returns></returns>
        public static ILinkBuilder AddLink(this IResourceStateBuilder builder, string rel) {
            return new LinkBuilder(builder, rel, false);
        }

        /// <summary>
        ///     Adds the "self" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddSelfLink(this IResourceStateBuilder builder) {
            return new LinkBuilder(builder, "self", false);
        }

        /// <summary>
        ///     Adds the "curies" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddCuriesLink(this IResourceStateBuilder builder) {
            return new LinkBuilder(builder, "curies", true);
        }

        /// <summary>
        ///     Adds the embedded resource collection to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name of the embedded resource collection.</param>
        /// <param name="enforcingArrayConverting">
        ///     The <see cref="bool" /> value which indicates whether the embedded resource state
        ///     should be always converted as an array even if there is only one state for that embedded resource.
        /// </param>
        /// <returns></returns>
        public static IEmbeddedResourceBuilder AddEmbedded(this IResourceStateBuilder builder, string name, bool enforcingArrayConverting = false) {
            return new EmbeddedResourceBuilder(builder, name, enforcingArrayConverting);
        }

        /// <summary>
        ///     Adds a link item to the currently building link.
        /// </summary>
        /// <param name="linkBuilder">The link builder.</param>
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
        /// <param name="additionalProperties">The additional properties.</param>
        /// <returns></returns>
        public static ILinkItemBuilder WithLinkItem(this ILinkBuilder            linkBuilder,                 string  href,
                                                    string?                      name                 = null, bool?   templated = null, string? type  = null,
                                                    string?                      deprecation          = null, string? profile   = null, string? title = null,
                                                    string?                      hreflang             = null,
                                                    IDictionary<string, object>? additionalProperties = null) {
            return new LinkItemBuilder(linkBuilder, linkBuilder.Rel, href, name, templated,
                                       type, deprecation, profile, title, hreflang, linkBuilder.EnforcingArrayConverting, additionalProperties);
        }

        /// <summary>
        ///     Adds a link to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rel">The relation of the resource location.</param>
        /// <returns></returns>
        public static ILinkBuilder AddLink(this ILinkItemBuilder builder, string rel) {
            return new LinkBuilder(builder, rel, false);
        }

        /// <summary>
        ///     Adds the "self" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddSelfLink(this ILinkItemBuilder builder) {
            return new LinkBuilder(builder, "self", false);
        }

        /// <summary>
        ///     Adds the "curies" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddCuriesLink(this ILinkItemBuilder builder) {
            return new LinkBuilder(builder, "curies", true);
        }

        /// <summary>
        ///     Adds a link item to the currently building link.
        /// </summary>
        /// <param name="builder">The link builder.</param>
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
        /// <param name="additionalProperties">The additional properties.</param>
        /// <returns></returns>
        public static ILinkItemBuilder WithLinkItem(this ILinkItemBuilder        builder,                     string  href,
                                                    string?                      name                 = null, bool?   templated = null, string? type  = null,
                                                    string?                      deprecation          = null, string? profile   = null, string? title = null,
                                                    string?                      hreflang             = null,
                                                    IDictionary<string, object>? additionalProperties = null) {
            return new LinkItemBuilder(builder, builder.Rel, href, name, templated,
                                       type, deprecation, profile, title, hreflang, builder.EnforcingArrayConverting, additionalProperties);
        }

        /// <summary>
        ///     Adds the embedded resource collection to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name of the embedded resource collection.</param>
        /// <param name="enforcingArrayConverting">
        ///     The <see cref="bool" /> value which indicates whether the embedded resource state
        ///     should be always converted as an array even if there is only one state for that embedded resource.
        /// </param>
        /// <returns></returns>
        public static IEmbeddedResourceBuilder AddEmbedded(this ILinkItemBuilder builder, string name, bool enforcingArrayConverting = false) {
            return new EmbeddedResourceBuilder(builder, name, enforcingArrayConverting);
        }

        /// <summary>
        ///     Adds the embedded resource to the embedded resource collection of the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="resourceBuilder">The resource builder that will build the embedded resource.</param>
        /// <returns></returns>
        public static IEmbeddedResourceItemBuilder Resource(this IEmbeddedResourceBuilder builder, IBuilder resourceBuilder) {
            return new EmbeddedResourceItemBuilder(builder, builder.Name, resourceBuilder);
        }

        /// <summary>
        ///     Adds the embedded resource to the embedded resource collection of the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="resourceBuilder">The resource builder that will build the embedded resource.</param>
        /// <returns></returns>
        public static IEmbeddedResourceItemBuilder Resource(this IEmbeddedResourceItemBuilder builder, IBuilder resourceBuilder) {
            return new EmbeddedResourceItemBuilder(builder, builder.Name, resourceBuilder);
        }

        /// <summary>
        ///     Adds a link to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rel">The relation of the resource location.</param>
        /// <returns></returns>
        public static ILinkBuilder AddLink(this IEmbeddedResourceItemBuilder builder, string rel) {
            return new LinkBuilder(builder, rel, false);
        }

        /// <summary>
        ///     Adds the "self" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddSelfLink(this IEmbeddedResourceItemBuilder builder) {
            return new LinkBuilder(builder, "self", false);
        }

        /// <summary>
        ///     Adds the "curies" link.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ILinkBuilder AddCuriesLink(this IEmbeddedResourceItemBuilder builder) {
            return new LinkBuilder(builder, "curies", true);
        }

        /// <summary>
        ///     Adds the embedded resource collection to the building resource.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name of the embedded resource collection.</param>
        /// <param name="enforcingArrayConverting">
        ///     The <see cref="bool" /> value which indicates whether the embedded resource state
        ///     should be always converted as an array even if there is only one state for that embedded resource.
        /// </param>
        /// <returns></returns>
        public static IEmbeddedResourceBuilder AddEmbedded(this IEmbeddedResourceItemBuilder builder, string name, bool enforcingArrayConverting = false) {
            return new EmbeddedResourceBuilder(builder, name, enforcingArrayConverting);
        }

        #endregion

    }

}