using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DealershipManagement.Common
{
    public enum CustomErrors
    {
        #region Generic

        [Description("Forbidden")]
        Forbidden = 403,

        #endregion

        #region Authentication

        [Description("User not found")]
        UserNotFound = 10,

        [Description("Incorrect password")]
        IncorrectPassword = 20,

        [Description("Unauthorized")]
        Unauthorized = 30,

        [Description("Password mismatch")]
        PasswordMismatch = 40,

        [Description("Password could not be set")]
        PasswordNotSet = 50,

        [Description("User is not active")]
        UserNotActive = 60,

        [Description("Account with this email already exists")]
        AccountAlreadyExists = 70,

        [Description("Account creation failed")]
        AccountCreationFailed = 80,

        [Description("Email not sent")]
        EmailNotSent = 90,

        #endregion Authentication

        #region Customer

        [Description("Customer already exists")]
        CustomerAlreadyExists = 500,

        [Description("Customer not found")]
        CustomerNotFound = 510,

        #endregion

        #region Vehicle

        [Description("Vehicle not found")]
        VehicleNotFound = 1000,

        [Description("VIN should be unique")]
        VINAlreadyExists = 1000,

        #endregion
    }
}
