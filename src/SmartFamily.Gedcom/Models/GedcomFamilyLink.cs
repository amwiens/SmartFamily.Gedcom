using SmartFamily.Gedcom.Enums;

using System;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// How an individual is linked to a family.
    /// </summary>
    public class GedcomFamilyLink : GedcomRecord, IComparable<GedcomFamilyLink>, IComparable, IEquatable<GedcomFamilyLink>
    {
        private string _family;
        private string _indi;

        private PedigreeLinkageType _pedigree;
        private ChildLinkageStatus _status;

        private PedigreeLinkageType _fatherPedigree;
        private PedigreeLinkageType _motherPedigree;

        private bool _preferredSpouse;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomFamilyLink"/> class.
        /// </summary>
        public GedcomFamilyLink()
        {
            _pedigree = PedigreeLinkageType.Unknown;
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.FamilyLink;
        }

        /// <summary>
        /// Gets or sets the family.
        /// </summary>
        /// <value>
        /// The family.
        /// </value>
        public string Family
        {
            get => _family;
            set
            {
                if (value != _family)
                {
                    _family = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the individual being linked in this family record.
        /// </summary>
        public string Individual
        {
            get => _indi;
            set
            {
                if (value != _indi)
                {
                    _indi = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the pedigree.
        /// </summary>
        /// <value>
        /// The pedigree.
        /// </value>
        public PedigreeLinkageType Pedigree
        {
            get => _pedigree;
            set
            {
                if (value != _pedigree)
                {
                    _pedigree = value;
                    FatherPedigree = value;
                    MotherPedigree = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the father pedigree.
        /// </summary>
        /// <value>
        /// The father pedigree.
        /// </value>
        public PedigreeLinkageType FatherPedigree
        {
            get => _fatherPedigree;
            set
            {
                if (value != _fatherPedigree)
                {
                    _fatherPedigree = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the mother pedigree.
        /// </summary>
        /// <value>
        /// The mother pedigree.
        /// </value>
        public PedigreeLinkageType MotherPedigree
        {
            get => _motherPedigree;
            set
            {
                if (value != _motherPedigree)
                {
                    _motherPedigree = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ChildLinkageStatus Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [preferred spouse].
        /// </summary>
        /// <value>
        /// <c>true</c> if [preferred spouse]; otherwise, <c>false</c>.
        /// </value>
        public bool PreferredSpouse
        {
            get => _preferredSpouse;
            set => _preferredSpouse = value;
        }

        /// <summary>
        /// Compares the current and passed family link to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>True if they match, False otherwise.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomFamilyLink) == 0;
        }

        /// <summary>
        /// Compares the current and passed family link to see if they are the same.
        /// </summary>
        /// <param name="link">The family link to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(GedcomFamilyLink link)
        {
            /* Family and Individual appear to store XRefId values,
             * which don't seem to contribute to the equality of a family link.
             */

            if (link == null)
            {
                return 1;
            }

            var compare = FatherPedigree.CompareTo(link.FatherPedigree);
            if (compare != 0)
            {
                return compare;
            }

            compare = MotherPedigree.CompareTo(link.MotherPedigree);
            if (compare != 0)
            {
                return compare;
            }

            compare = Pedigree.CompareTo(link.Pedigree);
            if (compare != 0)
            {
                return compare;
            }

            compare = PreferredSpouse.CompareTo(link.PreferredSpouse);
            if (compare != 0)
            {
                return compare;
            }

            compare = Status.CompareTo(link.Status);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares the current and passed family link to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomFamilyLink);
        }

        /// <summary>
        /// Compares the current and passed family link to see if they are the same.
        /// </summary>
        /// <param name="other">The GedcomFamilyLink to compare the current instance against.</param>
        /// <returns>True if they match, False otherwise.</returns>
        public bool Equals(GedcomFamilyLink other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compares the current and passed-in object to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>True if they match, False otherwise.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GedcomFamilyLink);
        }

        public override int GetHashCode()
        {
            return new
            {
                FatherPedigree,
                MotherPedigree,
                Pedigree,
                PreferredSpouse,
                Status,
            }.GetHashCode();
        }
    }
}