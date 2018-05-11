using System;
using healthsharp7.Model;
using Xunit;

namespace healthsharp7.utest.Model
{
    public class Hl7SegmentTest
    {
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
        public void ShouldParseSegmentsOnlyWithName()
        {
            //Act
            var segment = Hl7Segment.Parse("PID");

            //Assert
            Assert.Equal("PID", segment.Name);
        }

        [Fact]
        public void ShouldLazyParseSegments()
        {
            //Act
            var segment = Hl7Segment.Parse("EVN|A01|20110613083617|||");

            //Assert
            Assert.False(segment.IsParsed);
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
    }
}