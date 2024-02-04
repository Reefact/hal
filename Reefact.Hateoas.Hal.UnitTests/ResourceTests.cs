#region Usings declarations

using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    public class ResourceTests {

        [Fact]
        public void Example() {
            LinkCollection links = new LinkCollection {
                new Link("self") { Items = new LinkItemCollection { new LinkItem("/orders") } },
                new Link("curies") {
                    Items = new LinkItemCollection(true) {
                        new LinkItem("http://example.com/docs/rels/{rel}") { Name = "ea", Templated = true }
                    }
                },
                new Link("next") { Items = new LinkItemCollection { new LinkItem("/orders?page=2") } },
                new Link("ea:find") {
                    Items = new LinkItemCollection { new LinkItem("/orders{?id}") { Templated = true } }
                }
            };

            EmbeddedResource embedded = new EmbeddedResource {
                Name = "ea:order",
                Resources = new ResourceCollection {
                    new Resource(new { total = 30.00F, currency = "USD", status = "shipped" }) {
                        Links = new LinkCollection {
                            new Link("self") {
                                Items = new LinkItemCollection { new LinkItem("/orders/123") }
                            },
                            new Link("ea:basket") {
                                Items = new LinkItemCollection { new LinkItem("/baskets/98712") }
                            },
                            new Link("ea:customer") {
                                Items = new LinkItemCollection { new LinkItem("/customers/7809") }
                            }
                        }
                    },
                    new Resource(new { total = 20.00F, currency = "USD", status = "processing" }) {
                        Links = new LinkCollection {
                            new Link("self") {
                                Items = new LinkItemCollection { new LinkItem("/orders/124") }
                            },
                            new Link("ea:basket") {
                                Items = new LinkItemCollection { new LinkItem("/baskets/97213") }
                            },
                            new Link("ea:customer") {
                                Items = new LinkItemCollection { new LinkItem("/customers/12369") }
                            }
                        }
                    }
                }
            };

            Resource resource = new Resource(new { currentlyProcessing = 14, shippedToday = 20 }) {
                Links             = links,
                EmbeddedResources = new EmbeddedResourceCollection { embedded }
            };

            string hal = resource.ToString();
        }

        [Fact]
        public void EmbededListTest() {
            Resource halResoruce = new Resource(new { total = 100f, description = "test" });

            EmbeddedResourceCollection embeddedResources   = new EmbeddedResourceCollection();
            ResourceCollection         resourcesCollection = new ResourceCollection();
            resourcesCollection.Add(new Resource(new { description = "item 1" }));
            resourcesCollection.Add(new Resource(new { description = "item 2" }));
            resourcesCollection.Add(new Resource(new { description = "item 3" }));
            resourcesCollection.Add(new Resource(new { description = "item 4" }));

            Resource anotherResource = new Resource(new { code = "C001", value = 10 });

            EmbeddedResource embeddedResourceList = new EmbeddedResource {
                Name                     = "List",
                Resources                = resourcesCollection,
                EnforcingArrayConverting = true
            };
            EmbeddedResource anotherEmbeddedResource = new EmbeddedResource {
                Name      = "AnotherResource",
                Resources = new ResourceCollection { anotherResource }
            };

            embeddedResources.Add(embeddedResourceList);
            embeddedResources.Add(anotherEmbeddedResource);

            halResoruce.EmbeddedResources = embeddedResources;

            IDictionary<string, JToken> result = JObject.Parse(halResoruce.ToString());

            Assert.True(result.ContainsKey("total"));
            Assert.True(result.ContainsKey("description"));
            Assert.Equal(4, result["_embedded"]["List"].Children().Count());
            Assert.Equal("item 1", result["_embedded"]["List"].Children().First()["description"].ToString());
            Assert.Equal("C001", result["_embedded"]["AnotherResource"]["code"].ToString());
            Assert.Equal("10", result["_embedded"]["AnotherResource"]["value"].ToString());
        }

        [Fact]
        public void EmbededListTestWithOnlyOneElement() {
            Resource halResoruce = new Resource(new { total = 100f, description = "test" });

            EmbeddedResourceCollection embeddedResources   = new EmbeddedResourceCollection();
            ResourceCollection         resourcesCollection = new ResourceCollection();
            resourcesCollection.Add(new Resource(new { description = "item 1" }));

            Resource anotherResource = new Resource(new { code = "C001", value = 10 });

            EmbeddedResource embeddedResourceList = new EmbeddedResource {
                Name                     = "List",
                Resources                = resourcesCollection,
                EnforcingArrayConverting = true
            };
            EmbeddedResource anotherEmbeddedResource = new EmbeddedResource {
                Name      = "AnotherResource",
                Resources = new ResourceCollection { anotherResource }
            };

            embeddedResources.Add(embeddedResourceList);
            embeddedResources.Add(anotherEmbeddedResource);

            halResoruce.EmbeddedResources = embeddedResources;

            IDictionary<string, JToken> result = JObject.Parse(halResoruce.ToString());

            Assert.True(result.ContainsKey("total"));
            Assert.True(result.ContainsKey("description"));
            Assert.Single(result["_embedded"]["List"].Children());
            Assert.Equal("item 1", result["_embedded"]["List"].Children().First()["description"].ToString());
            Assert.Equal("C001", result["_embedded"]["AnotherResource"]["code"].ToString());
            Assert.Equal("10", result["_embedded"]["AnotherResource"]["value"].ToString());
        }

    }

}