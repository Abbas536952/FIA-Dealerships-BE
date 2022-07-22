using DealershipManagement.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Text;

namespace DealershipManagement.Common.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Allows throw a <see cref="CustomErrorException" /> in a better manner. Side benefit, JIT may now inline callers.
        /// </summary>
        /// <param name="value">Error value</param>
        /// <param name="code">Http status code</param>
        /// <typeparam name="T">Error enumeration type</typeparam>
        /// <returns>This method never returns.</returns>
        /// <exception>Throws a CustomErrorException.</exception>
        public static void ThrowCustomErrorException<T>(this T value, HttpStatusCode code, string description = null)
            where T : Enum
        {
            var errorDescription = string.IsNullOrEmpty(description) ? value.GetDescription() : description;
            throw new CustomErrorException((int)Convert.ChangeType(value, typeof(int)), code, errorDescription);
        }

        /// <summary>
        /// Extract the descrption of the enum
        /// </summary>
        /// <returns>description of enum</returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();

            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            var field = type.GetField(name);
            if (field == null)
            {
                return null;
            }

            var attr = field.GetCustomAttribute<DescriptionAttribute>();

            return attr?.Description;
        }
    }
}
