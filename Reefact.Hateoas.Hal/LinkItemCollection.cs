#region Usings declarations

using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using Reefact.Hateoas.Hal.Converters;

#endregion

namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents a collection of <see cref="ILinkItem" /> objects.
    /// </summary>
    public sealed class LinkItemCollection : ICollection<ILinkItem> {

        #region Fields declarations

        private readonly List<ILinkItem> items = new();

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkItemCollection" /> class.
        /// </summary>
        /// <param name="enforcingArrayConverting">if set to <c>true</c> [enforcing arrary converting].</param>
        public LinkItemCollection(bool enforcingArrayConverting = false) {
            EnforcingArrayConverting = enforcingArrayConverting;
        }

        #endregion

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Gets a value indicating whether the generated Json representation should be in an array
        ///     format, even if the number of items is only one.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the generated Json representation should be in an array format; otherwise, <c>false</c>.
        /// </value>
        public bool EnforcingArrayConverting { get; }

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(ILinkItem item) {
            items.Add(item);
        }

        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear() {
            items.Clear();
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///     otherwise, false.
        /// </returns>
        public bool Contains(ILinkItem item) {
            return items.Contains(item);
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        ///     <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///     from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
        ///     zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(ILinkItem[] array, int arrayIndex) {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ILinkItem> GetEnumerator() {
            return items.GetEnumerator();
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
        ///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(ILinkItem item) {
            return items.Remove(item);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return ToString(new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting        = Formatting.Indented
            });
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="jsonSerializerSettings">The serialization settings.</param>
        /// <returns>The string representation of the current instance.</returns>
        public string ToString(JsonSerializerSettings jsonSerializerSettings) {
            jsonSerializerSettings.Converters = new List<JsonConverter> { new LinkItemConverter(), new LinkItemCollectionConverter() };

            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return items.GetEnumerator();
        }

    }

}