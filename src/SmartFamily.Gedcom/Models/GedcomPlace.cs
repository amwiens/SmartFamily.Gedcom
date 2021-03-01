using SmartFamily.Gedcom.Enums;

using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Represents a place or location.
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomPlace : GedcomRecord, IEquatable<GedcomPlace>, IComparable<GedcomPlace>, IComparable
    {
        private string _name;
        private string _form;

        private GedcomRecordList<GedcomVariation> _phoneticVariations;
        private GedcomRecordList<GedcomVariation> _romanizedVariations;

        private string _latitude;
        private string _longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomPlace"/> class.
        /// </summary>
        public GedcomPlace()
        {
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Place;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a place.
        /// </summary>
        public override string GedcomTag
        {
            get => "PLAC";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the form.
        /// </summary>
        public string Form
        {
            get => _form;
            set
            {
                if (value != _form)
                {
                    _form = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the phonetic variations.
        /// </summary>
        public GedcomRecordList<GedcomVariation> PhoneticVariations
        {
            get
            {
                if (_phoneticVariations == null)
                {
                    _phoneticVariations = new GedcomRecordList<GedcomVariation>();
                    _phoneticVariations.CollectionChanged += ListChanged;
                }

                return _phoneticVariations;
            }
        }

        /// <summary>
        /// Gets the romanized variations.
        /// </summary>
        public GedcomRecordList<GedcomVariation> RomanizedVariations
        {
            get
            {
                if (_romanizedVariations == null)
                {
                    _romanizedVariations = new GedcomRecordList<GedcomVariation>();
                    _romanizedVariations.CollectionChanged += ListChanged;
                }

                return _romanizedVariations;
            }
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public string Latitude
        {
            get => _latitude;
            set
            {
                if (value != _latitude)
                {
                    _latitude = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public string Longitude
        {
            get => _longitude;
            set
            {
                if (value != _longitude)
                {
                    _longitude = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        public override GedcomChangeDate ChangeDate
        {
            get
            {
                GedcomChangeDate realChangeDate = base.ChangeDate;
                GedcomChangeDate childChangeDate;
                if (_phoneticVariations != null)
                {
                    foreach (GedcomVariation variation in _phoneticVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
                    }
                }

                if (_romanizedVariations != null)
                {
                    foreach (GedcomVariation variation in _romanizedVariations)
                    {
                        childChangeDate = variation.ChangeDate;
                        if (childChangeDate != null && realChangeDate != null && childChangeDate > realChangeDate)
                        {
                            realChangeDate = childChangeDate;
                        }
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
        /// Output GEDCOM formatted text representing the place.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write(Environment.NewLine);
            tw.Write(Level.ToString());
            tw.Write(" PLAC ");

            if (!string.IsNullOrEmpty(Name))
            {
                string line = Name.Replace("@", "@@");
                tw.Write(line);
            }

            OutputStandard(tw);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            if (!String.IsNullOrEmpty(Form))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                string line = Form.Replace("@", "@@");
                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" FORM ");
                tw.Write(line);
            }

            if (_phoneticVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = (Level + 2).ToString();
                }

                foreach (GedcomVariation variation in PhoneticVariations)
                {
                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusOne);
                    tw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    tw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        tw.Write(Environment.NewLine);
                        tw.Write(levelPlusTwo);
                        tw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        tw.Write(line);
                    }
                }
            }

            if (_romanizedVariations != null)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                if (levelPlusTwo == null)
                {
                    levelPlusTwo = (Level + 2).ToString();
                }

                foreach (GedcomVariation variation in RomanizedVariations)
                {
                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusOne);
                    tw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    tw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        tw.Write(Environment.NewLine);
                        tw.Write(levelPlusTwo);
                        tw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        tw.Write(line);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" MAP ");
                if (!string.IsNullOrEmpty(Latitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" LATI ");
                    string line = Latitude.Replace("@", "@@");
                    tw.Write(line);
                }

                if (!string.IsNullOrEmpty(Longitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" LONG ");
                    string line = Longitude.Replace("@", "@@");
                    tw.Write(line);
                }
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomPlace) == 0;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomPlace to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomPlace other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Compares this place record to another record.
        /// </summary>
        /// <param name="place">A place record.</param>
        /// <returns>
        /// &lt;0 if this record precedes the other in the sort order;
        /// &gt;0 if the other record precedes this one;
        /// 0 if the records are equal.
        /// </returns>
        public int CompareTo(GedcomPlace place)
        {
            if (place == null)
            {
                return 1;
            }

            var compare = string.Compare(Form, place.Form);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Latitude, place.Latitude);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Longitude, place.Longitude);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Name, place.Name);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(PhoneticVariations, place.PhoneticVariations);
            if (compare != 0)
            {
                return compare;
            }

            compare = GedcomGenericListComparer.CompareListOrder(RomanizedVariations, place.RomanizedVariations);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The GedcomRepositoryRecord to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>False</c>.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomPlace);
        }
    }
}