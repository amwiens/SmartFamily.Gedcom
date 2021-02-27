using SmartFamily.Gedcom.Parser;

using Xunit;

namespace SmartFamily.Gedcom.Tests.TortureTest
{
    /// <summary>
    /// Loads the torture test files to test every tag can be read at least without falling over.
    /// </summary>
    public class GedcomTortureTest
    {
        [Theory]
        [InlineData(".\\Data\\TortureTEsts\\TGC551.ged")]
        private void Files_can_be_loaded_without_exceptions(string sourceFile)
        {
            GedcomRecordReader.CreateReader(sourceFile);
        }
    }
}