#region Usings declarations

using System.Collections.Generic;

using Newtonsoft.Json;

using Reefact.Hateoas.Hal.Converters;

#endregion

namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents the link in the HAL.
    /// </summary>
    /// <seealso cref="Hal.ILink" />
    public sealed class Link : ILink {

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="rel">The relation of the resource location.</param>
        public Link(string rel) {
            Rel = rel;
        }

        #endregion

        /// <summary>
        ///     Gets or sets the relation.
        /// </summary>
        /// <value>
        ///     The relation.
        /// </value>
        public string Rel { get; set; }

        /// <summary>
        ///     Gets or sets the link items that belongs to the current link.
        /// </summary>
        /// <value>
        ///     The link items.
        /// </value>
        public LinkItemCollection? Items { get; set; }

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
            jsonSerializerSettings.Converters = new List<JsonConverter> {
                new LinkItemConverter(),
                new LinkItemCollectionConverter(),
                new LinkConverter()
            };

            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

    }

}