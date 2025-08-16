using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McWuT.Common.Extensions
{
    using System;

    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string representation of a Guid to its Guid equivalent.
        /// Throws a FormatException if the string is not a valid Guid.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The Guid represented by the string.</returns>
        public static Guid ToGuid(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            return Guid.Parse(value);
        }

        /// <summary>
        /// Attempts to convert the string representation of a Guid to its Guid equivalent.
        /// Returns null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The Guid if successful; otherwise, null.</returns>
        public static Guid? ToGuidOrNull(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return Guid.TryParse(value, out var guid) ? guid : null;
        }
    }
}
