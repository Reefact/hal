#region Usings declarations

using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Reefact.Hateoas.Hal.Converters;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests.Converters {

    public class ResourceConverterTests {

        [Fact]
        public void Resource_state_serialization_should_use_current_serializer() {
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> {
                    new ResourceConverter(),
                    new LinkCollectionConverter(),
                    new LinkConverter(),
                    new LinkItemCollectionConverter(),
                    new LinkItemConverter()
                }
            });
            Resource resource = new Resource(new { Id = 1234 });

            JToken result = JToken.FromObject(resource, serializer);

            JObject expected = new(new JProperty("id", 1234));
            Assert.True(JToken.DeepEquals(expected, result), $"Expected {result} to be equal to {expected}.");
        }

        [Fact]
        public void Resource_state_serialization_with_StringEnumConverter_should_convert_enum_as_string() {
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> {
                    new ResourceConverter(),
                    new LinkCollectionConverter(),
                    new LinkConverter(),
                    new LinkItemCollectionConverter(),
                    new LinkItemConverter(),
                    new StringEnumConverter()
                }
            });
            Resource resource = new Resource(new { Status = Status.Active });

            JToken result = JToken.FromObject(resource, serializer);

            JObject expected = new(new JProperty("status", "Active"));
            Assert.True(JToken.DeepEquals(expected, result), $"Expected {result} to be equal to {expected}.");
        }

        #region Nested types declarations

        private enum Status {

            Active   = 0,
            Inactive = 1

        }

        #endregion

    }

}