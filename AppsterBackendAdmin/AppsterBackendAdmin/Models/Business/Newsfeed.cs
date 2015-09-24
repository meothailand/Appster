using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwinkleStars.Infrastructure.Utils;

namespace AppsterBackendAdmin.Models.Business
{
    public class Newsfeed : BaseBusinessModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        private Newsfeed(){  }
        public Newsfeed(dynamic value) : this()
        {
            ModelObjectHelper.CopyObject(value, this);
            this.Message = this.Message.Replace("You", this.ReceiverName)
                                .Replace("you", this.ReceiverName)
                                .Replace("Your", string.Format("{0}'s", this.ReceiverName))
                                .Replace("your", string.Format("{0}'s", this.ReceiverName));
        }
    }
}