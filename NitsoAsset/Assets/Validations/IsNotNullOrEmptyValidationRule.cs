using System;
using Xamarin.Forms;
namespace NitsoAsset.Assets.Validations
{
    public class IsNotNullOrEmptyValidationRule<T> : IValidationRule<T> where T : class
    {
        public string ValidationMessage { get; set; }

        public bool Validate(T value)
        {
            if (value == null)
                return false;

            var stringValue = value as String;
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        public bool Validate(T value, T value1)
        {
            return false;
        }
    }
}