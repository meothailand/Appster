using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Exceptions
{
    public class AppsInvalidTokenException : HttpException
    {
        public AppsInvalidTokenException(): base(601, "Invalid token"){}
        public AppsInvalidTokenException(string message):base(601, message) {}
    }

    public class AppsTokenExpiredException : HttpException
    {
        public AppsTokenExpiredException()
            : base(602, "Invalid token") { }
        public AppsTokenExpiredException(string message)
            : base(602, message) { }
    }

    public class AppsUnAuthorizedException : HttpException
    {
        public AppsUnAuthorizedException():base(603, "Unauthorized access"){ }
    }
    public class AppsUnauthenticatedException : HttpException
    {
        public AppsUnauthenticatedException():base(604, "Unauthenticated"){ }
    }

    public class AppsInvalidUserException : HttpException
    {
        public AppsInvalidUserException() : base(721, "User role is invalid") {  }

        public AppsInvalidUserException(string message) : base(721, message) { }
    }

    public class AppsLoginFailException : HttpException
    {
        public AppsLoginFailException(): base(401, "Login fail"){ }

        public AppsLoginFailException(string message) : base(401, message) { }
        public AppsLoginFailException(string message, int statusCode): base(statusCode, message) { }
    }

    public class AppsDataNotFoundException : HttpException
    {
        /// <summary>
        /// Exception for data that doesn't exist or no longer exist
        /// </summary>
        public AppsDataNotFoundException() : base(651, "No data found"){ }
        /// <summary>
        /// Exception for data that doesn't exist or no longer exist
        /// </summary>
        public AppsDataNotFoundException(string errorMessage) : base(651, errorMessage) { }
    }
    public class AppsEmailAlreadyExistException : HttpException
    {
        /// <summary>
        /// Exception through when an user with the specify email has been exist in the database
        /// </summary>
        public AppsEmailAlreadyExistException() : base(760, "Email has been used"){ }
    }
    public class AppsUsernameAlreadyExistException : HttpException
    {
        /// <summary>
        /// Exception through when an user with the specify username has been exist in the database
        /// </summary>
        public AppsUsernameAlreadyExistException() : base(759, "Username has been used") { }
    }

    public class AppsDatabaseExecutionException : HttpException
    {
        /// <summary>
        /// Exception through when database can't not perform the task without knowing the exact error
        /// </summary>
        public AppsDatabaseExecutionException() : base(1001, "Database execution error") { }

        /// <summary>
        /// Exception through when database can't not perform the task without knowing the exact error
        /// </summary>
        public AppsDatabaseExecutionException(string errorMessage) : base(1001, errorMessage) { }
    }
}