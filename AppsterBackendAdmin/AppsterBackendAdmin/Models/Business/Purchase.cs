using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Models.Business
{
    public class Purchase : BaseBusinessModel
    {
        public int Id { get; set; }
        public string PurchaserName { get; set; }
    }
}