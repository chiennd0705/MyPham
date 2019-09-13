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
    
    public partial class Catalogs
    {
        public Catalogs()
        {
            this.CatalogProducts = new HashSet<CatalogProducts>();
            this.CatalogProperties = new HashSet<CatalogProperty>();
            this.Catalogs_Manufacturers = new HashSet<Catalogs_Manufacturers>();
        }
    
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Code { get; set; }
        public string CatalogName { get; set; }
        public string FriendlyUrl { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public int IsLast { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoDescription { get; set; }
        public Nullable<int> TotalVote { get; set; }
        public Nullable<int> VoteScore { get; set; }
        public string Banner { get; set; }
        public Nullable<long> PromotionID { get; set; }
        public string ShareTitle { get; set; }
        public string ShareKeyword { get; set; }
        public string ShareDescription { get; set; }
    
        public virtual ICollection<CatalogProducts> CatalogProducts { get; set; }
        public virtual ICollection<CatalogProperty> CatalogProperties { get; set; }
        public virtual ICollection<Catalogs_Manufacturers> Catalogs_Manufacturers { get; set; }
        public virtual PromotionList PromotionList { get; set; }
    }
}
