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
    
    public partial class paypal_settings
    {
        public int id { get; set; }
        public string signature { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int is_live { get; set; }
        public int group_buy_credit { get; set; }
        public int Promotion_credit { get; set; }
        public int gold { get; set; }
        public int bean { get; set; }
    }
}
