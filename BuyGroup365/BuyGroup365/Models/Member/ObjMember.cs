﻿using Common;
using System;
using System.Collections.Generic;

namespace BuyGroup365.Models.Member
{
    public class ObjOder
    {
        public string TotalMoney { get; set; }
        public string CountAll { get; set; }
        public string CountNotPay { get; set; }
        public string CountPay { get; set; }
        public string CountGetProduct { get; set; }
        public string CountNotGetProduct { get; set; }
        public string CountRecy { get; set; }
    }

    public class CartItem
    {
        public Product Product { set; get; }
        public int Quantity { set; get; }
        public string AvatarUrl { get; set; }

        //public Shop Shop { set; get; }
    }

    public class ShopCartItem
    {
        public List<CartItem> ListCartItems { get; set; }
        public Shop ShopCart { get; set; }
    }

    public class Download
    {
        public string FileName { get; set; }
        public double Size { get; set; }
        public string Description { get; set; }
        public string Modifydate { get; set; }
        public string CreateDate { get; set; }
        public string SoureFile { get; set; }
        public long CatalogDownloadID { get; set; }
    }

    public class AjaxCartProduct
    {
        public long ProductId { get; set; }
        public int Quantity { set; get; }
    }

    public class ProductCart
    {
        public long Id { get; set; }
        public string Model { get; set; }
        public long MemberId { get; set; }

        public Nullable<long> ShopCatalogId { get; set; }
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string FriendlyUrl { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }
        public string AvatarUrl { get; set; }
        public int Quantity { get; set; }
    }

    public class ContentRegister
    {
        public string LinkActive { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
    }

    public class Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Rpass { get; set; }
    }

    public class ModelNews
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public string Summary { get; set; }
        public string Descriptions { get; set; }
        public long NewsGroupId { get; set; }
        public string FriendlyUrl { get; set; }
        public string ModifyDate { get; set; }
    }
}