using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Utils
{
    public class ImageHelper
    {
        public static ImageHelper Instance { get; private set; }
        static ImageHelper()
        {
            Instance = new ImageHelper();
        }
    }
}