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
    
    public partial class CatalogDownloads
    {
        public CatalogDownloads()
        {
            this.Downloads = new HashSet<Downloads>();
        }
    
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string CatalogDownloadName { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public string Icon { get; set; }
        public string FriendlyUrl { get; set; }
        public string Description { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoDescription { get; set; }
        public string ShareTitle { get; set; }
        public string ShareKeyword { get; set; }
        public string ShareDescription { get; set; }
    
        public virtual ICollection<Downloads> Downloads { get; set; }
    }
}
