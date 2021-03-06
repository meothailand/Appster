﻿using AppsterBackendAdmin.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Models.Business
{
    public class User : BaseBusinessModel
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public Nullable<int> ref_id { get; set; }
        public int referral_id { get; set; }
        public string username { get; set; }
        public string display_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string paypal_email { get; set; }
        public string image { get; set; }
        public string user_image { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public int type { get; set; }
        public int facebook_account { get; set; }
        public int twitter_account { get; set; }
        public int instagram_account { get; set; }
        public int gift_received_count { get; set; }
        public int gift_sent_count { get; set; }
        public int total_gold { get; set; }
        public int total_bean { get; set; }
        public string address { get; set; }
        public int status { get; set; }
        public DateTime created { get; set; }

        #region additional fields
        public string AccessLevel { get; set; }
        public string LatLong { get; set; }
        public string UserImageLink { get; set; }

        #endregion

        public User() { }

        public User(user entity) : this()
        {
            ModelObjectHelper.CopyObject(entity, this);
            this.UserImageLink = SiteSettings.GetProfileImagePath(this.image);
            this.gender = string.IsNullOrWhiteSpace(this.gender) ? "Male" : this.gender;
            this.LatLong = string.Format("{0},{1}", string.IsNullOrWhiteSpace(entity.latitude) ? "0" : entity.latitude,
                string.IsNullOrWhiteSpace(entity.longitude) ? "0" : entity.longitude);
        }

        public User(user entity, IEnumerable<role> roles) : this(entity)
        {
            if(roles.Any(i => i.id == this.role_id))
                this.AccessLevel = roles.SingleOrDefault(i => i.id == this.role_id).name;
        }
        
    }
}