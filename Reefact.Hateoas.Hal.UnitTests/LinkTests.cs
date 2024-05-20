#region Usings declarations

using ApprovalTests;
using ApprovalTests.Reporters;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    [UseReporter(typeof(BeyondCompareReporter))]
    public class LinkTests {

        [Fact]
        public void SelfLinkToStringTest() {
            Link selfLink = new("self") {
                Items = new LinkItemCollection {
                    new LinkItem("/orders")
                }
            };

            string hal = selfLink.ToString();

            Approvals.Verify(hal);
        }

        [Fact]
        public void CuriesLinkToStringTest() {
            Link curiesLink = new("curies") {
                Items = new LinkItemCollection(true) {
                    new LinkItem("http://example.com/docs/rels/{rel}") { Name = "ea", Templated = true }
                }
            };

            string hal = curiesLink.ToString();

            Approvals.Verify(hal);
        }

    }

}