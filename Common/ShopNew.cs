//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShopNew
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public long NewsGroupId { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public string Summary { get; set; }
        public string Descriptions { get; set; }
        public int Status { get; set; }
        public bool isPublic { get; set; }
        public int TotalView { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public long AdminIDApproval { get; set; }
    
        public virtual ShopNewsGroup ShopNewsGroup { get; set; }
    }
}
