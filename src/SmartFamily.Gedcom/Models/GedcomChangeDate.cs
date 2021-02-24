namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// The date on which a GEDCOM record was changed.
    /// </summary>
    public class GedcomChangeDate : GedcomDate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomChangeDate"/> class.
        /// </summary>
        /// <param name="database">The GEDCOM database to associate this date with.</param>
        public GedcomChangeDate(GedcomDatabase database)
            : base(database)
        {
        }

        /// <inheritdoc/>
        protected override void Changed()
        {
        }
    }
}