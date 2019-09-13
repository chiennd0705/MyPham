using System;

namespace BuyGroup365.Areas.Manage.Models
{
    public class PropertybyCate
    {
        public long PropertyId { get; set; }
        public long PropertyValueId { get; set; }
        public bool IsSet { get; set; }
    }

    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }
}