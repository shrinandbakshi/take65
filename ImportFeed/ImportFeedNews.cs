using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using System.Net;

using Microsoft.Test.VisualVerification;

namespace ImportFeed
{
    public partial class ImportFeedNews : ServiceBase
    {
        StreamWriter fileLog = default(StreamWriter);
        System.Timers.Timer oTimerXml = default(System.Timers.Timer);

        public ImportFeedNews()
        {
            this.ServiceName = "Take65 - Import Feed News";
            InitializeComponent();
        }

        /// <summary>
        /// Metodo iniciado junto com o servico
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            //Cria arquivo de log
            fileLog = new StreamWriter(ConfigurationManager.AppSettings["Log.Path"].ToString(), true);
            fileLog.WriteLine(" ");
            fileLog.WriteLine("::: SERVICE STARTING: " + String.Format("{0:dd/MMM/yyyy HH:mm:ss}", DateTime.Now) + " :::");
            fileLog.Flush();

            //Faz Download Xml
            oTimerXml = new System.Timers.Timer();
            oTimerXml.Elapsed += new ElapsedEventHandler(OnTimedEventXml_Elapsed);
            //oTimerXml.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Timer.Interval.Xml"].ToString());
            oTimerXml.Interval = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["Timer.Interval.Xml"].ToString())).TotalMilliseconds;
            oTimerXml.Enabled = true;
        }


        /// <summary>
        /// Save Feed no Banco de dados
        /// Metodo chamado conforme no intervalo de tempo estipulado na variavel interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTimedEventXml_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Log.Save"]))
            {
                fileLog.WriteLine(" ");
                fileLog.WriteLine("Processing Importing News - Begin: " + DateTime.Now);
                fileLog.Flush();
            }

            LoadFeeds();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Log.Save"]))
            {
                fileLog.WriteLine("Processing Importing News - End: " + DateTime.Now);
                fileLog.Flush();
            }
        }

        private void LoadFeeds()
        {
            Bll.TrustedSource bllTrustedSouce = new Bll.TrustedSource();
            Bll.FeedContent bllFeedConcent = new Bll.FeedContent();


            Model.TrustedSource[] LtSources = bllTrustedSouce.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, 0);

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
                                NewContent.ImageList = new List<Model.FeedContentImage>();

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
                                    NewContent.Thumb = DownloadImage(NewContent.ImageList[0].Url);
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
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Log.Save"]))
                            {
                                fileLog.WriteLine(" ");
                                fileLog.WriteLine("Error: " + ex.Message);
                                fileLog.Flush();
                            }
                        }
                    }
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Log.Save"]))
            {
                fileLog.WriteLine(" ");
                fileLog.WriteLine("Processing Importing News - Generating Tags - Begin: " + DateTime.Now);
                fileLog.Flush();
            }

            bllFeedConcent.GenerateContentTag();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Log.Save"]))
            {
                fileLog.WriteLine(" ");
                fileLog.WriteLine("Processing Importing News - Generating Tags - Begin: " + DateTime.Now);
                fileLog.Flush();
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
                    string filePath = ConfigurationManager.AppSettings["Content.ImageFolder"] + fileName;
                    // if the remote file was found, download oit
                    using (Stream inputStream = response.GetResponseStream())
                    {
                        System.Drawing.Image ImgToSave = System.Drawing.Image.FromStream(inputStream);
                        ImgToSave = Bll.Util.ScaleImage(ImgToSave, Convert.ToInt32(ConfigurationManager.AppSettings["Content.ThumbSize.Width"]), Convert.ToInt32(ConfigurationManager.AppSettings["Content.ThumbSize.Height"]));
                        ImgToSave.Save(filePath);
                    }

                    Snapshot defaultImage = Snapshot.FromFile(ConfigurationManager.AppSettings["Content.ImageFolder.abcimage"]);
                    Snapshot imageToSave = Snapshot.FromFile(filePath);


                   
                    try
                    {
                        SnapshotVerifier verifier = new SnapshotToleranceMapVerifier(defaultImage);
                        VerificationResult enResult = verifier.Verify(imageToSave);

                        if (enResult == VerificationResult.Pass)
                        {
                            File.Delete(filePath);
                            return "";
                        }
                        else
                        {
                            return fileName; 
                        }

                        //Snapshot difference = defaultImage.CompareTo(imageToSave);
                        
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


        /// <summary>
        /// Metodo chamado quando o servido é parado
        /// </summary>
        protected override void OnStop()
        {
            fileLog.WriteLine(" ");
            fileLog.WriteLine("::: SERVICE STOPING: " + DateTime.Now + " :::");
            fileLog.Close();
        }

        public void Debug()
        {
            fileLog = new StreamWriter(ConfigurationManager.AppSettings["Log.Path"].ToString(), true);
            fileLog.WriteLine(" ");
            fileLog.WriteLine("::: DEBUG MODE: " + String.Format("{0:dd/MMM/yyyy HH:mm:ss}", DateTime.Now) + " :::");

            OnTimedEventXml_Elapsed(null, null);
            //LoadFeeds();
        }
    }
}
