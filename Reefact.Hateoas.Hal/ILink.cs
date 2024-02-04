namespace Reefact.Hateoas.Hal {

    /// <summary>
    ///     Represents that the implemented classes are the links.
    /// </summary>
    public interface ILink {

        /// <summary>
        ///     Gets or sets the relation.
        /// </summary>
        /// <value>
        ///     The relation.
        /// </value>
        string Rel { get; set; }

        /// <summary>
        ///     Gets or sets the link items that belongs to the current link.
        /// </summary>
        /// <value>
        ///     The link items.
        /// </value>
        LinkItemCollection? Items { get; set; }

    }

}