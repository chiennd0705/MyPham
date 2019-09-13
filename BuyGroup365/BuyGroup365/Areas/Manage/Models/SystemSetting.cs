using System;

namespace BuyGroup365.Areas.Manage.Models
{
    public class SystemSettings
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string DesCription { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}