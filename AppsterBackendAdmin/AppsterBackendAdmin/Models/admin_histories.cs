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
    
    public partial class admin_histories
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int role_id { get; set; }
        public string description { get; set; }
        public string action { get; set; }
        public string module { get; set; }
        public System.DateTime created { get; set; }
    }
}
