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
    
    public partial class leaderboard_activations
    {
        public int id { get; set; }
        public int leaderboard_id { get; set; }
        public System.DateTime start_time { get; set; }
        public System.DateTime end_time { get; set; }
        public int activation_status { get; set; }
    }
}
