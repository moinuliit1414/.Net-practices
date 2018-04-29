using System;

namespace JetEngine.Helper
{
    public class Converter
    {
        public static String DateToStringGlobal(DateTime dateTime)
        {
            return dateTime.ToString("yyyy - MM - ddTHH:mm: ss.fffffffzzz");
        }
        public static DateTime StringToDateGlobal(String dateTime)
        {
            return DateTime.ParseExact(dateTime,"yyyy - MM - ddTHH:mm: ss.fffffffzzz", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static bool ObjectToBoolean(object value)
        {
            switch (value.ToString().ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    throw new InvalidCastException("You can't cast a weird value to a bool!");
            }
        }
        public static String BooleanToStringTrueFalse(bool value)
        {
            switch (value)
            {
                case true:
                    return "True";
                case false:
                    return "False";
                default:
                    throw new InvalidCastException("You can't cast a weird value to a String!");
            }
        }
        public static String BooleanToStringTF(bool value)
        {
            switch (value)
            {
                case true:
                    return "T";
                case false:
                    return "F";
                default:
                    throw new InvalidCastException("You can't cast a weird value to a String!");
            }
        }
        public static String BooleanToString10(bool value)
        {
            switch (value)
            {
                case true:
                    return "1";
                case false:
                    return "0";
                default:
                    throw new InvalidCastException("You can't cast a weird value to a String!");
            }
        }
        public static int BooleanToInteger(bool value)
        {
            switch (value)
            {
                case true:
                    return 1;
                case false:
                    return 0;
                default:
                    throw new InvalidCastException("You can't cast a weird value to a Integer!");
            }
        }
        public static int? StringToInteger(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return (int?)null;
            }
            try
            {
                return Int32.Parse(value.Trim());
            }
            catch (Exception ex) {
                throw new InvalidCastException(String.Format("stringToInteger Parse failed!!.Details:{0}",ex.InnerException));
            }
        }
        
        public static int StringToIntegerWithDefult(string value,int defult=0)
        {
            int? val = StringToInteger(value);
            return val.HasValue ? val.Value : defult;
        }

        public static int DateTimeDiffInMiliseconds(DateTime dt1, DateTime dt2) {
            TimeSpan span = dt2 - dt1;
            int ms = (int)span.TotalMilliseconds;
            return ms;
        }
        public static int DateTimAbseDiffInMiliseconds(DateTime dt1, DateTime dt2)
        {
            return Math.Abs(DateTimeDiffInMiliseconds(dt1, dt2));
        }
    }
}
