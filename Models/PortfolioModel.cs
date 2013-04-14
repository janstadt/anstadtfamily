using photoshare.Models.Enums;
using System;
using System.Collections.Generic;
namespace photoshare.Models
{
    public class PortfolioModel
    {
        public string Id { get; set; }
        public List<PhotoAlbumModel> Albums { get; set; }
    }
}