#region Usings declarations

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    public class LinkTests {

        [Fact]
        public void SelfLinkToStringTest() {
            Link selfLink = new Link("self") {
                Items = new LinkItemCollection {
                    new LinkItem("/orders")
                }
            };

            string json = selfLink.ToString();
        }

        [Fact]
        public void CuriesLinkToStringTest() {
            Link curiesLink = new Link("curies") {
                Items = new LinkItemCollection(true) {
                    new LinkItem("http://example.com/docs/rels/{rel}") { Name = "ea", Templated = true }
                }
            };

            string json = curiesLink.ToString();
        }

    }

}