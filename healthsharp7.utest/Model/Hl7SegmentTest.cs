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
    }
}