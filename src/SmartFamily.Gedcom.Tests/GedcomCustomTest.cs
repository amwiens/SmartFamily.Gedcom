using SmartFamily.Gedcom.Parser;

using System.Linq;

using Xunit;

namespace SmartFamily.Gedcom.Tests
{
    /// <summary>
    /// Tests for ensuring the custom.ged file can be parsed and custom fields found.
    /// </summary>
    public class GedcomCustomTest
    {
        private GedcomRecordReader GetReader(string file)
        {
            var reader = new GedcomRecordReader();
            reader.ReadGedcom(file);
            return reader;
        }

        [Fact]
        private void Custom_marriage_name_tag_can_be_read()
        {
            var reader = GetReader(".\\Data\\custom.ged");

            var mother = reader.Database.Individuals.SingleOrDefault(x => x.GetName().Name == "/Mother/");

            Assert.Contains(mother._custom, c => c.Tag == "_MARNM");
        }

        [Fact]
        private void Custom_marriage_name_value_can_be_read()
        {
            var reader = GetReader(".\\Data\\custom.ged");

            var mother = reader.Database.Individuals.SingleOrDefault(x => x.GetName().Name == "/Mother/");

            Assert.Contains(mother._custom, c => c.Classification == "/Married name/");
        }
    }
}