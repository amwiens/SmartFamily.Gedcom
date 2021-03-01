using SmartFamily.Gedcom.Enums;

using System;
using System.IO;
using System.Text;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// A multimedia record, this can consist of any number of files
    /// of varying types
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomMultimediaRecord : GedcomRecord, IEquatable<GedcomMultimediaRecord>
    {
        private readonly GedcomRecordList<GedcomMultimediaFile> _files;

        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomMultimediaRecord"/> class.
        /// </summary>
        public GedcomMultimediaRecord()
        {
            _files = new GedcomRecordList<GedcomMultimediaFile>();
            _files.CollectionChanged += ListChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomMultimediaRecord"/> class.
        /// </summary>
        /// <param name="database"></param>
        public GedcomMultimediaRecord(GedcomDatabase database)
            : this()
        {
            Database = database;
            Level = 0;

            XRefID = database.GenerateXref("OBJE");
            database.Add(XrefId, this);
        }

        /// <summary>
        /// Gets the type of the record.
        /// </summary>
        public override GedcomRecordType RecordType
        {
            get => GedcomRecordType.Multimedia;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a multimedia record.
        /// </summary>
        public override string GedcomTag
        {
            get => "OBJE";
        }

        /// <summary>
        /// Gets the multimedia files.
        /// </summary>
        public GedcomRecordList<GedcomMultimediaFile> Files
        {
            get => _files;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (GedcomMultimediaFile file in _files)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(file.Filename);
                    }

                    _title = sb.ToString();
                }

                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
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
                foreach (GedcomMultimediaFile file in Files)
                {
                    childChangeDate = file.ChangeDate;
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
        /// Compares the two passed records by title.
        /// </summary>
        /// <param name="mediaA">The first multimedia record.</param>
        /// <param name="mediaB">The second multimedia record.</param>
        /// <returns>
        /// &lt;0 if the first record's title precedes the second in the sort order;
        /// &gt;0 if the second record's title precedes the first;
        /// 0 if the titles are equal
        /// </returns>
        public static int CompareByTitle(GedcomMultimediaRecord mediaA, GedcomMultimediaRecord mediaB)
        {
            return string.Compare(mediaA.Title, mediaB.Title);
        }

        /// <summary>
        /// Adds the multimedia file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void AddMultimediaFile(string filename)
        {
            FileInfo info = new FileInfo(filename);

            GedcomMultimediaFile file = new GedcomMultimediaFile
            {
                Database = Database,

                Filename = filename,
                Format = info.Extension
            };

            _files.Add(file);
        }

        /// <summary>
        /// Output GEDCOM formatted text representing the multimedia record.
        /// </summary>
        /// <param name="tw">The writer to output to.</param>
        public override void Output(TextWriter tw)
        {
            base.Output(tw);

            string levelPlusOne = null;
            string levelPlusTwo = null;

            foreach (GedcomMultimediaFile file in _files)
            {
                if (levelPlusOne == null)
                {
                    levelPlusOne = (Level + 1).ToString();
                    levelPlusTwo = (Level + 2).ToString();
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusOne);
                tw.Write(" FILE ");

                // TODO: we don't support BLOB so we can end up without a filename
                if (!string.IsNullOrEmpty(file.Filename))
                {
                    tw.Write(file.Filename);
                }

                tw.Write(Environment.NewLine);
                tw.Write(levelPlusTwo);
                tw.Write(" FORM ");
                if (!string.IsNullOrEmpty(file.Format))
                {
                    tw.Write(file.Format);
                }
                else
                {
                    tw.Write("Unknown");
                }

                if (!string.IsNullOrEmpty(file.SourceMediaType))
                {
                    tw.Write(Environment.NewLine);
                    tw.Write(levelPlusTwo);
                    tw.Write(" MEDI ");
                    tw.Write(file.SourceMediaType);
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
            var media = obj as GedcomMultimediaRecord;

            if (media == null)
            {
                return false;
            }

            if (!GedcomGenericListComparer.CompareLists(Files, media.Files))
            {
                return false;
            }

            if (!Equals(Title, media.Title))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomMultimediaRecord to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public bool Equals(GedcomMultimediaRecord other)
        {
            return IsEquivalentTo(other);
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns><c>True</c> if instance matches user data, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return IsEquivalentTo(obj);
        }

        public override int GetHashCode()
        {
            return new
            {
                Files,
                Title,
            }.GetHashCode();
        }
    }
}