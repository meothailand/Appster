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
    
    public partial class role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
        public Nullable<int> updated_by { get; set; }
    }
}
