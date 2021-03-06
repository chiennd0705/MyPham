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
    
    public partial class Role
    {
        public Role()
        {
            this.RoleModules = new HashSet<RoleModule>();
            this.UserGroupRoles = new HashSet<UserGroupRole>();
        }
    
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Nullable<int> Type { get; set; }
        public System.DateTime Createdate { get; set; }
        public System.DateTime Modifieddate { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        public virtual ICollection<UserGroupRole> UserGroupRoles { get; set; }
    }
}
