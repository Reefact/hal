#region Usings declarations

using Xunit;

#endregion

namespace Reefact.Hateoas.Hal.UnitTests {

    public class LinkItemCollectionTests {

        [Fact]
        public void LinkItemCollectionToStringTest() {
            LinkItem linkItem = new("/orders");
            linkItem.Name      = "ea";
            linkItem.Templated = true;
            linkItem.AddProperty("age", 10);

            LinkItemCollection collection = new() {
                linkItem
            };

            string json = collection.ToString();
        }

        [Fact]
        public void LinkItemCollectionWithMultipleItemsToStringTest() {
            LinkItem linkItem1 = new("/orders");
            linkItem1.Name      = "ea";
            linkItem1.Templated = true;
            linkItem1.AddProperty("age", 10);

            LinkItem linkItem2 = new("/customers");

            LinkItemCollection collection = new() {
                linkItem1, linkItem2
            };

            string json = collection.ToString();
        }

    }

}