﻿using System;
using System.Collections.Generic;

namespace Common.util
{
    public class UserInfo
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public bool IsSuperUser { get; set; }
        public float DisountPercent { get; set; }
        public DateTime ModifyDate { get; set; }

        public bool Status { get; set; }
        public string StatusMessage { get; set; } //Thông tin
    }

    public class User
    {
        public string Code { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public double? Discount { get; set; }
    }

    public class Objchangepass
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string RepeatPass { get; set; }
        public long MemberId { get; set; }
    }

    public class UserInfoFull
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long? CompanyId { get; set; }
        public bool IsSuperUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ScreenName { get; set; }
        public DateTime LoginDate { get; set; }
        public int FailedLoginAttemp { get; set; }
        public DateTime LockoutDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public double? DisountPercent { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }

        //public bool Status { get; set; }
        //public string StatusMessage { get; set; } //Thông tin
    }

    public class GroupInfo
    {
        public long GroupId { get; set; }
        public string Code { get; set; }
        public string GroupName { get; set; }
    }

    public class RoleInfo
    {
        public long RoleId { get; set; }
        public string Code { get; set; }
        public string RoleName { get; set; }
    }

    public class ModuleInfo
    {
        public long ModuleId { get; set; }
        public string Code { get; set; }
        public string ModuleName { get; set; }
        public string WsMethod { get; set; }
        public string WebMethod { get; set; }
    }

    public class ObjSendMail
    {
        public string Pass { get; set; }
        public string Mail { get; set; }
        public string UrlVeryfly { get; set; }
    }

    public class ProductItem
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string FriendlyUrl { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public int Rate { get; set; }

        public double Weight { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int TotalView { get; set; }
    }

    public class ProductComment
    {
        public Product Product { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public class Manufacturers
    {
        public long ManuId { get; set; }
        public string ManuName { get; set; }
        public string icon { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}