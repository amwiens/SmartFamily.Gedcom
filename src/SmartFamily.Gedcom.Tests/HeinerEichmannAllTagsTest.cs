using SmartFamily.Gedcom.Enums;
using SmartFamily.Gedcom.Parser;

using Xunit;

namespace SmartFamily.Gedcom.Tests
{
    /// <summary>
    /// Ensures that the parser loads a file that contains all known GEDCOM tags.
    /// TODO: Could do with validating that it actually understood every tag in that file.
    /// </summary>
    public class HeinerEichmannAllTagsTest
    {
        /// <summary>
        /// File sourced from http://heiner-eichmann.de/gedcom/allged.htm.
        /// </summary>
        [Fact]
        private void Heiner_Eichmanns_test_file_with_nearly_all_tags_loads_and_parses()
        {
            var loader = new GedcomLoader();

            var result = loader.LoadAndParse("allged.ged");

            Assert.Equal(GedcomErrorState.NoError, result.ErrorState);
        }
    }
}