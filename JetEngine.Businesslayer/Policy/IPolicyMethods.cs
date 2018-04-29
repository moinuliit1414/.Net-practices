using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.Businesslayer.Policy
{
    public interface IPolicyMethods
    {
        String GetPasswordStrength(string password, string strengthConfigText);
        bool IsPasswordExpired(DateTime LastPasswordChange, string days);
        //int GetPasswordMinLength();
        //int GetPasswordMaxLength();
        //int GetPasswordExpiryDays();
        //bool IsComplexPasswordRequired();
        bool IsPasswordComplex(string password);

    }
}
