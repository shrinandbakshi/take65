using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;

using System.Security.Cryptography;
using System.Runtime.Serialization;

using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Bll
{
    public class Util
    {
        public static String ImagePath = ConfigurationManager.AppSettings["Content.ImageFolder"];

        /// <summary>
        /// Encode text to MD5 (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String EncodeMD5(String text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                //Declarations
                Byte[] originalBytes;
                Byte[] encodedBytes;
                MD5 md5;

                //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
                md5 = new MD5CryptoServiceProvider();
                originalBytes = ASCIIEncoding.Default.GetBytes(text);
                encodedBytes = md5.ComputeHash(originalBytes);

                //Convert encoded bytes back to a 'readable' string
                return BitConverter.ToString(encodedBytes).Replace("-", "");
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Return relative time from a datetime (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String RelativeTime(DateTime date)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            if (delta < 0)
            {
                return "Not yet";
            }
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "One second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                return "A minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                return "An hour ago";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                return "Yesterday";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "One month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "One year ago" : years + " years ago";
            }
        }

        /// <summary>
        /// Generate a random string (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String RandomString(int length)
        {
            Random random = new Random();
            String chars = "abcdefghijklmnopqrstuvwxyz";

            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }

            return new String(buffer);
        }

        /// <summary>
        /// Send mail (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="pToEmail"></param>
        /// <param name="pSubject"></param>
        /// <param name="pFileTemplate"></param>
        /// <param name="pFields"></param>
        public static void SendEmail(string pDisplayNameFrom, string pToEmail, string pSubject, string pFileTemplate, NameValueCollection pFields)
        {
            string sBody = string.Empty;
            using (StreamReader oReader = File.OpenText((HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["Email.Path"] + pFileTemplate)))
            {
                sBody = oReader.ReadToEnd();
                oReader.Close();
            }

            //pFields.Add("HTTP_HOST", HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);

            foreach (string fieldName in pFields)
                sBody = sBody.Replace("#" + fieldName + "#", pFields[fieldName]);

            System.Net.Mail.MailMessage oMessage = new System.Net.Mail.MailMessage();
            oMessage.IsBodyHtml = true;
            oMessage.To.Add(pToEmail);
            oMessage.Subject = HttpUtility.HtmlDecode(pSubject);
            oMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["Email.From"], ((string.IsNullOrEmpty(pDisplayNameFrom) ? ConfigurationManager.AppSettings["Email.From"] : pDisplayNameFrom)));
            oMessage.Body = sBody;

            System.Net.Mail.SmtpClient oSmtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["Application.Smtp.Server"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.EnableSsl"]))
                oSmtp.EnableSsl = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["Application.Smtp.EnableSsl"]));

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.Port"]))
                oSmtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Application.Smtp.Port"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.Username"]))
                oSmtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Application.Smtp.Username"], ConfigurationManager.AppSettings["Application.Smtp.Password"]);

            oSmtp.Send(oMessage);
        }

        #region SendEmailWithBody
        /// <summary>
        /// Send mail when subject and body supplied (Support questions).
        /// </summary>
        /// <param name="strMailTo"></param>
        /// <param name="strSubject"></param>
        /// <param name="pFileTemplate"></param>
        /// <param name="pFields"></param>
        public static void SendEmailWithBody(string strMailTo, string strSubject, string pFileTemplate, NameValueCollection pFields)
        {
            string sBody = string.Empty;
            using (StreamReader oReader = File.OpenText((HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["Email.Path"] + pFileTemplate)))
            {
                sBody = oReader.ReadToEnd();
                oReader.Close();
            }
            
            foreach (string fieldName in pFields)
                sBody = sBody.Replace("#" + fieldName + "#", pFields[fieldName]);

            System.Net.Mail.MailMessage oMessage = new System.Net.Mail.MailMessage();
            oMessage.IsBodyHtml = true;
            oMessage.To.Add(string.IsNullOrEmpty(strMailTo) ? ConfigurationManager.AppSettings["Email.SupportInfo"] : strMailTo);
            
            oMessage.Subject = HttpUtility.HtmlDecode(strSubject);
            oMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["Email.From"], ConfigurationManager.AppSettings["Email.From"]);
            oMessage.Body = sBody;

            System.Net.Mail.SmtpClient oSmtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["Application.Smtp.Server"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.EnableSsl"]))
                oSmtp.EnableSsl = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["Application.Smtp.EnableSsl"]));

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.Port"]))
                oSmtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Application.Smtp.Port"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Smtp.Username"]))
                oSmtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Application.Smtp.Username"], ConfigurationManager.AppSettings["Application.Smtp.Password"]);

            oSmtp.Send(oMessage);
        }
        #endregion

        /// <summary>
        /// Return a valid URL (wagner.leonardi@netbiis.com)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String ReturnFullUrl(String url)
        {
            if (url != null && url.Length > 5)
            {
                if (url.Substring(0, 4).ToLower() != "http")
                {
                    return "http://" + url;
                }
                else
                {
                    return url;
                }
            }
            else
            {
                return "#";
            }
        }

        #region Mobile

        //private static readonly Regex MOBILE_REGEX = new Regex(ConfigurationManager.AppSettings.Get("Device.Mobile"), RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Verifica se o usuario utiliza um dispositivo movel
        /// </summary>
        public static bool IsMobile
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    if (request.Browser.IsMobileDevice)
                        return true;

                    //if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Verifica se usuario foi redirecionado de um mobile
        /// </summary>
        public static bool IsMobileRedirect
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (!String.IsNullOrEmpty(Convert.ToString(context.Session["Redirect"])))
                    return false;

                if (!String.IsNullOrEmpty(Convert.ToString(context.Request.QueryString["Redirect"])))
                {
                    context.Session["Redirect"] = Convert.ToString(context.Request.QueryString["Redirect"]);
                    return false;
                }
                return true;
            }
        }

        #endregion

        public static string RemoveHTMLTag(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            else
            {
                return Regex.Replace(text, "<.*?>", string.Empty);
            }
        }

        public static string ShowPreviewContent(string pContent, int pPreviewChar)
        {
            pContent = Regex.Replace(pContent, "<.*?>", string.Empty);
            if (pContent.Length > pPreviewChar)
            {
                int endPosition = pContent.IndexOf(' ', pPreviewChar);
                if (endPosition > 2)
                    pContent = pContent.Substring(0, endPosition) + "...";
                else
                {
                    pContent = pContent.Substring(0, pPreviewChar) + "...";
                }
            }

            return pContent;
        }

        public static String EnumToDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }


        public static Image ScaleImage(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH > nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format16bppRgb565);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


    }
}
