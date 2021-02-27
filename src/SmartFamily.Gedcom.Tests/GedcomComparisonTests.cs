using SmartFamily.Gedcom.Models;

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace SmartFamily.Gedcom.Tests
{
    /// <summary>
    /// Class GedcomComparisonTests.
    /// </summary>
    public class GedcomComparisonTests
    {
        /// <summary>
        /// GEDCOM comparison gedcom association is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomAssociation_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomAssociation();
            var object2 = new GedcomAssociation();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom date is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomDate_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomDate();
            var object2 = new GedcomDate();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom event is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomEvent_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomEvent { Database = new GedcomDatabase() };
            var object2 = new GedcomEvent { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom family link is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomFamilyLink_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomFamilyLink();
            var object2 = new GedcomFamilyLink();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom family record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomFamilyRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomFamilyRecord { Database = new GedcomDatabase() };
            var object2 = new GedcomFamilyRecord { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom header is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomHeader_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomHeader();
            var object2 = new GedcomHeader();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom individual record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomIndividualRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomIndividualRecord();
            var object2 = new GedcomIndividualRecord();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom multimedia record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomMultimediaRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomMultimediaRecord { Database = new GedcomDatabase() };
            var object2 = new GedcomMultimediaRecord { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom name is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomName_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomName();
            var object2 = new GedcomName();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom note record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomNoteRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomNoteRecord();
            var object2 = new GedcomNoteRecord();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom place is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomPlace_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomPlace { Database = new GedcomDatabase() };
            var object2 = new GedcomPlace { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom repository citation is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomRepositoryCitation_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomRepositoryCitation();
            var object2 = new GedcomRepositoryCitation();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom repository record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomRepositoryRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomRepositoryRecord { Database = new GedcomDatabase() };
            var object2 = new GedcomRepositoryRecord { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom source citation is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomSourceCitation_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomSourceCitation();
            var object2 = new GedcomSourceCitation();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom source record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomSourceRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomSourceRecord { Database = new GedcomDatabase() };
            var object2 = new GedcomSourceRecord { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom submission record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomSubmissionRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomSubmissionRecord();
            var object2 = new GedcomSubmissionRecord();

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }

        /// <summary>
        /// GEDCOM comparison gedcom submitter record is equivalent to expect are equal.
        /// </summary>
        [Fact]
        public void GedcomComparison_GedcomSubmitterRecord_IsEquivalentTo_ExpectAreEqual()
        {
            // Arrange
            var object1 = new GedcomSubmitterRecord { Database = new GedcomDatabase() };
            var object2 = new GedcomSubmitterRecord { Database = new GedcomDatabase() };

            // Act and Assert
            Assert.True(object1.IsEquivalentTo(object2));
            Assert.True(object2.IsEquivalentTo(object1));
        }
    }
}