using System;
using System.IO;
using System.Xml;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// Stores details of an address
    /// </summary>
    public class GedcomAddress : IComparable<GedcomAddress>, IComparable, IEquatable<GedcomAddress>
    {
        private string _addressLine;
        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _city;
        private string _country;
        private GedcomDatabase _database;
        private string _email1;
        private string _email2;
        private string _email3;
        private string _fax1;
        private string _fax2;
        private string _fax3;
        private string _phone1;
        private string _phone2;
        private string _phone3;
        private string _postCode;
        private string _state;
        private string _www1;
        private string _www2;
        private string _www3;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomAddress"/> class.
        /// </summary>
        public GedcomAddress()
        {
        }

        /// <summary>
        /// Gets or sets a complete address as a single line.
        /// </summary>
        public string AddressLine
        {
            get => _addressLine;
            set
            {
                if (value != _addressLine)
                {
                    _addressLine = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the first line in an address.
        /// </summary>
        public string AddressLine1
        {
            get => _addressLine1;
            set
            {
                if (value != _addressLine1)
                {
                    _addressLine1 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the second line in an address.
        /// </summary>
        public string AddressLine2
        {
            get => _addressLine2;
            set
            {
                if (value != _addressLine2)
                {
                    _addressLine2 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the third line in an address.
        /// </summary>
        public string AddressLine3
        {
            get => _addressLine3;
            set
            {
                if (value != _addressLine3)
                {
                    _addressLine3 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the date the address was changed.
        /// </summary>
        public GedcomChangeDate ChangeDate { get; set; }

        /// <summary>
        /// Gets or sets the city for the address.
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the country the address is in.
        /// </summary>
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the database the address is in.
        /// </summary>
        public GedcomDatabase Database
        {
            get => _database;
            set => _database = value;
        }

        /// <summary>
        /// Gets or sets the main email address.
        /// </summary>
        public string Email1
        {
            get => _email1;
            set
            {
                if (value != _email1)
                {
                    _email1 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the secondary email address.
        /// </summary>
        public string Email2
        {
            get => _email2;
            set
            {
                if (value != _email2)
                {
                    _email2 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tertiary email address.
        /// </summary>
        public string Email3
        {
            get => _email3;
            set
            {
                if (value != _email3)
                {
                    _email3 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the main fax number.
        /// </summary>
        public string Fax1
        {
            get => _fax1;
            set
            {
                if (value != _fax1)
                {
                    _fax1 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the secondary fax number.
        /// </summary>
        public string Fax2
        {
            get => _fax2;
            set
            {
                if (value != _fax2)
                {
                    _fax2 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tertiary fax number.
        /// </summary>
        public string Fax3
        {
            get => _fax3;
            set
            {
                if (value != _fax3)
                {
                    _fax3 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the main phone number.
        /// </summary>
        public string Phone1
        {
            get => _phone1;
            set
            {
                if (value != _phone1)
                {
                    _phone1 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the secondary phone number.
        /// </summary>
        public string Phone2
        {
            get => _phone2;
            set
            {
                if (value != _phone2)
                {
                    _phone2 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tertiary phone number.
        /// </summary>
        public string Phone3
        {
            get => _phone3;
            set
            {
                if (value != _phone3)
                {
                    _phone3 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the PostCode / zip code for the address.
        /// </summary>
        public string PostCode
        {
            get => _postCode;
            set
            {
                if (value != _postCode)
                {
                    _postCode = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the state or county for the address
        /// </summary>
        public string State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the main website URI.
        /// </summary>
        public string Www1
        {
            get => _www1;
            set
            {
                if (value != _www1)
                {
                    _www1 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the secondary website URI.
        /// </summary>
        public string Www2
        {
            get => _www2;
            set
            {
                if (value != _www2)
                {
                    _www2 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the tertiary website URI.
        /// </summary>
        public string Www3
        {
            get => _www3;
            set
            {
                if (value != _www3)
                {
                    _www3 = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Compares the current and passed-in address to see if they are the same.
        /// </summary>
        /// <param name="otherAddress">The address to compare the current instance against.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(GedcomAddress otherAddress)
        {
            if (otherAddress == null)
            {
                return 1;
            }

            var compare = string.Compare(AddressLine, otherAddress.AddressLine);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(AddressLine1, otherAddress.AddressLine1);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(AddressLine2, otherAddress.AddressLine2);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(AddressLine3, otherAddress.AddressLine3);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(City, otherAddress.City);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Country, otherAddress.Country);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Email1, otherAddress.Email1);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Email2, otherAddress.Email2);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Email3, otherAddress.Email3);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Fax1, otherAddress.Fax1);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Fax2, otherAddress.Fax2);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Fax3, otherAddress.Fax3);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Phone1, otherAddress.Phone1);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Phone2, otherAddress.Phone2);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Phone3, otherAddress.Phone3);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(PostCode, otherAddress.PostCode);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(State, otherAddress.State);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Www1, otherAddress.Www1);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Www2, otherAddress.Www2);
            if (compare != 0)
            {
                return compare;
            }

            compare = string.Compare(Www3, otherAddress.Www3);
            if (compare != 0)
            {
                return compare;
            }

            return compare;
        }

        /// <summary>
        /// Compares the current and passed-in address to see if they are the same.
        /// </summary>
        /// <param name="obj">The object to compare the current instance against.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as GedcomAddress);
        }

        /// <summary>
        /// Compares the current and passed-in address to see if they are the same.
        /// </summary>
        /// <param name="otherAddress">The address to compare the current instance against.</param>
        /// <returns>
        /// True if they match, false otherwise.
        /// </returns>
        public bool Equals(GedcomAddress otherAddress)
        {
            return CompareTo(otherAddress) == 0;
        }

        /// <summary>
        /// Compares the current and passed-in address to see if they are the same.
        /// </summary>
        /// <param name="obj">The address to compare the current instance against.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap.
            unchecked
            {
                int hash = 17;

                hash *= 23 + (_addressLine == null ? 0 : _addressLine.GetHashCode());
                hash *= 23 + (_addressLine1 == null ? 0 : _addressLine1.GetHashCode());
                hash *= 23 + (_addressLine2 == null ? 0 : _addressLine2.GetHashCode());
                hash *= 23 + (_addressLine3 == null ? 0 : _addressLine3.GetHashCode());
                hash *= 23 + (_city == null ? 0 : _city.GetHashCode());
                hash *= 23 + (_country == null ? 0 : _country.GetHashCode());
                hash *= 23 + (_database == null ? 0 : _database.GetHashCode());
                hash *= 23 + (_email1 == null ? 0 : _email1.GetHashCode());
                hash *= 23 + (_email2 == null ? 0 : _email2.GetHashCode());
                hash *= 23 + (_email3 == null ? 0 : _email3.GetHashCode());
                hash *= 23 + (_fax1 == null ? 0 : _fax1.GetHashCode());
                hash *= 23 + (_fax2 == null ? 0 : _fax2.GetHashCode());
                hash *= 23 + (_fax3 == null ? 0 : _fax3.GetHashCode());
                hash *= 23 + (_phone1 == null ? 0 : _phone1.GetHashCode());
                hash *= 23 + (_phone2 == null ? 0 : _phone2.GetHashCode());
                hash *= 23 + (_phone3 == null ? 0 : _phone3.GetHashCode());
                hash *= 23 + (_postCode == null ? 0 : _postCode.GetHashCode());
                hash *= 23 + (_state == null ? 0 : _state.GetHashCode());
                hash *= 23 + (_www1 == null ? 0 : _www1.GetHashCode());
                hash *= 23 + (_www2 == null ? 0 : _www2.GetHashCode());
                hash *= 23 + (_www3 == null ? 0 : _www3.GetHashCode());

                return hash;
            }
        }

        /// <summary>
        /// Add the GEDCOM 6 XML elements for the data in this object as child
        /// nodes of the given root.
        /// </summary>
        /// <param name="root">
        /// A <see cref="XmlNode"/>
        /// </param>
        public void GenerateXML(XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;

            XmlNode node = doc.CreateElement("MailAddress");

            root.AppendChild(node);

            if (!string.IsNullOrEmpty(_phone1))
            {
                node = doc.CreateElement("Phone");
                node.AppendChild(doc.CreateTextNode(_phone1));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_phone2))
            {
                node = doc.CreateElement("Phone");
                node.AppendChild(doc.CreateTextNode(_phone2));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_phone3))
            {
                node = doc.CreateElement("Phone");
                node.AppendChild(doc.CreateTextNode(_phone3));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_email1))
            {
                node = doc.CreateElement("Email");
                node.AppendChild(doc.CreateTextNode(_email1));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_email2))
            {
                node = doc.CreateElement("Email");
                node.AppendChild(doc.CreateTextNode(_email2));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_email3))
            {
                node = doc.CreateElement("Email");
                node.AppendChild(doc.CreateTextNode(_email3));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_www1))
            {
                node = doc.CreateElement("URI");
                node.AppendChild(doc.CreateTextNode(_www1));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_www2))
            {
                node = doc.CreateElement("URI");
                node.AppendChild(doc.CreateTextNode(_www2));
                root.AppendChild(node);
            }

            if (!string.IsNullOrEmpty(_www3))
            {
                node = doc.CreateElement("URI");
                node.AppendChild(doc.CreateTextNode(_www3));
                root.AppendChild(node);
            }
        }

        /// <summary>
        /// Get the GEDCOM 5.5 lines for the data in this object.
        /// Lines start at the given level.
        /// </summary>
        /// <param name="tw">A <see cref="TextWriter"/></param>
        /// <param name="level">A <see cref="int"/></param>
        public void Output(TextWriter tw, int level)
        {
            tw.Write(Environment.NewLine);
            tw.Write(level.ToString());
            tw.Write(" ADDR");

            if (!string.IsNullOrEmpty(AddressLine))
            {
                tw.Write(" ");

                Util.SplitLineText(tw, AddressLine, level, 60, 3, true);
            }

            string levelStr = null;
            string levelPlusOne = null;

            if (!string.IsNullOrEmpty(AddressLine1))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = AddressLine1.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" ADR1 ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating AddressLine1");
                }
            }

            if (!string.IsNullOrEmpty(AddressLine2))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = AddressLine2.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" ADR2 ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating AddressLine2");
                }
            }

            if (!string.IsNullOrEmpty(AddressLine3))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = AddressLine3.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" ADR3 ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating AddressLine3");
                }
            }

            if (!string.IsNullOrEmpty(City))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = City.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" CITY ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating City");
                }
            }

            if (!string.IsNullOrEmpty(State))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = State.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" STAE ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating State");
                }
            }

            if (!string.IsNullOrEmpty(PostCode))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = PostCode.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" POST ");
                if (line.Length <= 10)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 10));
                    System.Diagnostics.Debug.WriteLine("Truncating PostCode");
                }
            }

            if (!string.IsNullOrEmpty(Country))
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (level + 1).ToString();
                }

                string line = Country.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" CTRY ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating Country");
                }
            }

            if (!string.IsNullOrEmpty(Phone1))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Phone1.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" PHON ");
                if (line.Length <= 25)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 25));
                    System.Diagnostics.Debug.WriteLine("Truncating Phone1");
                }
            }

            if (!string.IsNullOrEmpty(Phone2))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Phone2.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" PHON ");
                if (line.Length <= 25)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 25));
                    System.Diagnostics.Debug.WriteLine("Truncating Phone2");
                }
            }

            if (!string.IsNullOrEmpty(Phone3))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Phone3.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" PHON ");
                if (line.Length <= 25)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 25));
                    System.Diagnostics.Debug.WriteLine("Truncating Phone3");
                }
            }

            if (!string.IsNullOrEmpty(Fax1))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Fax1.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" FAX ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating Fax1");
                }
            }

            if (!string.IsNullOrEmpty(Fax2))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Fax2.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" FAX ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating Fax2");
                }
            }

            if (!string.IsNullOrEmpty(Fax3))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Fax3.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" FAX ");
                if (line.Length <= 60)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 60));
                    System.Diagnostics.Debug.WriteLine("Truncating Fax3");
                }
            }

            if (!string.IsNullOrEmpty(Email1))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Email1.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" EMAIL ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Email1");
                }
            }

            if (!string.IsNullOrEmpty(Email2))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Email2.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" EMAIL ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Email2");
                }
            }

            if (!string.IsNullOrEmpty(Email3))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Email3.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" EMAIL ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Email3");
                }
            }

            if (!string.IsNullOrEmpty(Www1))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Www1.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" WWW ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Www1");
                }
            }

            if (!string.IsNullOrEmpty(Www2))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Www2.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" WWW ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Www2");
                }
            }

            if (!string.IsNullOrEmpty(Www3))
            {
                if (levelStr == null)
                {
                    levelStr = level.ToString();
                }

                string line = Www3.Replace("@", "@@");

                tw.Write(Environment.NewLine);
                tw.Write(levelStr);
                tw.Write(" WWW ");
                if (line.Length <= 120)
                {
                    tw.Write(line);
                }
                else
                {
                    tw.Write(line.Substring(0, 120));
                    System.Diagnostics.Debug.WriteLine("Truncating Www3");
                }
            }
        }

        private void Changed()
        {
            if (_database == null)
            {
                //System.Console.WriteLine("Changed() called on record with no database set");

                //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                //foreach (System.Diagnostics.StackFrame f in trace.GetFrames())
                //{
                //    System.Console.WriteLine(f);
                //}
            }
            else if (!_database.Loading)
            {
                if (ChangeDate == null)
                {
                    ChangeDate = new GedcomChangeDate(_database);

                    // TODO: what level?
                }

                // TODO: change to SystemTime?
                DateTime now = DateTime.Now;

                ChangeDate.Date1 = now.ToString("dd MMM yyyy");
                ChangeDate.Time = now.ToString("hh:mm:ss");
            }
        }
    }
}