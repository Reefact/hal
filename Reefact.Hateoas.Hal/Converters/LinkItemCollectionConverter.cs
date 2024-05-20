#region Usings declarations

using System;
using System.Linq;

using Newtonsoft.Json;

#endregion

namespace Reefact.Hateoas.Hal.Converters {

    /// <summary>
    ///     Represents the JSON converter for link item collection.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public sealed class LinkItemCollectionConverter : JsonConverter {

        /// <summary>
        ///     Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanRead => false;

        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(LinkItemCollection);
        }

        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        ///     The object value.
        /// </returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            return null;
        }

        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            LinkItemCollection collection = (LinkItemCollection)value!;

            if (collection.Count == 1) {
                if (collection.EnforcingArrayConverting) {
                    writer.WriteStartArray();
                }

                serializer.Serialize(writer, collection.First());

                if (collection.EnforcingArrayConverting) {
                    writer.WriteEndArray();
                }
            } else {
                writer.WriteStartArray();
                foreach (LinkItem? item in collection) {
                    serializer.Serialize(writer, item);
                }
                writer.WriteEndArray();
            }
        }

    }

}