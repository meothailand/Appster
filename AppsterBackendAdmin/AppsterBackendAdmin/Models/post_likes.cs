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
    
    public partial class post_likes
    {
        public int id { get; set; }
        public string post_id { get; set; }
        public int post_owner_id { get; set; }
        public string user_id { get; set; }
        public int like { get; set; }
        public System.DateTime created { get; set; }
    }
}