#region Usings declarations

using ApprovalTests;
using ApprovalTests.Reporters;

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    [UseReporter(typeof(BeyondCompareReporter))]
    public class LinkItemTests {

        [Fact]
        public void LinkItemToStringTest() {
            // Setup
            LinkItem linkItem = new("/orders");
            linkItem.Name      = "ea";
            linkItem.Templated = true;
            linkItem.AddProperty("age", 10);
            // Exercise
            string hal = linkItem.ToString();
            // Verify
            Approvals.Verify(hal);
        }

    }

}