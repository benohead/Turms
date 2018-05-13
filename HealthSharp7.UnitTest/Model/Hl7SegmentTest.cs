using System;
using healthsharp7.Model;
using HealthSharp7.Model;
using Xunit;

namespace healthsharp7.UnitTest.Model
{
    public class Hl7SegmentTest
    {
        private const string EvnSegmentTrimmed = "EVN|A01|20110613083617";
        private const string EvnSegment = "EVN|A01|20110613083617|||";
        private const string EvnSegmentDollarAsSeparatorTrimmed = "EVN$A01$20110613083617";
        private const string EvnSegmentDollarAsSeparator = "EVN$A01$20110613083617$$$";

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
        public void ShouldLazyParseSegments()
        {
            //Act
            var segment = Hl7Segment.Parse(EvnSegment);

            //Assert
            Assert.False(segment.IsParsed);
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
        public void GettingNonExistingFieldShouldReturnFieldWithoutValue()
        {
            //Act
            var segment = Hl7Segment.Parse(EvnSegmentTrimmed);

            //Assert
            Assert.Equal("", segment[4].ToString());
        }

        [Fact]
        public void ShouldBeParsingMessageWithOtherSegmentSeparator()
        {
            //Act
            Hl7Encoding encoding = new Hl7Encoding { FieldSeparator = '$' };
            var segment = Hl7Segment.Parse(EvnSegmentDollarAsSeparatorTrimmed, encoding);

            //Assert
            Assert.Equal(EvnSegmentDollarAsSeparatorTrimmed, segment.ToString());
        }
    }
}