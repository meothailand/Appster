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
    
    public partial class chat
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int receiver_user_id { get; set; }
        public string message { get; set; }
        public int sender_read_status { get; set; }
        public int receiver_read_status { get; set; }
        public string sender_delete { get; set; }
        public int receiver_delete { get; set; }
        public string status { get; set; }
        public System.DateTime created { get; set; }
        public Nullable<int> message_type { get; set; }
    }
}
