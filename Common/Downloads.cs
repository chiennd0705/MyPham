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
    
    public partial class Downloads
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public Nullable<double> Size { get; set; }
        public string Description { get; set; }
        public System.DateTime Modifydate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string SoureFile { get; set; }
        public long CatalogDownloadID { get; set; }
    
        public virtual CatalogDownloads CatalogDownloads { get; set; }
    }
}
