using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Net;
using Microsoft.Test.VisualVerification;

namespace Bll
{
    public class FeedContentImport
    {
        public void LoadContent()
        {
            String Log = "";

            Bll.TrustedSource bllTrustedSouce = new Bll.TrustedSource();
            Bll.FeedContent bllFeedConcent = new Bll.FeedContent();


            Model.TrustedSource[] LtSources = bllTrustedSouce.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED,0);

            Log += "<br>Sources: " + LtSources.Length;

            foreach (Model.TrustedSource Source in LtSources)
            {
                Model.TrustedSourceFeed[] LtFeed = bllTrustedSouce.GetTrustedSourceFeedList(Source.Id);

                if (LtFeed != null)
                {
                    Log += "<br>        Feeds: " + LtFeed.Length;
                    foreach (Model.TrustedSourceFeed FeedSource in LtFeed)
                    {
                        Log += "<br>        One more time";
                        try
                        {
                            XmlReader reader = XmlReader.Create(FeedSource.Url);

                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            long TrustedSourceId = FeedSource.TrustedSourceId;



                            foreach (SyndicationItem f in feed.Items)
                            {
                                Model.FeedContent NewContent = new Model.FeedContent();
                                NewContent.ImageList = new List<Model.FeedContentImage>();

                                NewContent.TrustedSourceFeedId = FeedSource.Id;
                                NewContent.Title = f.Title.Text;
                                NewContent.Description = Bll.Util.RemoveHTMLTag(f.Summary.Text);
                                NewContent.PublishedDate = f.PublishDate.DateTime;
                                NewContent.LastModified = f.LastUpdatedTime.DateTime;

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
                                                NewContent.ImageList.Add(new Model.FeedContentImage
                                                {
                                                    Url = value
                                                });
                                            }
                                        }
                                    }
                                }


                                if (NewContent.ImageList.Count > 0)
                                {
                                    Log += "<br>                Imagem A: " + NewContent.ImageList[0].Url;
                                    NewContent.Thumb = DownloadImage(NewContent.ImageList[0].Url);
                                    Log += "<br>                Imagem B: " + NewContent.Thumb;
                                }

                                //SyndicationElementExtension a = oItem.ElementExtensions[0];
                                //string a = oItem.AttributeExtensions
                                //string sImage = Regex.Match(oItem.Summary.Text, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                Model.FeedContent ContentSaved = bllFeedConcent.Save(NewContent);

                                if (ContentSaved.Id == 0)
                                {
                                    if (!string.IsNullOrEmpty(ContentSaved.Thumb))
                                    {
                                        string filePath = ConfigurationManager.AppSettings["Content.ImageFolder"] + ContentSaved.Thumb;
                                        File.Delete(filePath);
                                    }
                                }

                                //Generate search tags from content
                                /*
                                Bll.Tag bllTag = new Tag();
                                List<Model.Tag> listTag = bllTag.TextToTag(NewContent.Title,0);
                                listTag.AddRange(bllTag.TextToTag(NewContent.Description,0));

                                bllFeedConcent.SaveFeedContentTag(ContentSaved.Id, listTag);
                                 */
                            }
                        }
                        catch(Exception e)
                        {
                           // throw e;
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
                    string filePath = ConfigurationManager.AppSettings["Content.ImageFolder"] + "\\" + fileName;
                    // if the remote file was found, download it
                    using (Stream inputStream = response.GetResponseStream())
                    {
                        System.Drawing.Image ImgToSave = System.Drawing.Image.FromStream(inputStream);
                        ImgToSave.Save(filePath);
                    }

  
                    Snapshot defaultImage = Snapshot.FromFile(ConfigurationManager.AppSettings["Content.ImageFolder.abcimage"]);
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
    }
}
