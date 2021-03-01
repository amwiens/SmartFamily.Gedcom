using SmartFamily.Gedcom.Enums;

using System;

namespace SmartFamily.Gedcom.Models
{
    /// <summary>
    /// TODO: Doc
    /// </summary>
    /// <seealso cref="GedcomRecord"/>
    public class GedcomSubmissionRecord : GedcomRecord, IEquatable<GedcomSubmissionRecord>
    {
        /// <summary>
        /// The submitter.
        /// </summary>
        private string _submitter;

        /// <summary>
        /// The family file.
        /// </summary>
        private string _familyFile;

        /// <summary>
        /// The temple code.
        /// </summary>
        private string _templeCode;

        /// <summary>
        /// The generations of ancestors.
        /// </summary>
        private int _generationsOfAncestors;

        /// <summary>
        /// The generations of descendants.
        /// </summary>
        private int _generationsOfDescendants;

        /// <summary>
        /// The ordinance process flag.
        /// </summary>
        private bool _ordinanceProcessFlag;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomSubmissionRecord"/> class.
        /// </summary>
        public GedcomSubmissionRecord()
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
            get => GedcomRecordType.Submission;
        }

        /// <summary>
        /// Gets the GEDCOM tag for a submission record.
        /// </summary>
        /// <value>
        /// The GEDCOM tag.
        /// </value>
        public override string GedcomTag
        {
            get => "SUBN";
        }

        /// <summary>
        /// Gets or sets the submitter.
        /// </summary>
        /// <value>
        /// The submitter.
        /// </value>
        public string Submitter
        {
            get => _submitter;
            set
            {
                if (value != _submitter)
                {
                    _submitter = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the family file.
        /// </summary>
        /// <value>
        /// The family file.
        /// </value>
        public string FamilyFile
        {
            get => _familyFile;
            set
            {
                if (value != _familyFile)
                {
                    _familyFile = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the temple code.
        /// </summary>
        /// <value>
        /// The temple code.
        /// </value>
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
        /// Gets or sets the generations of ancestors.
        /// </summary>
        /// <value>
        /// The generations of ancestors
        /// </value>
        public int GenerationsOfAncestors
        {
            get => _generationsOfAncestors;
            set
            {
                if (value != _generationsOfAncestors)
                {
                    _generationsOfAncestors = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets the generations of descendants.
        /// </summary>
        /// <value>
        /// The generations of descendants.
        /// </value>
        public int GenerationsOfDecendants
        {
            get => _generationsOfDescendants;
            set
            {
                if (value != _generationsOfDescendants)
                {
                    _generationsOfDescendants = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ordinance process flag].
        /// </summary>
        /// <value>
        /// <c>true</c> if [ordinance process flag]; otherwise, <c>false</c>.
        /// </value>
        public bool OrdinanceProcessFlag
        {
            get => _ordinanceProcessFlag;
            set
            {
                if (value != _ordinanceProcessFlag)
                {
                    _ordinanceProcessFlag = value;
                    Changed();
                }
            }
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="obj">The object to compare this instance against.</param>
        /// <returns>True if the instance matches user data, otherwise false.</returns>
        public override bool IsEquivalentTo(object obj)
        {
            var submission = obj as GedcomSubmissionRecord;

            if (submission == null)
            {
                return false;
            }

            if (!Equals(FamilyFile, submission.FamilyFile))
            {
                return false;
            }

            if (!Equals(GenerationsOfAncestors, submission.GenerationsOfAncestors))
            {
                return false;
            }

            if (!Equals(GenerationsOfDecendants, submission.GenerationsOfDecendants))
            {
                return false;
            }

            if (!Equals(OrdinanceProcessFlag, submission.OrdinanceProcessFlag))
            {
                return false;
            }

            if (!Equals(Submitter, submission.Submitter))
            {
                return false;
            }

            if (!Equals(TempleCode, submission.TempleCode))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compare the user entered data against the passed instance for similarity.
        /// </summary>
        /// <param name="other">The GedcomSubmissionRecord to compare this instance against.</param>
        /// <returns>True if instance matches user data, otherwise false.</returns>
        public bool Equals(GedcomSubmissionRecord other)
        {
            return IsEquivalentTo(other);
        }

        // TODO: add output method
    }
}