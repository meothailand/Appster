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
    
    public partial class faq
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string sp_title { get; set; }
        public string sp_description { get; set; }
        public string zh_title { get; set; }
        public string zh_description { get; set; }
        public int status { get; set; }
    }
}
