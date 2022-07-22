using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.ApplicationConstants
{
    public static class AuthenticationConstants
    {
        public static string SchemeDescription = "SchemeDescription";
        public static string SchemeName = "SchemeName";
        public static string Scheme = "Scheme";
        public static string BearerFormat = "BearerFormat";
        public static string MaxFailedLoginAttempts = "MaxFailedLoginAttempts";
        public static string LockoutTimeSpan = "LockoutTimeSpan";
        public static string JwtSecret = "Jwt Secret";
        public static string Issuer = "Issuer";
        public static string Audience = "Audience";
        public static string AccessTokenExpiryTime = "AccessTokenExpiryTime";
        public static string SlidingExpirationTimeInHours = "SlidingExpirationTimeInHours";
    }
}
