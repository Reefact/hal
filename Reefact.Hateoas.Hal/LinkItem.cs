#region Usings declarations

using System.Collections.Generic;

using Newtonsoft.Json;

using Reefact.Hateoas.Hal.Converters;

#endregion

namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents the link item of a link in the HAL.
    /// </summary>
    public sealed class LinkItem {

        #region Fields declarations

        private readonly Dictionary<string, object> properties = new();

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkItem" /> class.
        /// </summary>
        /// <param name="href">The href attribute of the link item.</param>
        public LinkItem(string href) {
            Href = href;
        }

        #endregion

        /// <summary>
        ///     Gets or sets the href attribute of a link item. This value is required.
        /// </summary>
        /// <remarks>
        ///     Its value is either a URI [RFC3986] or a URI Template [RFC6570].
        ///     If the value is a URI Template then the <see cref="Link" /> Object SHOULD have a
        ///     "templated" attribute whose value is true.
        /// </remarks>
        public string Href { get; set; }

        /// <summary>
        ///     Gets or sets the name attribute of a link item. This value is optional.
        /// </summary>
        /// <remarks>
        ///     Its value MAY be used as a secondary key for selecting Link Objects
        ///     which share the same relation type.
        /// </remarks>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets a <see cref="bool" /> value which indicates if the <c>Href</c> property
        ///     is a URI template. This value is optional.
        /// </summary>
        /// <remarks>
        ///     Its value SHOULD be considered false if it is undefined or any other
        ///     value than true.
        /// </remarks>
        public bool? Templated { get; set; }

        /// <summary>
        ///     Gets or sets the media type expected when dereferencing the target source. This value
        ///     is optional.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        ///     Gets or sets a URL which provides further information about the deprecation. This value is
        ///     optional.
        /// </summary>
        /// <remarks>
        ///     Its presence indicates that the link is to be deprecated (i.e.
        ///     removed) at a future date.Its value is a URL that SHOULD provide
        ///     further information about the deprecation.
        ///     A client SHOULD provide some notification (for example, by logging a
        ///     warning message) whenever it traverses over a link that has this
        ///     property.The notification SHOULD include the deprecation property's
        ///     value so that a client manitainer can easily find information about
        ///     the deprecation.
        /// </remarks>
        public string? Deprecation { get; set; }

        /// <summary>
        ///     Gets or sets the URI that hints about the profile of the target resource. This
        ///     value is optional.
        /// </summary>
        public string? Profile { get; set; }

        /// <summary>
        ///     Gets or sets a <see cref="string" /> value which is intended for labelling
        ///     the link with a human-readable identifier. This value is optional.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Gets or sets a <see cref="string" /> value which is intending for indicating
        ///     the language of the target resource.
        /// </summary>
        public string? Hreflang { get; set; }

        /// <summary>
        ///     Gets a list of additional properties assigned to the current link item.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Properties => properties;

        /// <summary>
        ///     Adds a property to the additional properties collection.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        public void AddProperty(string name, object value) {
            properties.Add(name, value);
        }

        /// <summary>
        ///     Removes all the properties from the properties collection.
        /// </summary>
        public void ClearProperties() {
            properties.Clear();
        }

        /// <summary>
        ///     Gets the property value by using the specified name.
        /// </summary>
        /// <param name="name">The name of the property which the property value should be returned.</param>
        /// <returns>The value of the property.</returns>
        public object GetProperty(string name) {
            return properties[name];
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return ToString(new JsonSerializerSettings {
                Formatting        = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="jsonSerializerSettings">The serialization settings.</param>
        /// <returns>The string representation of the current instance.</returns>
        public string ToString(JsonSerializerSettings jsonSerializerSettings) {
            jsonSerializerSettings.Converters = new[] { new LinkItemConverter() };

            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

    }

}