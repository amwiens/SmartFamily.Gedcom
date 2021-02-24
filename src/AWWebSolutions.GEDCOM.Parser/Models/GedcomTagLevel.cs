namespace AWWebSolutions.GEDCOM.Parser.Models
{
    /// <summary>
    /// Used by a stack to store a tag and level for tracking the parsing process.
    /// </summary>
    public class GedcomTagLevel
    {
        /// <summary>
        /// Gets or sets the current tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the current tag level.
        /// </summary>
        public int Level { get; set; }
    }
}