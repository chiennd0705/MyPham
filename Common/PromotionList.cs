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
    
    public partial class PromotionList
    {
        public PromotionList()
        {
            this.Catalogs = new HashSet<Catalog>();
            this.Products = new HashSet<Product>();
            this.PromotionItem = new HashSet<PromotionItem>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifyDate { get; set; }
    
        public virtual ICollection<Catalog> Catalogs { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<PromotionItem> PromotionItem { get; set; }
    }
}
