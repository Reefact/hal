﻿#region Usings declarations

using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Reefact.Hateoas.Hal.Converters {

    /// <summary>
    ///     Represents the JSON converter for resources.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public sealed class ResourceConverter : JsonConverter {

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
            return objectType == typeof(Resource);
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
            Resource resource = (Resource)value!;
            JToken?  obj      = null;

            if (resource.State != null) {
                obj = JToken.FromObject(resource.State, serializer);
                if (obj != null && obj.Type != JTokenType.Object) {
                    obj.WriteTo(writer);

                    return;
                }
            }

            writer.WriteStartObject();

            if (resource.Links != null && resource.Links.Count > 0) {
                serializer.Serialize(writer, resource.Links);
            }

            if (obj != null) {
                JObject @object = (JObject)obj;
                foreach (JProperty? prop in @object.Properties()) {
                    prop.WriteTo(writer);
                }
            }

            if (resource.EmbeddedResources != null && resource.EmbeddedResources.Count() > 0) {
                writer.WritePropertyName("_embedded");
                writer.WriteStartObject();
                foreach (EmbeddedResource? embeddedResource in resource.EmbeddedResources) {
                    if (!string.IsNullOrEmpty(embeddedResource.Name)) {
                        writer.WritePropertyName(embeddedResource.Name!);

                        embeddedResource.Resources ??= new ResourceCollection();
                        if (!resource.EmbeddedResources.EnforcingArrayConverting &&
                            !embeddedResource.EnforcingArrayConverting           &&
                            embeddedResource.Resources.Count == 1) {
                            //writer.WriteStartObject();
                            Resource? first = embeddedResource.Resources.First();
                            WriteJson(writer, first, serializer);
                            //writer.WriteEndObject();
                        } else {
                            writer.WriteStartArray();
                            foreach (Resource? current in embeddedResource.Resources) {
                                WriteJson(writer, current, serializer);
                            }
                            writer.WriteEndArray();
                        }
                    }
                }
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

    }

}