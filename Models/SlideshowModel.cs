using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class SlideshowModel
    {
        public bool ShowIndicator { get; set; }
        public bool ShowNavigation { get; set; }
        public List<PhotoModel> Items { get; set; }
        public string Id { get; set; }
        public SlideshowModel()
        {
            this.Items = new List<PhotoModel>();
        }
    }
}