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
    
    public partial class Member
    {
        public Member()
        {
            this.ShopNewsGroups = new HashSet<ShopNewsGroup>();
            this.ShopSettings = new HashSet<ShopSetting>();
            this.Orders = new HashSet<Order>();
        }
    
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordTransaction { get; set; }
        public long PasswordEncrypted { get; set; }
        public System.DateTime PasswordModifyDate { get; set; }
        public string PasswordEncryptedMethod { get; set; }
        public System.DateTime LoginDate { get; set; }
        public System.DateTime LastFailedLoginDate { get; set; }
        public bool Loutout { get; set; }
        public System.DateTime LockoutDate { get; set; }
        public string Verify { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public System.DateTime ActiveDate { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    
        public virtual MemberProfile MemberProfile { get; set; }
        public virtual MembershipCard MembershipCard { get; set; }
        public virtual ICollection<ShopNewsGroup> ShopNewsGroups { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual ICollection<ShopSetting> ShopSettings { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
