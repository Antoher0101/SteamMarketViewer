using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace SteamMarketViewer.SteamAuth
{
    public class Util
    {
        public static long GetSystemUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            int hexLen = hex.Length;
            byte[] ret = new byte[hexLen / 2];
            for (int i = 0; i < hexLen; i += 2)
            {
                ret[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return ret;
        }
        public static void WriteCookiesToDisk(string file, CookieContainer cookieJar)
        {
	        using (Stream stream = File.Create(file))
	        {
		        try
		        {
			        Console.Out.Write("Writing cookies to disk... ");
			        BinaryFormatter formatter = new BinaryFormatter();
			        formatter.Serialize(stream, cookieJar);
			        Console.Out.WriteLine("Done.");
		        }
		        catch (Exception e)
		        {
			        Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
		        }
	        }
        }

        public static CookieContainer ReadCookiesFromDisk(string file)
        {
	        try
	        {
		        using (Stream stream = File.Open(file, FileMode.Open))
		        {
			        Console.Out.Write("Reading cookies from disk... ");
			        BinaryFormatter formatter = new BinaryFormatter();
			        Console.Out.WriteLine("Done.");
			        return (CookieContainer)formatter.Deserialize(stream);
		        }
	        }
	        catch (Exception e)
	        {
		        Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
		        return new CookieContainer();
	        }
        }
}
}
