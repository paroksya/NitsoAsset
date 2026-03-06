using System;

using Xamarin.Forms;

namespace NitsoAsset.Models
{
    public class VerificationAssetRequestModel
    {
        public string assetcode { get; set; }
        public string CompanyCode { get; set; }
        public string verificationremarks { get; set; }
        public string verificationdoneby { get; set; }
    }

    public class VerificationAssetResponse : CommonResponse
    {
        public bool Response { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseStatus { get; set; }
    }
}