using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Damascus.Core
{

    public static class PhoneNormalizer
    {
        public static string NormalizePhone(this string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            var result = new StringBuilder();
            for (int i = 0; i < phoneNumber.Length; i++)
            {
                if (char.IsDigit(phoneNumber[i]))
                    result.Append(phoneNumber[i]);
            }

            var countryCode = "1";

            if (result.Length > 0 && !result.ToString().StartsWith(countryCode))
                result.Insert(0, countryCode);

            if (result[0] != '+')
                result.Insert(0, '+');

            return result.ToString();
        }
    }
}
