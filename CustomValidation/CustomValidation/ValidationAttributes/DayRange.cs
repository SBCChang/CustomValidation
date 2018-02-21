using System;
using System.ComponentModel.DataAnnotations;

namespace CustomValidation.ValidationAttributes
{
    public sealed class DayRange : ValidationAttribute
    {
        private readonly string _comparePropertyName;
        private readonly int _minDays;
        private readonly int _maxDays;

        public DayRange(string comparePropertyName, int minDays, int maxDays)
        {
            _comparePropertyName = comparePropertyName;
            _minDays = minDays;
            _maxDays = maxDays;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var compareProperty = validationContext.ObjectType.GetProperty(_comparePropertyName);

            if (compareProperty == null)
            {
                return new ValidationResult($"{_comparePropertyName}不存在");
            }

            if (compareProperty != typeof(DateTime))
            {
                return new ValidationResult($"{_comparePropertyName}非DateTime格式");
            }

            if (value is DateTime == false)
            {
                return new ValidationResult($"目標屬性非DateTime格式");
            }

            var start = Convert.ToDateTime(compareProperty.GetValue(validationContext.ObjectInstance));
            var end = Convert.ToDateTime(value);
            var diffDays = (end - start).TotalDays;

            if (diffDays >= _minDays && diffDays <= _maxDays)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

    }
}