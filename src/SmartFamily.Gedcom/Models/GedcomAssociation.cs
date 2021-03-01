using SmartFamily.Gedcom.Enums;

using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// How the given individual is associated to another.
    /// Each GedcomIndividual contains a list of these.
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomAssociation : GedcomRecord, IComparable, IComparable<GedcomAssociation>, IEquatable<GedcomAssociation>
    {
        private string _description;

        private string _individual;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomAssociation"/> class.
        /// </summary>
        public GedcomAssociation()
        {
        }

        /// <summary>
        /// Gets the record type for an association.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Association;
        }

        /// <summary>
        /// Gets the GEDCOM tag for an association.
        /// </summary>
        public override string GedcomTag
        {
            get => "ASSO";
        }

        /// <summary>
        /// Gets or sets the description for this association.
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the individual for this association.
        /// </summary>
        public string Individual
        {
            get => _individual;
            set
            {
                if (value != _individual)
                {
                    _individual = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the association.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write(Environment.NewLine);
            tw.Write(Level.ToString());
            tw.Write(" ASSO ");
            tw.Write("@");
            tw.Write(Individual);
            tw.Write("@");

            string levelPlusOne = (Level + 1).ToString();

            tw.Write(Environment.NewLine);
            tw.Write(levelPlusOne);
            tw.Write(" RELA ");

            string line = Description.Replace("@", "@@");
            if (line.Length > 25)
            {
                Util.SplitText(tw, line, Level + 1, 25, 1, true);
            }
            else
            {
                tw.Write(line);
            }

            OutputStandard(tw);
        }

        /// <summary>
        /// Compare the user-entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomAssociation) == 0;
        }

        /// <summary>
        /// Compares the current and passed-in object to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomAssociation);
        }

        /// <summary>
        /// Compares the current and passed-in association to see if they are the same.
        /// </summary>
        /// <param name="other">The association to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(GedcomAssociation other)
        {
            if (other == null)
            {
                return 1;
            }

            var compare = string.Compare(Description, other.Description);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Individual, other.Individual);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares the current and passed-in association to see if they are the same.
        /// </summary>
        /// <param name="other">The association to compare the current instance against.</param>
        /// <returns><c>True</c> if they match, <c>False</c> otherwise.</returns>
        public bool Equals(GedcomAssociation other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compares the current and passed-in object to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns><c>True</c> if they match, <c>False</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GedcomAssociation);
        }

        public override int GetHashCode()
        {
            return new
            {
                Description,
                Individual,
            }.GetHashCode();
        }
    }
}