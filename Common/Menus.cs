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
    
    public partial class Menus
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public long ParentId { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
        public string Link { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int TypeMenu { get; set; }
        public string BackGround { get; set; }
        public Nullable<bool> IsBackGround { get; set; }
    }
}
