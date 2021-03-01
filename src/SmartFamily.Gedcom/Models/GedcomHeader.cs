using SmartFamily.Gedcom.Enums;

using System;
using System.IO;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// The header from / for a GEDCOM file.
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomHeader : GedcomRecord, IEquatable<GedcomHeader>
    {
        private GedcomNoteRecord _contentDescription;

        private string _submitterXRefID;

        private GedcomDate _transmissionDate;

        private string _copyright;

        private string _language;

        private string _sourceName = string.Empty;
        private GedcomDate _sourceDate;
        private string _sourceCopyright;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomHeader"/> class.
        /// </summary>
        public GedcomHeader()
        {
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <exception cref="Exception">Database can only have one header.</exception>
        public override GedcomDatabase Database
        {
            get => base.Database;
            set
            {
                base.Database = value;
                if (Database != null)
                {
                    if (Database.Header != null)
                    {
                        throw new Exception("Database can only have one header");
                    }

                    Database.Header = this;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        public string ApplicationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string ApplicationVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the application system identifier.
        /// </summary>
        public string ApplicationSystemId { get; set; } = "SmartFamily.Gedcom";

        /// <summary>
        /// Gets or sets the corporation.
        /// </summary>
        public string Corporation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the corporation address.
        /// </summary>
        public GedcomAddress CorporationAddress { get; set; }

        /// <summary>
        /// Gets or sets the content description.
        /// </summary>
        public GedcomNoteRecord ContentDescription
        {
            get => _contentDescription;
            set
            {
                if (value != _contentDescription)
                {
                    _contentDescription = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the submitter x reference identifier.
        /// </summary>
        public string SubmitterXRefID
        {
            get => _submitterXRefID;
            set
            {
                if (_submitterXRefID != value)
                {
                    if (!string.IsNullOrEmpty(_submitterXRefID))
                    {
                        Submitter.Delete();
                    }

                    _submitterXRefID = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the submitter.
        /// </summary>
        public GedcomSubmitterRecord Submitter
        {
            get => Database[SubmitterXRefID] as GedcomSubmitterRecord;
            set
            {
                if (value == null)
                {
                    SubmitterXRefID = null;
                }
                else
                {
                    SubmitterXRefID = value.XRefID;
                }
            }
        }

        /// <summary>
        /// Gets or sets the transmission date.
        /// </summary>
        public GedcomDate TransmissionDate
        {
            get => _transmissionDate;
            set
            {
                if (_transmissionDate != value)
                {
                    _transmissionDate = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        public string Copyright
        {
            get => _copyright;
            set
            {
                if (_copyright != value)
                {
                    _copyright = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        public string SourceName
        {
            get => _sourceName;
            set
            {
                if (_sourceName != value)
                {
                    _sourceName = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the source date.
        /// </summary>
        public GedcomDate SourceDate
        {
            get => _sourceDate;
            set
            {
                if (_sourceDate != value)
                {
                    _sourceDate = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the source copyright.
        /// </summary>
        public string SourceCopyright
        {
            get => _sourceCopyright;
            set
            {
                if (_sourceCopyright != value)
                {
                    _sourceCopyright = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Header;
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the header.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            tw.Write("0 HEAD");

            tw.Write(Environment.NewLine);
            tw.Write("1 SOUR {0}", ApplicationSystemId);

            if (!string.IsNullOrEmpty(ApplicationName))
            {
                tw.Write(Environment.NewLine);
                tw.Write("2 NAME {0}", ApplicationName);
            }

            if (!string.IsNullOrEmpty(ApplicationVersion))
            {
                tw.Write(Environment.NewLine);
                tw.Write("2 VERS {0}", ApplicationVersion);
            }

            if (!string.IsNullOrEmpty(Corporation))
            {
                tw.Write(Environment.NewLine);
                tw.Write("2 CORP {0}", Corporation);
            }

            if (CorporationAddress != null)
            {
                CorporationAddress.Output(tw, 3);
            }

            if (!string.IsNullOrEmpty(SourceName) ||
                !string.IsNullOrEmpty(SourceCopyright) ||
                SourceDate != null)
            {
                tw.Write(Environment.NewLine);
                tw.Write("2 DATA");
                if (!string.IsNullOrEmpty(SourceName))
                {
                    tw.Write(" ");
                    tw.Write(SourceName);
                }

                if (!string.IsNullOrEmpty(SourceCopyright))
                {
                    tw.Write(Environment.NewLine);
                    tw.Write("3 COPR ");
                    tw.Write(SourceCopyright);
                }

                if (SourceDate != null)
                {
                    SourceDate.Output(tw);
                }
            }

            if (TransmissionDate != null)
            {
                TransmissionDate.Output(tw);
            }

            tw.Write(Environment.NewLine);
            tw.Write("1 FILE {0}", Filename);

            if (ContentDescription != null)
            {
                ContentDescription.Output(tw);
            }

            tw.Write(Environment.NewLine);
            tw.Write("1 GEDC");

            tw.Write(Environment.NewLine);
            tw.Write("2 VERS 5.5.1");

            tw.Write(Environment.NewLine);
            tw.Write("2 FORM LINEAGE-LINKED");

            tw.Write(Environment.NewLine);
            tw.Write("1 CHAR UTF-8");

            tw.Write(Environment.NewLine);
            if (!string.IsNullOrWhiteSpace(Language))
            {
                tw.Write($"1 LANG {Language}");
            }

            bool hasSubmitter = !string.IsNullOrEmpty(_submitterXRefID);
            if (hasSubmitter)
            {
                tw.Write(Environment.NewLine);
                tw.Write($"1 SUBM @{_submitterXRefID}@");
            }
        }

        /// <summary>
        /// Checks if the passed header is equal in terms of user content to the current instance.
        /// If new fields are added to the header they should also be added in here for comparison.
        /// </summary>
        /// <param name="obj">The object to compare against this instance.</param>
        /// <returns>Returns <c>true</c> if headers match in user entered content, otherwise <c>false</c>.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            GedcomHeader header = obj as GedcomHeader;

            if (header == null)
            {
                return false;
            }

            if (!Equals(ApplicationName, header.ApplicationName))
            {
                return false;
            }

            if (!Equals(ApplicationSystemId, header.ApplicationSystemId))
            {
                return false;
            }

            if (!Equals(ApplicationVersion, header.ApplicationVersion))
            {
                return false;
            }

            if (!Equals(ContentDescription, header.ContentDescription))
            {
                return false;
            }

            if (!Equals(Copyright, header.Copyright))
            {
                return false;
            }

            if (!Equals(Corporation, header.Corporation))
            {
                return false;
            }

            if (!Equals(CorporationAddress, header.CorporationAddress))
            {
                return false;
            }

            if (!Equals(Filename, header.Filename))
            {
                return false;
            }

            if (!Equals(Language, header.Language))
            {
                return false;
            }

            if (!Equals(SourceCopyright, header.SourceCopyright))
            {
                return false;
            }

            if (!Equals(SourceDate, header.SourceDate))
            {
                return false;
            }

            if (!Equals(SourceName, header.SourceName))
            {
                return false;
            }

            if (!Equals(TransmissionDate, header.TransmissionDate))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the passed header is equal in terms of user content to the current instance.
        /// If new fields are added to the header they should also be added in here for comparison.
        /// </summary>
        /// <param name="other">The GedcomHeader to compare against this instance.</param>
        /// <returns>Returns <c>true</c> if headers match in user entered content, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomHeader other)
        {
            return IsEquivalentTo(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap.
            unchecked
            {
                int hash = 17;

                hash *= 23 + ApplicationName.GetHashCode();
                hash *= 23 + ApplicationSystemId.GetHashCode();
                hash *= 23 + ApplicationVersion.GetHashCode();
                hash *= 23 + ContentDescription.GetHashCode();
                hash *= 23 + Copyright.GetHashCode();
                hash *= 23 + Corporation.GetHashCode();
                hash *= 23 + CorporationAddress.GetHashCode();
                hash *= 23 + Filename.GetHashCode();
                hash *= 23 + Language.GetHashCode();
                hash *= 23 + SourceCopyright.GetHashCode();
                hash *= 23 + SourceDate.GetHashCode();
                hash *= 23 + SourceName.GetHashCode();
                hash *= 23 + TransmissionDate.GetHashCode();

                return hash;
            }
        }
    }
}