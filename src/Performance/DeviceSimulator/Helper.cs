using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DeviceSimulator
{
    public static class Helper
    {
        public static string BuildUrl(string @namespace, string eventHub, string publisher)
        {
            var url = string.Format(
                "https://{0}.servicebus.windows.net/{1}/publishers/{2}/messages",
                @namespace,
                eventHub,
                publisher
                );

            return url;
        }

        public static string GenerateToken(string @namespace, string eventHub, string publisher, string sharedAccessKeyName, string sharedAccessKey, DateTimeOffset expiry)
        {
            var unixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var timeSinceEpoch = expiry - unixEpoch;
            var timeSinceEpochInMilliseconds = (long)timeSinceEpoch.TotalMilliseconds;

            var url = Helper.BuildUrl(@namespace, eventHub, publisher);
            var encodedUrl = HttpUtility.UrlEncode(url);
            var signatureContent = string.Format("{0}\n{1}", encodedUrl, timeSinceEpochInMilliseconds);

            var signatureContentAsBytes = Encoding.UTF8.GetBytes(signatureContent);
            var provider = new SHA256CryptoServiceProvider();
            var signatureAsBytes = provider.ComputeHash(signatureContentAsBytes);
            var signature = Convert.ToBase64String(signatureAsBytes);

            var token = string.Format(
                "sr={0}&sig={1}&se={2}&skn={3}",
                HttpUtility.UrlEncode(url),
                HttpUtility.UrlEncode(signature),
                timeSinceEpochInMilliseconds,
                HttpUtility.UrlEncode(sharedAccessKeyName)
                );

            return token;
        }
    }
}
