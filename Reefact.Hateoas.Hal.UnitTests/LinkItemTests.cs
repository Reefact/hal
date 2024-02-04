#region Usings declarations

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    public class LinkItemTests {

        [Fact]
        public void LinkItemToStringTest() {
            LinkItem linkItem = new LinkItem("/orders");
            linkItem.Name      = "ea";
            linkItem.Templated = true;
            linkItem.AddProperty("age", 10);
            string str = linkItem.ToString();
        }

    }

}