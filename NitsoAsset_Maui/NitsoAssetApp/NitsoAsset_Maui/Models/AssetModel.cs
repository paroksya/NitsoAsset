using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Models
{
    public class AssetRequestModel
    {
        public string assetcode { get; set; }
        public string CompanyCode { get; set; }
    }

    public class Asset
    {
        public string assetcode { get; set; }
        public string asset_pid { get; set; }
        public string block_fid { get; set; }
        public string blockname { get; set; }
        public string category_fid { get; set; }
        public string categoryname { get; set; }
        public string subcategory_fid { get; set; }
        public string subcategoryname { get; set; }
        public string location_fid { get; set; }
        public string location_name { get; set; }
        public string sublocation_fid { get; set; }
        public string sublocation_name { get; set; }
        public string costcenter_fid { get; set; }
        public string cost_center_name { get; set; }
        public string department_fid { get; set; }
        public string dept_name { get; set; }
        public string custodian_fid { get; set; }
        public string custodian_name { get; set; }
        public string assetuser_fid { get; set; }
        public string asset_user_name { get; set; }
        public string ownershiptype_fid { get; set; }
        public object ownershiptype { get; set; }
        public string assetdescription { get; set; }
        public string assetremarks { get; set; }
        public string purchasedate { get; set; }
        public string usedate { get; set; }
        public string physicalno { get; set; }
        public string assetcost { get; set; }
        public string accrueddepreciation { get; set; }
        public string cenvat { get; set; }
        public string gst { get; set; }
        public string quantity { get; set; }
        public string measurementunit_fid { get; set; }
        public string measurementunitname { get; set; }
        public string qtywisedep { get; set; }
        public string leasedate { get; set; }
        public string asset_status { get; set; }
        public string status_name { get; set; }
        public string usein_shift { get; set; }
        public string revalue_entry { get; set; }
        public string usefull_life { get; set; }
        public string manufacturer_fid { get; set; }
        public string manufacture_name { get; set; }
        public string vendor_fid { get; set; }
        public string vendorname { get; set; }
        public string serial_no { get; set; }
        public string model_no { get; set; }
        public string chassis_no { get; set; }
        public string doc_no { get; set; }
        public string doc_date { get; set; }
        public string make_of { get; set; }
        public string po_no { get; set; }
        public string po_date { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string voucher_no { get; set; }
        public string vaucher_date { get; set; }
        public object total_amount { get; set; }
        public string remarks { get; set; }
        public string colour { get; set; }
        public string tag_location { get; set; }
        public string boe_no { get; set; }
        public string boe_date { get; set; }
        public string road_permit_no { get; set; }
        public string warrenty_upto { get; set; }
        public string company_fid { get; set; }
    }

    public class AssetResponse : CommonResponse
    {
        public Asset Response { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseStatus { get; set; }
    }
}