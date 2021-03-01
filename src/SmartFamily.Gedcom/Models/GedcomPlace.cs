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
        /// <value>
        /// The type of the record.
        /// </value>
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
        /// <value>
        /// The name.
        /// </value>
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
        /// <value>
        /// The form.
        /// </value>
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
        /// <value>
        /// The phonetic variations.
        /// </value>
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
        /// <value>
        /// The romanized variations.
        /// </value>
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
        /// <value>
        /// The latitude.
        /// </value>
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
        /// <value>
        /// The longitude.
        /// </value>
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
        /// <value>
        /// The change date.
        /// </value>
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
        /// Outputs this instance as a GEDCOM record.
        /// </summary>
        /// <param name="sw">The writer to output to.</param>
        public override void Output(TextWriter sw)
        {
            sw.Write(Environment.NewLine);
            sw.Write(Level.ToString());
            sw.Write(" PLAC ");

            if (!string.IsNullOrEmpty(Name))
            {
                string line = Name.Replace("@", "@@");
                sw.Write(line);
            }

            OutputStandard(sw);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            if (!String.IsNullOrEmpty(Form))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                string line = Form.Replace("@", "@@");
                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" FORM ");
                sw.Write(line);
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
                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusOne);
                    sw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(line);
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
                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusOne);
                    sw.Write(" FONE ");
                    string line = variation.Value.Replace("@", "@@");
                    sw.Write(line);
                    if (!string.IsNullOrEmpty(variation.VariationType))
                    {
                        sw.Write(Environment.NewLine);
                        sw.Write(levelPlusTwo);
                        sw.Write(" TYPE ");
                        line = variation.VariationType.Replace("@", "@@");
                        sw.Write(line);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Latitude) || !string.IsNullOrEmpty(Longitude))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                }

                sw.Write(Environment.NewLine);
                sw.Write(levelPlusOne);
                sw.Write(" MAP ");
                if (!string.IsNullOrEmpty(Latitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusTwo);
                    sw.Write(" LATI ");
                    string line = Latitude.Replace("@", "@@");
                    sw.Write(line);
                }

                if (!string.IsNullOrEmpty(Longitude))
                {
                    if (levelPlusTwo == null)
                    {
                        levelPlusTwo = (Level + 2).ToString();
                    }

                    sw.Write(Environment.NewLine);
                    sw.Write(levelPlusTwo);
                    sw.Write(" LONG ");
                    string line = Longitude.Replace("@", "@@");
                    sw.Write(line);
                }
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            return CompareTo(obj as GedcomPlace) == 0;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomPlace to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
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
        /// <returns>True if instance matches user data, otherwise False.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomPlace);
        }
    }
}