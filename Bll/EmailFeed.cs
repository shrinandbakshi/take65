using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Bll
{
    public class EmailFeed
    {
        public static XmlDocument LoadGmailFeed(string pUsername, string pPassword)
        {
            ServicePointManager.CertificatePolicy = new IgnoreBadCerts();

            NetworkCredential cred = new NetworkCredential();
            cred.UserName = pUsername;
            cred.Password = pPassword;

            WebRequest req = WebRequest.Create("https://mail.google.com/mail/feed/atom/all");
            req.Credentials = cred;
            Stream resp = req.GetResponse().GetResponseStream();

            

            XmlReader reader = XmlReader.Create(resp);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlDocument docClean = new XmlDocument();
            string Xml = doc.InnerXml;
            Xml = Xml.Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
            docClean.LoadXml(Xml);

            return docClean;
        }

        public static IEnumerable<Model.REST.EmailFeed> LoadOutlookFeed(string username, string password)
        {
            return EmailFeed.GenericPop3Feed("pop3.live.com", 995, true, "http://outlook.com", username, password);
        }

        public static IEnumerable<Model.REST.EmailFeed> LoadYahooFeed(string username, string password)
        {
            return EmailFeed.GenericPop3Feed("pop.mail.yahoo.com", 995, true, "http://mail.yahoo.com", username, password);
        }

        private static IEnumerable<Model.REST.EmailFeed> GenericPop3Feed(string hostname, int port, bool enableSsl, string redirectLink, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                try
                {
                    // Connect to the server
                    client.Connect(hostname, port, enableSsl);

                    // Authenticate ourselves towards the server
                    client.Authenticate(username, password);

                    int messageCount = client.GetMessageCount();

                    if (messageCount > 20)
                        messageCount = 20;

                    // We want to download all messages
                    List<Model.REST.EmailFeed> messageHeaders = new List<Model.REST.EmailFeed>(messageCount);

                    // We want to check the headers of the message before we download
                    // the full message
                    MessageHeader headers = client.GetMessageHeaders(messageCount);

                    for (int i = messageCount; i > 0; i--)
                    {
                        var header = client.GetMessageHeaders(i);

                        messageHeaders.Add(new Model.REST.EmailFeed
                        {
                            // id = n.SelectSingleNode("./id").InnerText,
                            link = redirectLink, // n.SelectSingleNode("./link").Attributes["href"].Value,
                            title = header.Subject,
                            lastmodified = header.DateSent,
                            lastmodifiedLabel = Bll.Util.RelativeTime(header.DateSent),
                            fromName = header.From.DisplayName ?? header.From.MailAddress.DisplayName,
                            fromEmail = header.From.Address ?? header.From.MailAddress.Address
                        });
                    }

                    // client.GetMessageHeaders(i)

                    return messageHeaders;
                }
                catch (OpenPop.Pop3.Exceptions.InvalidLoginException)
                {
                    throw new Exception("Invalid user or password");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private class IgnoreBadCerts : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint sp,
                                               X509Certificate certificate,
                                               WebRequest request,
                                               int error)
            {
                return true;
            }


        }
    }
}
