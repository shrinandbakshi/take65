using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Website.Service
{
    /// <summary>
    /// Summary description for UserWidgetEmail
    /// </summary>
    public class UserWidgetEmail : REST
    {

        protected override void Get(string parameter)
        {
            bool status = true;
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                if (parameter.ToUpper() == "FEED")
                {
                    Bll.UserEmailAccount bllEmailAccount = new Bll.UserEmailAccount();
                    Model.EmailAccount currentAccount = bllEmailAccount.Get(user.Id);
                    if (currentAccount != null)
                    {
                        IEnumerable<Model.REST.EmailFeed> ltFeed = new List<Model.REST.EmailFeed>();
                        try
                        {
                            switch (currentAccount.EmailServer)
                            {
                                case Model.Enum.enEmailServer.GMAIL:
                                    ltFeed = TransformGmailFeed(Bll.EmailFeed.LoadGmailFeed(currentAccount.Username, currentAccount.Password));
                                    break;
                                case Model.Enum.enEmailServer.HOTMAIL:
                                    ltFeed = Bll.EmailFeed.LoadOutlookFeed(currentAccount.Username, currentAccount.Password);
                                    break;
                                case Model.Enum.enEmailServer.YAHOO:
                                    ltFeed = Bll.EmailFeed.LoadYahooFeed(currentAccount.Username, currentAccount.Password);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("(401) Unauthorized"))
                            {
                                status = false;
                            }
                            else
                                throw ex;
                        }

                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = status,                            
                            response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ltFeed)
                        });
                    }
                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }

        protected override void Get()
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                Bll.UserEmailAccount bllEmailAccount = new Bll.UserEmailAccount();
                Model.EmailAccount currentAccount = bllEmailAccount.Get(user.Id);
                if (currentAccount != null)
                {
                    this.Response<Model.REST.EmailAccount>(new Model.REST.EmailAccount
                    {
                        username = currentAccount.Username,
                        serverType = currentAccount.EmailServer.ToString()
                    });

                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }

        protected override void Post(string parameter)
        {
            Model.User user = this.GetSessionUser();
            if (user != null)
            {
                if (parameter.ToUpper() == "SAVE")
                {
                    try
                    {
                        Model.REST.EmailAccount restAccount = this.ReadJsonRequest<Model.REST.EmailAccount>();

                        /* Valide Provider Info */
                        Model.Enum.enEmailServer enServer = (Model.Enum.enEmailServer)Enum.Parse(typeof(Model.Enum.enEmailServer), restAccount.serverType);
                        IEnumerable<Model.REST.EmailFeed> ltFeed = new List<Model.REST.EmailFeed>();
                        switch (enServer)
                        {
                            case Model.Enum.enEmailServer.GMAIL:
                                // ltFeed = TransformGmailFeed(Bll.EmailFeed.LoadGmailFeed(restAccount.username, restAccount.password));
                                break;
                            case Model.Enum.enEmailServer.HOTMAIL:
                                ltFeed = Bll.EmailFeed.LoadOutlookFeed(restAccount.username, restAccount.password);
                                break;
                            case Model.Enum.enEmailServer.YAHOO:
                                ltFeed = Bll.EmailFeed.LoadYahooFeed(restAccount.username, restAccount.password);
                                break;
                        }
                        /* Valide Provider Info */
                        
                        Model.EmailAccount emailAccount = new Model.EmailAccount();
                        emailAccount.Username = restAccount.username;
                        emailAccount.Password = restAccount.password;
                        emailAccount.EmailServer = (Model.Enum.enEmailServer)Enum.Parse(typeof(Model.Enum.enEmailServer), restAccount.serverType);

                        Bll.UserEmailAccount bllEmailAccount = new Bll.UserEmailAccount();
                        bllEmailAccount.Save(user.Id, emailAccount);

                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ltFeed)
                        });

                    }
                    catch (Exception ex)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = ex.Message
                        });
                    }
                }
                else if (parameter.ToUpper() == "DELETE")
                {
                    Bll.UserEmailAccount bllEmailAccount = new Bll.UserEmailAccount();
                    Model.EmailAccount currentAccount = bllEmailAccount.Get(user.Id);
                    if (currentAccount != null)
                    {
                        currentAccount.Deleted = DateTime.Now;
                        bllEmailAccount.Save(user.Id, currentAccount);
                    }

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Account deleted"
                    });
                }
            }
            else
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "User not logged"
                });
            }
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        protected List<Model.REST.EmailFeed> TransformGmailFeed(XmlDocument pFeed)
        {
            
            List<Model.REST.EmailFeed> ltFeed = new List<Model.REST.EmailFeed>();
            foreach(XmlNode n in pFeed.SelectNodes("//entry")){
                ltFeed.Add(new Model.REST.EmailFeed
                {
                    id = n.SelectSingleNode("./id").InnerText,
                    link = n.SelectSingleNode("./link").Attributes["href"].Value,
                    title = n.SelectSingleNode("./title").InnerText,
                    lastmodified = DateTime.Parse(n.SelectSingleNode("./modified").InnerText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                    lastmodifiedLabel = Bll.Util.RelativeTime(DateTime.Parse(n.SelectSingleNode("./modified").InnerText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)),
                    fromName = n.SelectSingleNode("./author/name").InnerText,
                    fromEmail = n.SelectSingleNode("./author/email").InnerText
                });
            }
            return ltFeed;
        }

        private DateTime ParseDate(string W3CDateTimeString)
        {
            const string W3CDateFormat =
              @"^(?<year>\d\d\d\d)" +
              @"(-(?<month>\d\d)(-(?<day>\d\d)
	(T(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" +
              @"(?<tzd>(Z|[+\-]\d\d:\d\d)))?)?)?$";

            Regex regex = new Regex(W3CDateFormat);

            Match match = regex.Match(W3CDateTimeString);
            if (!match.Success)
            {
                // Didn't match either expression. Throw an exception.
                throw new FormatException("String is not a valid date time stamp.");
            }

            int year = int.Parse(match.Groups["year"].Value);
            int month = (match.Groups["month"].Success) ?
                int.Parse(match.Groups["month"].Value) : 1;
            int day = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value) : 1;
            int hour = match.Groups["hour"].Success ? int.Parse(match.Groups["hour"].Value) : 0;
            int min = match.Groups["min"].Success ? int.Parse(match.Groups["min"].Value) : 0;
            int sec = match.Groups["sec"].Success ? int.Parse(match.Groups["sec"].Value) : 0;
            int ms = match.Groups["ms"].Success ?
            (int)Math.Round((1000 * double.Parse(match.Groups["ms"].Value))) : 0;

            //for google mail feed
            if (hour == 24)
                hour = 0;

            TimeSpan tzd = TimeSpan.Zero;
            //if (match.Groups["tzd"].Success)
                //tzd = ParseW3COffset(match.Groups["tzd"].Value);

            DateTime time = new DateTime(year, month, day, hour, min, sec, ms);
            return time;
            //return new W3CDateTime(time, tzd);
        }
    }
}