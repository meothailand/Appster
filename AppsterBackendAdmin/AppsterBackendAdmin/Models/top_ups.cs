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
    
    public partial class top_ups
    {
        public int id { get; set; }
        public int bean { get; set; }
        public float price_usd { get; set; }
        public string image { get; set; }
        public int percentage { get; set; }
        public int status { get; set; }
        public System.DateTime created { get; set; }
        public string store_id { get; set; }
    }
}
