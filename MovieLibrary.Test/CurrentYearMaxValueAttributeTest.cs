namespace MovieLibrary.Test
{
    using System;

    using Xunit;

    using MovieLibrary.Web.ValidationsAttribute;

    public class CurrentYearMaxValueAttributeTest
    {
        [Fact]
        public void IsValidReturnsFalseForDateTimeAfterCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.Year+1);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidReturnsTrueForDateTimeBeforeCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.Year - 1);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValidReturnsTrueForDateTimeEqualCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.Year);

            // Assert
            Assert.True(isValid);
        }
    }
}
