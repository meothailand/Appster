using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Exceptions
{
    public class InvalidTokenException : HttpException
    {
        public InvalidTokenException(): base(601, "Invalid token")
        {
        }
        public InvalidTokenException(string message):base(601, message)
        {
        }
    }

    public class TokenExpiredException : HttpException
    {
        public TokenExpiredException()
            : base(602, "Invalid token")
        {
        }
        public TokenExpiredException(string message)
            : base(602, message)
        {
        }
    }

    public class UnAuthorizedException : HttpException
    {
        public UnAuthorizedException():base(603, "Unauthorized access")
        {

        }
    }
    public class UnauthenticatedException : HttpException
    {
        public UnauthenticatedException():base(604, "Unauthenticated")
        {

        }
    }

    public class InvalidUserException : HttpException
    {
        public InvalidUserException() : base(721, "User role is invalid") {  }

        public InvalidUserException(string message) : base(721, message) { }
    }

    public class LoginFailException : HttpException
    {
        public LoginFailException(): base(401, "Login fail")
        {
        }

        public LoginFailException(string message) : base(401, message)
        {

        }
        public LoginFailException(string message, int statusCode): base(statusCode, message)
        {
        }
    }

}