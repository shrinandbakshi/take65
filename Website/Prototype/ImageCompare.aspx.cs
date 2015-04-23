//using Microsoft.Test.VisualVerification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Website.Prototype
{
    public partial class ImageCompare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int Page = 0;
            if (!string.IsNullOrEmpty(Request["Page"]))
            {
                Page = Convert.ToInt32(Request["Page"]);
            }

            string html;
            WebClient webClient = new WebClient();
            using (Stream stream = webClient.OpenRead(new Uri("http://www.alexa.com/topsites/countries;" + Page.ToString() + "/US")))
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            int divLocation = html.IndexOf("<div class=\"module\" id=\"topsites-countries\">");
            int ulLocation = html.IndexOf("<ul>", divLocation);


                Bll.SafeWebsite bllSafeWs = new Bll.SafeWebsite();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(html.Substring(ulLocation, html.IndexOf("</ul>", ulLocation) - ulLocation + 5).Replace("&", "&amp;"));
            foreach (XmlNode node in xmlDoc.SelectNodes("//li"))
            {
                XmlNode nodeUrl = node.SelectSingleNode("./div/h2/a");
                
                bllSafeWs.Save(new Model.SafeWebsite
                {
                    Url = nodeUrl.InnerText.Trim(),
                    OpenIFrame = false
                });

                bllSafeWs.Save(new Model.SafeWebsite
                {
                    Url = "www." + nodeUrl.InnerText.Trim(),
                    OpenIFrame = false
                });
            }
            Page++;
            Response.Redirect("ImageCompare.aspx?Page=" + Page.ToString());
            
        }
    }
}