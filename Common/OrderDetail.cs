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
    
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public Nullable<long> IdOrder { get; set; }
        public Nullable<long> IdProduct { get; set; }
        public string NameProduct { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<double> Point { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Description { get; set; }
        public string PathImage { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
