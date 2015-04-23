using ImapX;
using ImapX.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Website.Service
{
    /// <summary>
    /// Summary description for Gmail
    /// </summary>
    public class Gmail : REST
    {
        protected override void Get()
        {
            var token = GetParameterValue("token");
            var email = GetParameterValue("email");

            var client = new ImapClient("imap.gmail.com", true);
            client.Behavior.AutoDownloadBodyOnAccess = false;
            client.Behavior.AutoPopulateFolderMessages = false;
            client.Behavior.ExamineFolders = false;
            client.Behavior.SearchAllNotSupported = false;
            client.Behavior.MessageFetchMode = ImapX.Enums.MessageFetchMode.GMailMessageId;
            client.Behavior.FolderTreeBrowseMode = ImapX.Enums.FolderTreeBrowseMode.Lazy;

            if (client.Connect())
            {
                var credentials = new OAuth2Credentials(email, token);

                string lastweek = DateTime.Now.AddDays(-3).ToString("dd-MMM-yyyy");
                string query = String.Concat("SINCE ", lastweek);

                if (client.Login(credentials))
                {
                    Folder folder = client.Folders.Inbox;
                    folder.Messages.Download(query: query, mode: ImapX.Enums.MessageFetchMode.Headers);

                    DateTime outDate;
                    Regex regexEmail = new Regex("\\<(.*)\\>"),
                        regexName = new Regex(@"(?<="")[^\""]*(?="")");

                    var messages = client.Folders.Inbox.Messages
                        .OrderByDescending(t => t.Date).Select(t => new
                        {
                            lastmodifiedLabel = DateTime.TryParse(t.Headers["date"], out outDate) ? Bll.Util.RelativeTime(outDate) : "",
                            title = t.Subject,
                            fromEmail = regexEmail.Match(t.Headers["from"]).Groups[1].ToString(),
                            fromName = regexName.Match(t.Headers["from"]).Value
                        });

                    this.Response<List<object>>(messages);
                }
            }
            else
            {
                this.Response<object>(null);
            }
        }

        protected override void Get(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post()
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }
    }
}