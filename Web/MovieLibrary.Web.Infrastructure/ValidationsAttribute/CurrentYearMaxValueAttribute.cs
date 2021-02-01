namespace MovieLibrary.Web.ValidationsAttribute
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CurrentYearMaxValueAttribute : ValidationAttribute
    {
        private readonly int minValue;

        public CurrentYearMaxValueAttribute(int minValue)
        {
            this.minValue = minValue;
            this.ErrorMessage = $"The year should be between {minValue} and {DateTime.UtcNow.Year}";
        }

        public override bool IsValid(object value)
        {
            if (value is int intValue)
            {
                if (intValue <= DateTime.UtcNow.Year && intValue >= this.minValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
