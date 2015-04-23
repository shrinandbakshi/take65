using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Website.Prototype
{
    public partial class GmailReader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /*
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = new NetworkCredential("henrique.romero@gmail.com", "milenium0429@@");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = resolver;

            XmlReader reader = XmlReader.Create("https://gmail.google.com/gmail/feed/atom", settings);

            */
           

            btnSubmit.Click += btnSubmit_Click;

        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            ServicePointManager.CertificatePolicy = new IgnoreBadCerts();

            NetworkCredential cred = new NetworkCredential();
            cred.UserName = txtUser.Text;
            cred.Password = txtPass.Text;

            try
            {
                WebRequest req = WebRequest.Create("https://mail.google.com/mail/feed/atom");
                req.Credentials = cred;
                Stream resp = req.GetResponse().GetResponseStream();

                XmlReader reader = XmlReader.Create(resp);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
            }
            catch (Exception ex)
            {

                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
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