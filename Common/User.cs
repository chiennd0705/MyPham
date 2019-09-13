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
    
    public partial class User
    {
        public User()
        {
            this.UserUsergroups = new HashSet<UserUsergroup>();
        }
    
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public bool IsSuperUser { get; set; }
        public System.DateTime Createdate { get; set; }
        public System.DateTime Modifydate { get; set; }
        public Nullable<System.DateTime> Expireddate { get; set; }
        public bool PasswordEncrypted { get; set; }
        public System.DateTime PasswordModify_date { get; set; }
        public string PasswordEncryptedMethod { get; set; }
        public string Screenname { get; set; }
        public System.DateTime Logindate { get; set; }
        public Nullable<System.DateTime> LastFailedLoginDate { get; set; }
        public int FailedLoginAttemp { get; set; }
        public bool Lockout { get; set; }
        public System.DateTime LockoutDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<UserUsergroup> UserUsergroups { get; set; }
    }
}
