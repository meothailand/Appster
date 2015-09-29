using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Business
{
    public partial class ModifyDataBusiness : BusinessBase
    {
        #region initial
        public static ModifyDataBusiness Instance { get; private set; }
        static ModifyDataBusiness()
        {
            Instance = new ModifyDataBusiness();
        }

        #endregion

        public void SuspendUser(int userId)
        {
            Context.SuspendUser(userId);
        }
    }
}