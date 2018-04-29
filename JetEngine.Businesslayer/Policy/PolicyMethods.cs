using JetEngine.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetEngine.Businesslayer.Policy
{
    public class PolicyMethods: IPolicyMethods
    {
        public string GetPasswordStrength(string password, string strengthConfigText)
        {
            string strengthText = "Weak";
            var text = strengthConfigText.Split('|');
            int passLength = password.Length;
            if (text.Length>2 && !String.IsNullOrEmpty(text[1]) && Converter.StringToIntegerWithDefult(text[1],6)<= passLength) {
                strengthText = "Medium";
            }
            if (text.Length > 3 && !String.IsNullOrEmpty(text[2]) && Converter.StringToIntegerWithDefult(text[2],8) <= passLength)
            {
                strengthText = "Strong";
            }
            return strengthText;
        }

        public virtual bool IsPasswordComplex(String password)
        {
            if (!password.Any(c => char.IsUpper(c)) || !password.Any(c => char.IsLower(c)) ||
                !password.Any(c => char.IsDigit(c)) || !password.Any(c => char.IsSymbol(c))) {
                return false;
            }
            return true;
        }

        public bool IsPasswordExpired(DateTime LastPasswordChange, string expirationDays)
        {
            int days = Converter.StringToIntegerWithDefult(expirationDays, 30);//30 defult Expired time;
            DateTime expairedAt = LastPasswordChange.AddDays(7);
            return expairedAt < DateTime.Now;
        }
    }
}
