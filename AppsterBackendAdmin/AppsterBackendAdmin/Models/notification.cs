//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppsterBackendAdmin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class notification
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public int fb_acc { get; set; }
        public int twitter_acc { get; set; }
        public int notices { get; set; }
        public int nearby_feature { get; set; }
        public int searchable { get; set; }
        public int notices_sound { get; set; }
        public int language { get; set; }
    }
}