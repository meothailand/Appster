using AppsterBackendAdmin.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Models.Business
{
    public class Gifts
    {
        public int id { get; set; }
        public int gift_category_id { get; set; }
        public string gift_name { get; set; }
        public int gift_weight { get; set; }
        public int cost_gold { get; set; }
        public int cost_bean { get; set; }
        public int receiver_gold { get; set; }
        public string image { get; set; }
        public string sp_sound { get; set; }
        public string zh_sound { get; set; }
        public string sound { get; set; }
        public string sp_gift_name { get; set; }
        public string zh_gift_name { get; set; }
        public int status { get; set; }
        public System.DateTime created { get; set; }

        #region additional fields
        public string Category { get; set; }
        public string SoundPath { get; set; }
        #endregion
        public Gifts()
        {

        }

        public Gifts(gift entity)
            : this()
        {
            ModelObjectHelper.CopyObject(entity, this);
            this.image = SiteSettings.GetGiftFilePath(this.image);
            this.SoundPath = !string.IsNullOrWhiteSpace(this.sound) ? SiteSettings.GetGiftFilePath(this.sound) : string.Empty;
        }

        public Gifts(dynamic value)
            : this()
        {
            ModelObjectHelper.CopyObject(value, this);
            this.image = SiteSettings.GetGiftFilePath(this.image);
            this.SoundPath = !string.IsNullOrWhiteSpace(this.sound) ? SiteSettings.GetGiftFilePath(this.sound) : string.Empty;
        }

        public Gifts(gift entity, List<gift_categories> categories)
            : this(entity)
        {
            if (categories.Exists(i => i.id == this.gift_category_id))
                this.Category = categories.SingleOrDefault(i => i.id == gift_category_id).name;
        }
    }
}