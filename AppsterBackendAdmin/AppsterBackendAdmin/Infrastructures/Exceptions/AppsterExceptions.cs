using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Exceptions
{
    public class InvalidTokenException : HttpException
    {
        public InvalidTokenException(): base(601, "Invalid token"){}
        public InvalidTokenException(string message):base(601, message) {}
    }

    public class TokenExpiredException : HttpException
    {
        public TokenExpiredException()
            : base(602, "Invalid token") { }
        public TokenExpiredException(string message)
            : base(602, message) { }
    }

    public class UnAuthorizedException : HttpException
    {
        public UnAuthorizedException():base(603, "Unauthorized access"){ }
    }
    public class UnauthenticatedException : HttpException
    {
        public UnauthenticatedException():base(604, "Unauthenticated"){ }
    }

    public class InvalidUserException : HttpException
    {
        public InvalidUserException() : base(721, "User role is invalid") {  }

        public InvalidUserException(string message) : base(721, message) { }
    }

    public class LoginFailException : HttpException
    {
        public LoginFailException(): base(401, "Login fail"){ }

        public LoginFailException(string message) : base(401, message) { }
        public LoginFailException(string message, int statusCode): base(statusCode, message) { }
    }

    public class DataNotFoundException : HttpException
    {
        /// <summary>
        /// Exception for data that doesn't exist or no longer exist
        /// </summary>
        public DataNotFoundException() : base(651, "No data found"){ }
        /// <summary>
        /// Exception for data that doesn't exist or no longer exist
        /// </summary>
        public DataNotFoundException(string errorMessage) : base(651, errorMessage) { }
    }
    public class EmailAlreadyExistException : HttpException
    {
        /// <summary>
        /// Exception through when an user with the specify email has been exist in the database
        /// </summary>
        public EmailAlreadyExistException() : base(760, "Email has been used"){ }
    }
    public class UsernameAlreadyExistException : HttpException
    {
        /// <summary>
        /// Exception through when an user with the specify username has been exist in the database
        /// </summary>
        public UsernameAlreadyExistException() : base(759, "Username has been used") { }
    }

    public class DatabaseExecutionException : HttpException
    {
        /// <summary>
        /// Exception through when database can't not perform the task without knowing the exact error
        /// </summary>
        public DatabaseExecutionException() : base(1001, "Database execution error") { }

        /// <summary>
        /// Exception through when database can't not perform the task without knowing the exact error
        /// </summary>
        public DatabaseExecutionException(string errorMessage) : base(1001, errorMessage) { }
    }
}