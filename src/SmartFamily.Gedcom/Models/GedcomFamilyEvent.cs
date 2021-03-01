using SmartFamily.Gedcom.Enums;

using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// An event relating to a given family.
    /// </summary>
    public class GedcomFamilyEvent : GedcomEvent, IEquatable<GedcomFamilyEvent>
    {
        private GedcomAge _husbandAge;
        private GedcomAge _wifeAge;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomFamilyEvent"/> class.
        /// </summary>
        public GedcomFamilyEvent()
        {
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.FamilyEvent;
        }

        /// <summary>
        /// Gets or sets the husband age.
        /// </summary>
        /// <value>
        /// The husband age.
        /// </value>
        public GedcomAge HusbandAge
        {
            get => _husbandAge;
            set
            {
                if (value != _husbandAge)
                {
                    _husbandAge = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the wife age.
        /// </summary>
        /// <value>
        /// The wife age.
        /// </value>
        public GedcomAge WifeAge
        {
            get => _wifeAge;
            set
            {
                if (value != _wifeAge)
                {
                    _wifeAge = value;
                    Changed();
                }
            }
        }

        // util backpointer to the family record
        // this event belongs in
        /// <summary>
        /// Gets or sets the family record.
        /// </summary>
        /// <value>
        /// The family record.
        /// </value>
        /// <exception cref="Exception">Must set a GedcomFamilyRecord on a GedcomFamilyEvent.</exception>
        public GedcomFamilyRecord FamRecord
        {
            get => (GedcomFamilyRecord)Record;
            set
            {
                if (value != Record)
                {
                    Record = value;
                    if (Record != null)
                    {
                        if (Record.RecordType != GedcomRecordType.Family)
                        {
                            throw new Exception("Must set a GedcomFamilyRecord on a GedcomFamilyEvent");
                        }

                        Database = Record.Database;
                    }
                    else
                    {
                        Database = null;
                    }

                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public override GedcomChangeDate ChangeDate
        {
            get
            {
                GedcomChangeDate realChangeDate = base.ChangeDate;
                GedcomChangeDate childChangeDate;
                if (_husbandAge != null)
                {
                    childChangeDate = _husbandAge.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                if (_wifeAge != null)
                {
                    childChangeDate = _wifeAge.ChangeDate;
                    if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                    {
                        realChangeDate = childChangeDate;
                    }
                }

                if (realChangeDate != null)
                {
                    realChangeDate.Level = Level + 2;
                }

                return realChangeDate;
            }
            set => base.ChangeDate = value;
        }

        /// <summary>
        /// Output GEDCOM format for this family event.
        /// </summary>
        /// <param name="sw">Where to output the data to.</param>
        public override void Output(TextWriter sw)
        {
            base.Output(sw);

            string levelPlusOne = null;

            if (HusbandAge != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" HUSB ");

                HusbandAge.Output(sw, Level + 2);
            }

            if (WifeAge != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" WIFE ");

                WifeAge.Output(sw, Level + 2);
            }
        }

        /// <inheritdoc/>
        public bool Equals(GedcomFamilyEvent other)
        {
            return IsEquivalentTo(other);
        }
    }
}