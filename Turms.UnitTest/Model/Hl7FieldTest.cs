using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7FieldTest
    {
        [Fact]
        public void ParsingWithANullEncodingShouldThrowAnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Hl7Field.Parse("1", null));
        }

        [Fact]
        public void ShouldReturnSecondRepetition()
        {
            //Arrange
            var fieldString = @"This\.br\is\.br\A Test~MoreText~SomeMoreText";

            //Act
            var field = Hl7Field.Parse(fieldString);
            var repetition = field[2].ToString();

            //Assert
            Assert.Equal("MoreText", repetition);
        }

        [Fact]
        public void ShouldReturnEmptyRepetitionByDefault()
        {
            //Arrange
            var fieldString = @"This\.br\is\.br\A Test~MoreText~SomeMoreText";

            //Act
            var field = Hl7Field.Parse(fieldString);
            var repetition = field[4].ToString();

            //Assert
            Assert.Equal("", repetition);
        }

        [Fact]
        public void ShouldParseCreatedField()
        {
            //Arrange
            var field = new Hl7Field() + @"This\.br\is\.br\A Test"+"MoreText"+"SomeMoreText";

            //Assert
            Assert.Equal(@"This\.br\is\.br\A Test~MoreText~SomeMoreText", field.ToString());
        }
    }
}