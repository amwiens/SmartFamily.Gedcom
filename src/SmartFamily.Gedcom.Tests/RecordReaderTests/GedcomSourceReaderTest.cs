using SmartFamily.Gedcom.Parser;

using System.Linq;

using Xunit;

namespace SmartFamily.Gedcom.Tests.RecordReaderTests
{
    /// <summary>
    /// Tests that source records are read in for the varying record types.
    /// </summary>
    public class GedcomSourceReaderTest
    {
        [Fact]
        private void Correct_number_of_sources_loaded_for_individual()
        {
            var reader = GedcomRecordReader.CreateReader(".\\Data\\multiple-sources.ged");
            string personId = reader.Parser.XrefCollection["P1"];

            var individual = reader.Database.Individuals.First(i => i.XRefID == personId);

            Assert.Single(individual.Birth.Sources);
            Assert.Single(individual.Death.Sources);
        }
    }
}