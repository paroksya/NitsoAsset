using System;
using Xamarin.Forms;
namespace NitsoAsset.Assets.Validations
{
    public interface IValidationRule<T>
    {
        String ValidationMessage
        {
            get;
            set;
        }
        bool Validate(T value);
        bool Validate(T value, T value1);
    }
}