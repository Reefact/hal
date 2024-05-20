#region Usings declarations

using System.Collections.Generic;
using System.IO;

using ApprovalTests;
using ApprovalTests.Reporters;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Reefact.Hateoas.Hal.Converters;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests.Converters {

    [UseReporter(typeof(BeyondCompareReporter))]
    public class ResourceConverterTests {

        [Fact]
        public void Resource_state_serialization_should_use_current_serializer() {
            // Setup
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
            Resource resource = new(new { Id = 1234 });
            // Exercise
            using StringWriter writer = new();
            serializer.Serialize(writer, resource);
            string hal = writer.ToString();
            // Verify
            Approvals.Verify(hal);
        }

        [Fact]
        public void Resource_state_serialization_with_StringEnumConverter_should_convert_enum_as_string() {
            // Setup
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
            Resource resource = new(new { Status = Status.Active });

            // Exercise
            using StringWriter writer = new();
            serializer.Serialize(writer, resource);
            string hal = writer.ToString();
            // Verify
            Approvals.Verify(hal);
        }

        #region Nested types declarations

        private enum Status {

            Active   = 0,
            Inactive = 1

        }

        #endregion

    }

}