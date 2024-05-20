#region Usings declarations

using ApprovalTests;
using ApprovalTests.Reporters;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    [UseReporter(typeof(BeyondCompareReporter))]
    public class ResourceTests {

        [Fact]
        public void Example() {
            LinkCollection links = new() {
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

            EmbeddedResource embedded = new() {
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

            Resource resource = new(new { currentlyProcessing = 14, shippedToday = 20 }) {
                Links             = links,
                EmbeddedResources = new EmbeddedResourceCollection { embedded }
            };

            string hal = resource.ToString();

            Approvals.Verify(hal);
        }

        [Fact]
        public void EmbededListTest() {
            Resource                   resource            = new(new { total = 100f, description = "test" });
            EmbeddedResourceCollection embeddedResources   = new();
            ResourceCollection         resourcesCollection = new();
            resourcesCollection.Add(new Resource(new { description = "item 1" }));
            resourcesCollection.Add(new Resource(new { description = "item 2" }));
            resourcesCollection.Add(new Resource(new { description = "item 3" }));
            resourcesCollection.Add(new Resource(new { description = "item 4" }));

            Resource anotherResource = new(new { code = "C001", value = 10 });

            EmbeddedResource embeddedResourceList = new() {
                Name                     = "List",
                Resources                = resourcesCollection,
                EnforcingArrayConverting = true
            };
            EmbeddedResource anotherEmbeddedResource = new() {
                Name      = "AnotherResource",
                Resources = new ResourceCollection { anotherResource }
            };

            embeddedResources.Add(embeddedResourceList);
            embeddedResources.Add(anotherEmbeddedResource);

            resource.EmbeddedResources = embeddedResources;

            string hal = resource.ToString();

            Approvals.Verify(hal);
        }

        [Fact]
        public void EmbededListTestWithOnlyOneElement() {
            Resource                   resource            = new(new { total = 100f, description = "test" });
            EmbeddedResourceCollection embeddedResources   = new();
            ResourceCollection         resourcesCollection = new();
            resourcesCollection.Add(new Resource(new { description = "item 1" }));

            Resource anotherResource = new(new { code = "C001", value = 10 });

            EmbeddedResource embeddedResourceList = new() {
                Name                     = "List",
                Resources                = resourcesCollection,
                EnforcingArrayConverting = true
            };
            EmbeddedResource anotherEmbeddedResource = new() {
                Name      = "AnotherResource",
                Resources = new ResourceCollection { anotherResource }
            };

            embeddedResources.Add(embeddedResourceList);
            embeddedResources.Add(anotherEmbeddedResource);

            resource.EmbeddedResources = embeddedResources;

            string hal = resource.ToString();

            Approvals.Verify(hal);
        }

    }

}