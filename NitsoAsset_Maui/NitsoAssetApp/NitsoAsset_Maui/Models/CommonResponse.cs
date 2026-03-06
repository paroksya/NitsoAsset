using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.Models
{
    public class CommonResponse
    {
        public CommonResponse()
        {
        }
        public string Message { get; set; }
        public string Messages { get; set; }
        public bool Result { get; set; }
    }
}