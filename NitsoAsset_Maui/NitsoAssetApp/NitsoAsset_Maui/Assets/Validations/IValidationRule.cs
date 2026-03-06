using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.Assets.Validations
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