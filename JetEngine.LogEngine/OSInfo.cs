using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JetEngine.LogEngine
{
    public class OSInfo
    {
        /// <summary> 
        /// Get OS Description
        /// </summary> 
        /// <returns></returns>
        public static String GetOSDescription() {
            String osDescription = String.Empty;
            try
            {
                osDescription = RuntimeInformation.OSDescription;
            }
            catch(Exception ex) {
                osDescription = String.Format("Unable to get OS description. ERROR:{0} ERROR_MESSAGE:{1}", ex.HResult, ex.Message);
            }
            return osDescription;
        }

        /// <summary> 
        /// Get OS Architecture
        /// </summary> 
        /// <returns></returns>
        public static String GetOSArchitecture()
        {
            String osArchitecture = String.Empty;
            try
            {
                osArchitecture = RuntimeInformation.OSArchitecture.ToString();
            }
            catch (Exception ex)
            {
                osArchitecture = String.Format("Unable to get OS Architecture. ERROR:{0} ERROR_MESSAGE:{1}", ex.HResult, ex.Message);
            }
            return osArchitecture;
        }

        /// <summary> 
        /// Get OS Process Architecture
        /// </summary> 
        /// <returns></returns>
        public static String GetOSProcessArchitecture()
        {
            String osProcessArchitecture = String.Empty;
            try
            {
                osProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
            }
            catch (Exception ex)
            {
                osProcessArchitecture = String.Format("Unable to get OS Process Architecture. ERROR:{0} ERROR_MESSAGE:{1}", ex.HResult, ex.Message);
            }
            return osProcessArchitecture;
        }

        /// <summary> 
        /// Get Framework Description
        /// </summary> 
        /// <returns></returns>
        public String GetFrameworkDescription()
        {
            String osFrameworkDescription = String.Empty;
            try
            {
                osFrameworkDescription = RuntimeInformation.FrameworkDescription;
            }
            catch (Exception ex)
            {
                osFrameworkDescription = String.Format("Unable to get OS Framework Description. ERROR:{0} ERROR_MESSAGE:{1}", ex.HResult, ex.Message);
            }
            return osFrameworkDescription;
        }

        /// <summary> 
        /// Get OS platform 
        /// </summary> 
        /// <returns></returns> 
        public static String GetOSPlatform()
        {
            OSPlatform osPlatform;
            try {
                osPlatform = OSPlatform.Create("Other Platform");
                // Check if it's windows 
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                osPlatform = isWindows ? OSPlatform.Windows : osPlatform;
                // Check if it's osx 
                bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
                osPlatform = isOSX ? OSPlatform.OSX : osPlatform;
                // Check if it's Linux 
                bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                osPlatform = isLinux ? OSPlatform.Linux : osPlatform;
                return osPlatform.ToString();
            }
            catch (Exception ex) {
                return String.Format("Unable to get OS Platform. ERROR:{0} ERROR_MESSAGE:{1}", ex.HResult, ex.Message);
            }
            
        }
    }
}
