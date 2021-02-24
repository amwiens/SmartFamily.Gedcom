using SmartFamily.Gedcom.Enums;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Teh result of parsing and extracting a date period from a string.
    /// </summary>
    public class GedcomDatePeriodParseResult
    {
        /// <summary>
        /// Gets or sets the string that shows the parsed data with the date period extracted.
        /// </summary>
        public string DataAfterExtration { get; set; }

        /// <summary>
        /// Gets or sets the date period that has been parsed from the raw text.
        /// </summary>
        public GedcomDatePeriod DatePeriod { get; set; }
    }
}