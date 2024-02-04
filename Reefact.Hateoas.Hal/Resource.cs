#region Usings declarations

using System.Collections.Generic;

using Newtonsoft.Json;

using Reefact.Hateoas.Hal.Converters;

#endregion

namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents a resource in HAL.
    /// </summary>
    /// <seealso cref="Hal.IResource" />
    public sealed class Resource : IResource {

        #region Statics members declarations

        private static readonly List<JsonConverter> converters = new() {
            new LinkItemConverter(), new LinkItemCollectionConverter(), new LinkConverter(),
            new LinkCollectionConverter(), new ResourceConverter()
        };

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resource" /> class.
        /// </summary>
        public Resource() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resource" /> class.
        /// </summary>
        /// <param name="state">The state of the resource.</param>
        public Resource(object? state) {
            State = state;
        }

        #endregion

        /// <summary>
        ///     Gets the embedded resources.
        /// </summary>
        /// <value>
        ///     The embedded resources.
        /// </value>
        public EmbeddedResourceCollection? EmbeddedResources { get; set; }

        /// <summary>
        ///     Gets or sets the links.
        /// </summary>
        /// <value>
        ///     The links.
        /// </value>
        public LinkCollection? Links { get; set; }

        /// <summary>
        ///     Gets or sets the state of the resource, usually it is the object
        ///     that holds the domain information.
        /// </summary>
        /// <value>
        ///     The state of the resource.
        /// </value>
        public object? State { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return ToString(new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting        = Formatting.Indented
            });
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="jsonSerializerSettings">The serialization settings.</param>
        /// <returns>The string representation of the current instance.</returns>
        public string ToString(JsonSerializerSettings jsonSerializerSettings) {
            if (jsonSerializerSettings.Converters.Count == 0) {
                jsonSerializerSettings.Converters = converters;
            }

            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

    }

}