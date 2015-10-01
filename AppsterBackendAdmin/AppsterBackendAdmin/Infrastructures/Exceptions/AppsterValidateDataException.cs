using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Exceptions
{//start
    public class AppsRequiredDataIsNullException : HttpException
    {
        public AppsRequiredDataIsNullException() : base(750, "Data is required"){}

        public AppsRequiredDataIsNullException(string msg) : base(750, msg) { }
    }

    public class AppsInvalidDataFormatException : HttpException
    {
        public AppsInvalidDataFormatException() : base(700, "Invalid format") { }
        public AppsInvalidDataFormatException(string message) : base(700, message) { }
    }

    public class AppsInvalidEmailFormatException : HttpException
    {
        public AppsInvalidEmailFormatException() : base(708, "Not a valid email address format") { }
    }

    public class AppsOutOfAcceptedAgeException : HttpException
    {
        public AppsOutOfAcceptedAgeException(string message = "Out of accepted age range"): base(723, message){ }
    }

//end
}