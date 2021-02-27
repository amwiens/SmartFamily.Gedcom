using SmartFamily.Gedcom.Enums;
using SmartFamily.Gedcom.Models;

using Xunit;

namespace SmartFamily.Gedcom.Tests.Equality
{
    /// <summary>
    /// Test suite for equality of GedcomSourceCitation.
    /// </summary>
    public class GedcomSourceCitationTest
    {
        private readonly GedcomSourceCitation sourceCit1;
        private readonly GedcomSourceCitation sourceCit2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomSourceCitationTest"/> class.
        /// </summary>
        public GedcomSourceCitationTest()
        {
            sourceCit1 = GenerateSourceCitation();
            sourceCit2 = GenerateSourceCitation();
        }

        [Fact]
        private void Source_citation_is_not_equal_to_null()
        {
            Assert.NotNull(sourceCit1);
        }

        [Fact]
        private void Source_citation_is_not_equal_in_comparison_to_null()
        {
            Assert.Equal(1, sourceCit1.CompareTo(null));
        }

        [Fact]
        private void Source_citation_with_different_certainty_is_not_equal()
        {
            sourceCit1.Certainty = GedcomCertainty.Primary;
            sourceCit2.Certainty = GedcomCertainty.Unknown;

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citation_with_different_date_is_not_equal()
        {
            sourceCit1.Date = new GedcomDate { Date1 = "01/01/1980" };
            sourceCit2.Date = new GedcomDate { Date1 = "12/12/2001" };

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citation_with_different_event_type_is_not_equal()
        {
            sourceCit1.EventType = "event type one";
            sourceCit2.EventType = "event type two";

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citation_with_different_page_is_not_equal()
        {
            sourceCit1.Page = "page one";
            sourceCit2.Page = "page two";

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citation_with_different_role_is_not_equal()
        {
            sourceCit1.Role = "role one";
            sourceCit2.Role = "role two";

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citation_with_different_text_is_not_equal()
        {
            sourceCit1.Text = "sample text one";
            sourceCit2.Text = "sample text two";

            Assert.NotEqual(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citations_with_same_facts_are_equal()
        {
            Assert.Equal(sourceCit1, sourceCit2);
        }

        [Fact]
        private void Source_citations_with_same_facts_are_equal_in_comparison()
        {
            Assert.Equal(0, sourceCit1.CompareTo(sourceCit2));
        }

        private GedcomSourceCitation GenerateSourceCitation()
        {
            return new GedcomSourceCitation
            {
                Certainty = GedcomCertainty.Primary,
                Date = new GedcomDate { Date1 = "01/01/1980" },
                EventType = "sample event type",
                Page = "sample page",
                Role = "sample role",
                Text = "sample text",
            };
        }
    }
}