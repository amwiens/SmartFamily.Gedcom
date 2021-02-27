using SmartFamily.Gedcom.Models;

using Xunit;

namespace SmartFamily.Gedcom.Tests.Equality
{
    /// <summary>
    /// Test suite for equality of GedcomNoteRecord.
    /// </summary>
    public class GedcomNoteRecordTest
    {
        private readonly GedcomNoteRecord noteRecord1;
        private readonly GedcomNoteRecord noteRecord2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomNoteRecordTest"/> class.
        /// </summary>
        public GedcomNoteRecordTest()
        {
            noteRecord1 = GenerateNoteRecord();
            noteRecord2 = GenerateNoteRecord();
        }

        [Fact]
        private void Note_record_is_not_equal_to_null()
        {
            Assert.NotNull(noteRecord1);
        }

        [Fact]
        private void Note_record_with_different_text_is_not_equal()
        {
            noteRecord1.Text = "note one";
            noteRecord2.Text = "note two";

            Assert.NotEqual(noteRecord1, noteRecord2);
        }

        [Fact]
        private void Note_records_with_same_facts_are_equal()
        {
            Assert.Equal(noteRecord1, noteRecord2);
        }

        private GedcomNoteRecord GenerateNoteRecord()
        {
            return new GedcomNoteRecord
            {
                Text = "sample note",
            };
        }
    }
}