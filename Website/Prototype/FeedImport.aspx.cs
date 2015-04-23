using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Configuration;
using Microsoft.Test.VisualVerification;

namespace Website.Prototype
{
    public partial class FeedImport : System.Web.UI.Page
    {
        /*
        protected Bll.FeedContent bllFeedConcent;
        protected Bll.TrustedSource bllTrustedSouce;

        protected void Page_Load(object sender, EventArgs e)
        {
            //XmlReader reader = XmlReader.Create("http://feeds.abcnews.com/abcnews/entertainmentheadlines");

            bllTrustedSouce = new Bll.TrustedSource();
            bllFeedConcent = new Bll.FeedContent();

            ImportFeed();
            //SaveImageFromUrl();

            Response.Write("IMPORTED: " + DateTime.Now.ToString());
            
        }

        private void ImportFeed()
        {
            Model.TrustedSource[] LtSources = bllTrustedSouce.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, string.Empty);

            foreach (Model.TrustedSource Source in LtSources)
            {
                Model.TrustedSourceFeed[] LtFeed = bllTrustedSouce.GetTrustedSourceFeedList(Source.Id);

                if (LtFeed != null)
                {
                    foreach (Model.TrustedSourceFeed FeedSource in LtFeed)
                    {
                        try
                        {
                            XmlReader reader = XmlReader.Create(FeedSource.Url);
                        
                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            long TrustedSourceId = FeedSource.TrustedSourceId;



                            foreach (SyndicationItem f in feed.Items)
                            {
                                Model.FeedContent NewContent = new Model.FeedContent();
                                NewContent.Images = new List<Model.FeedContentImage>();

                                NewContent.TrustedSourceFeedId = FeedSource.Id;
                                NewContent.Title = f.Title.Text;
                                NewContent.Description = f.Summary.Text;
                                NewContent.PublishedDate = f.PublishDate.DateTime;
                                NewContent.LastModified = f.LastUpdatedTime.DateTime;
                                //NewContent.FeedGuid = f.

                                if (f.Links.Count > 0)
                                {
                                    NewContent.Link = f.Links.First().Uri.AbsoluteUri;
                                }

                                foreach (SyndicationElementExtension extension in f.ElementExtensions)
                                {

                                    XElement element = extension.GetObject<XElement>();

                                    if (element.HasAttributes)
                                    {
                                        foreach (var attribute in element.Attributes())
                                        {
                                            string value = attribute.Value.ToLower();
                                            if (value.StartsWith("http://") && (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".gif")))
                                            {
                                                NewContent.Images.Add(new Model.FeedContentImage
                                                {
                                                    Url = value
                                                });
                                                //rssItem.Add(value); // Add here the image link to some array
                                            }
                                        }
                                    }
                                }

                                if (NewContent.Images.Count > 0)
                                {
                                    NewContent.Thumb = DownloadImage(NewContent.Images[0].Url);

                                }
                                //SyndicationElementExtension a = oItem.ElementExtensions[0];
                                //string a = oItem.AttributeExtensions
                                //string sImage = Regex.Match(oItem.Summary.Text, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                Model.FeedContent ContentSaved = bllFeedConcent.Save(NewContent);
                                if (ContentSaved.Id == 0)
                                {
                                    if (!string.IsNullOrEmpty(ContentSaved.Thumb))
                                    {
                                        string filePath = Server.MapPath("..\\" + ConfigurationManager.AppSettings["Content.ImageFolder"]) + ContentSaved.Thumb;
                                        File.Delete(filePath);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            //Next
                        }
                    }
                }
            }
        }

        private string DownloadImage(string pUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    string[] urlFile = pUrl.Split(new string[] { "." }, StringSplitOptions.None);
                    string fileName = Guid.NewGuid().ToString() + "." + urlFile[urlFile.Length - 1];
                    string filePath = Server.MapPath("..\\" + ConfigurationManager.AppSettings["Content.ImageFolder"]) + fileName;
                    // if the remote file was found, download oit
                    using (Stream inputStream = response.GetResponseStream())
                    {
                        System.Drawing.Image ImgToSave = System.Drawing.Image.FromStream(inputStream);
                        ImgToSave.Save(filePath);
                    }

                    
                    Snapshot defaultImage = Snapshot.FromFile(Server.MapPath("abc-default-image.jpg"));
                    Snapshot imageToSave = Snapshot.FromFile(filePath);
                    try
                    {
                        Snapshot difference = defaultImage.CompareTo(imageToSave);
                        File.Delete(filePath);
                        return "";
                    }
                    catch
                    {
                        return fileName; 
                    }

                    return null;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404") || ex.Message.Contains("400"))
                {
                    return string.Empty;
                }
                else
                    throw ex;
            }
        }

        private void SaveImageFromUrl()
        {
            Model.FeedContents ContentsToSync = bllFeedConcent.GetContentToSync();

            
            foreach (Model.FeedContent ContentImg in ContentsToSync.FeedContentList)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ContentImg.Thumb);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // Check that the remote file was found. The ContentType
                    // check is performed since a request for a non-existent
                    // image file might be redirected to a 404-page, which would
                    // yield the StatusCode "OK", even though the image was not
                    // found.
                    if ((response.StatusCode == HttpStatusCode.OK ||
                        response.StatusCode == HttpStatusCode.Moved ||
                        response.StatusCode == HttpStatusCode.Redirect) &&
                        response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] urlFile = ContentImg.Thumb.Split(new string[] { "." }, StringSplitOptions.None);
                        string fileName = Guid.NewGuid().ToString() + "." + urlFile[urlFile.Length - 1];
                        string filePath = Server.MapPath("..\\" + ConfigurationManager.AppSettings["Content.ImageFolder"]) + fileName;
                        // if the remote file was found, download oit
                        using (Stream inputStream = response.GetResponseStream())
                        {
                            System.Drawing.Image ImgToSave = System.Drawing.Image.FromStream(inputStream);
                            ImgToSave.Save(filePath);
                        }

                        ContentImg.Thumb = fileName;
                        bllFeedConcent.Save(ContentImg);
                    }
                    else
                    {
                        ContentImg.Thumb = string.Empty;
                        bllFeedConcent.Save(ContentImg);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("404") || ex.Message.Contains("400"))
                    {
                        ContentImg.Thumb = string.Empty;
                        bllFeedConcent.Save(ContentImg);
                    }
                    else
                        throw ex;
                }

            }
        }
         */
    }
}