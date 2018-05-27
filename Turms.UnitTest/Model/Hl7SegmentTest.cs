using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7SegmentTest
    {
        private const string EvnSegmentTrimmed = "EVN|A01|20110613083617";
        private const string EvnSegment = "EVN|A01|20110613083617|||";
        private const string EvnSegmentDollarAsSeparatorTrimmed = "EVN$A01$20110613083617";
        private const string EvnSegmentDollarAsSeparator = "EVN$A01$20110613083617$$$";

        [Fact]
        public void GettingNonExistingFieldShouldReturnFieldWithoutValue()
        {
            //Act
            var segment = Hl7Segment.Parse(EvnSegmentTrimmed);

            //Assert
            Assert.Equal("", segment[4].ToString());
        }

        [Fact]
        public void ParsingNullValueShouldThrowAnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Hl7Segment.Parse(null));
        }

        [Fact]
        public void SegmentNamesShouldBeThreeCharacterLong()
        {
            Assert.Throws<ArgumentException>(() => Hl7Segment.Parse("PI"));
            Assert.Throws<ArgumentException>(() => Hl7Segment.Parse("PI|D"));
            Assert.Throws<ArgumentException>(() => Hl7Segment.Parse("PIDD"));
        }

        [Fact]
        public void SegmentOnlyWithNameShouldBeMarkedAsParsed()
        {
            //Act
            var segment = Hl7Segment.Parse("EVN");
            var segment2 = Hl7Segment.Parse("EVN|");

            //Assert
            Assert.True(segment.IsParsed);
            Assert.True(segment2.IsParsed);
        }

        [Fact]
        public void ShouldBeParsingMessageWithOtherSegmentSeparator()
        {
            //Act
            var encoding = new Hl7Encoding {FieldSeparator = '$'};
            var segment = Hl7Segment.Parse(EvnSegmentDollarAsSeparatorTrimmed, encoding);

            //Assert
            Assert.Equal(EvnSegmentDollarAsSeparatorTrimmed, segment.ToString());
        }

        [Fact]
        public void ShouldLazyParseSegments()
        {
            //Act
            var segment = Hl7Segment.Parse(EvnSegment);

            //Assert
            Assert.False(segment.IsParsed);
        }

        [Fact]
        public void ShouldParseCreatedSegment()
        {
            //Arrange
            var segment = new Hl7Segment() + "EVN" + "A01" + "20110613083617";

            //Assert
            Assert.Equal(EvnSegmentTrimmed, segment.ToString());
        }

        [Fact]
        public void ShouldParseSegmentsOnlyWithName()
        {
            //Act
            var segment = Hl7Segment.Parse("PID");

            //Assert
            Assert.Equal("PID", segment.Name);
        }

        [Fact]
        public void ShouldReturnProperFieldValueUsingArrayNotation()
        {
            //Act
            var segment = Hl7Segment.Parse(EvnSegment);

            //Assert
            Assert.Equal("A01", segment[1].ToString());
            Assert.Equal("", segment[4].ToString());
        }

        [Fact]
        public void ToStringReturnsOriginalStringWithoutTrailingPipes()
        {
            //Arrange
            var segment = Hl7Segment.Parse(EvnSegment);
            var segment2 = Hl7Segment.Parse(EvnSegmentDollarAsSeparator, new Hl7Encoding {FieldSeparator = '$'});

            //Assert
            Assert.Equal(EvnSegmentTrimmed, segment.ToString());
            Assert.Equal(EvnSegmentDollarAsSeparatorTrimmed, segment2.ToString());
        }

        [Fact]
        public void ShouldReturnAddedField()
        {
            //Arrange
            var segmentString = "EVN";

            //Act
            var segment = Hl7Segment.Parse(segmentString);
            segment[1] = new Hl7Field("A01");
            segment[2] = new Hl7Field("20110613083617");
            var field1 = segment[1].ToString();
            var field2 = segment[2].ToString();

            //Assert
            Assert.Equal("A01", field1);
            Assert.Equal("20110613083617", field2);
        }

        [Fact]
        public void ShouldReturnEmptyFieldByDefault()
        {
            //Arrange
            var segmentString = "EVN|A01|20110613083617";

            //Act
            var segment = Hl7Segment.Parse(segmentString);
            var field = segment[3].ToString();

            //Assert
            Assert.Equal("", field);
        }
    }
}