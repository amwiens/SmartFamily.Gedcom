using SmartFamily.Gedcom.Enums;
using SmartFamily.Gedcom.Helpers;

using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Details the spouse sealing event which can occur between a husband and wife.
    /// Sealing is a ritual performed by Latter Day Saint temples to seal familial relationships and
    /// the promise of family relationships throughout eternity.
    /// </summary>
    public class GedcomSpouseSealingRecord : GedcomRecord, IComparable, IComparable<GedcomSpouseSealingRecord>, IEquatable<GedcomSpouseSealingRecord>
    {
        /// <summary>
        /// The date that this sealing occurred on.
        /// </summary>
        private GedcomDate _date;

        /// <summary>
        /// The description for this sealing event.
        /// </summary>
        private string _description;

        /// <summary>
        /// The place at which this sealing occurred.
        /// </summary>
        private GedcomPlace _place;

        /// <summary>
        /// The status of this sealing.
        /// </summary>
        private SpouseSealingDateStatus _status;

        /// <summary>
        /// The date that the status was last changed.
        /// </summary>
        private GedcomChangeDate _statusChangeDate;

        /// <summary>
        /// The temple code.
        /// </summary>
        private string _templeCode;

        /// <summary>
        /// Gets or sets the date that this sealing occurred on.
        /// </summary>
        public GedcomDate Date
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the description for this sealing event.
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
        /// Gets or sets the place that this sealing occurred at.
        /// </summary>
        public GedcomPlace Place
        {
            get => _place;
            set
            {
                if (value != _place)
                {
                    _place = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the status of this sealing at a point in time.
        /// </summary>
        public SpouseSealingDateStatus Status
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
        /// Gets or sets the date that the status was last changed.
        /// </summary>
        public GedcomChangeDate StatusChangeDate
        {
            get => _statusChangeDate;
            set
            {
                if (value != _statusChangeDate)
                {
                    _statusChangeDate = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the temple code
        /// </summary>
        public string TempleCode
        {
            get => _templeCode;
            set
            {
                if (value != _templeCode)
                {
                    _templeCode = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get  => GedcomRecordType.SpouseSealing;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a spouse sealing record.
        /// </summary>
        public override string GedcomTag
        {
            get => "SLGS";
        }

        /// <summary>
        /// Compare two GEDCOM spouse sealing records.
        /// </summary>
        /// <param name="recorda">First record to compare.</param>
        /// <param name="recordb">Second record to compare.</param>
        /// <returns>0 if equal, -1 if recorda less than recordb, else 1.</returns>
        public static int Compare(GedcomSpouseSealingRecord recorda, GedcomSpouseSealingRecord recordb)
        {
            bool anull = Equals(recorda, null);
            bool bnull = Equals(recordb, null);

            if (anull && bnull)
            {
                return 0;
            }
            else if (anull)
            {
                return -1;
            }
            else if (bnull)
            {
                return 1;
            }

            int ret = recorda.Date.CompareTo(recordb._date);
            if (ret == 0)
            {
                ret = recorda.TempleCode.CompareTo(recordb.TempleCode);
                if (ret == 0)
                {
                    ret = recorda.Description.CompareTo(recordb.Description);
                    if (ret == 0)
                    {
                        ret = recorda.Place.Name.CompareTo(recordb.Place.Name);
                        if (ret == 0)
                        {
                            ret = recorda.Status.CompareTo(recordb.Status);
                            if (ret == 0)
                            {
                                ret = recorda.StatusChangeDate.CompareTo(recordb.StatusChangeDate);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomSpouseSealingRecord) == 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this == (GedcomSpouseSealingRecord)obj;
        }

        /// <summary>
        /// Compares the current and passed-in object to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomSpouseSealingRecord);
        }

        /// <summary>
        /// Compares the current and passed-in sealing record to see if they are the same.
        /// </summary>
        /// <param name="otherRecord">The sealing record to compare the current instance against.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(GedcomSpouseSealingRecord otherRecord)
        {
            return Compare(this, otherRecord);
        }

        /// <summary>
        /// Compares the current and passed-in sealing record to see if they are the same.
        /// </summary>
        /// <param name="otherRecord">The sealing record to compare the current instance against.</param>
        /// <returns>True if they match, False otherwise.</returns>
        public bool Equals(GedcomSpouseSealingRecord otherRecord)
        {
            return this == otherRecord;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap.
            unchecked
            {
                int hash = 17;

                hash *= 23 + _date.GetHashCode();
                hash *= 23 + _templeCode.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Outputs this instance as a GEDCOM record.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.WriteLine();
            tw.Write(Level.ToString());
            tw.Write(" SLGS ");

            if (!string.IsNullOrEmpty(Description))
            {
                tw.Write(Description);
            }

            if (_date != null)
            {
                _date.Output(tw);
            }

            if (_place != null)
            {
                _place.Output(tw);
            }

            var levelPlusOne = (Level + 1).ToString();
            if (!string.IsNullOrWhiteSpace(_templeCode))
            {
                tw.WriteLine();
                tw.Write(levelPlusOne);
                tw.Write(" TEMP ");
                tw.Write(_templeCode);
            }

            if (_status != SpouseSealingDateStatus.NotSet)
            {
                tw.WriteLine();
                tw.Write(levelPlusOne);
                tw.Write(" STAT ");
                tw.Write(EnumHelper.ToDescription(_status));

                if (StatusChangeDate != null)
                {
                    var levelPlusTwo = (Level + 2).ToString();

                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" CHAN ");
                    StatusChangeDate.Output(tw);
                }
            }

            OutputStandard(tw);
        }
    }
}