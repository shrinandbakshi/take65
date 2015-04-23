using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Invite
{
    public class Outlook : IInvite
    {
        private String user = "";
        private String pass = "";

        public Outlook(String user, String pass)
        {
            this.user = user;
            this.pass = pass;
        }

        private LiveConnectSession _session;
        public LiveConnectSession Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        List<Model.Contact> IInvite.GetContact()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Model.Contact>> GetContact()
        {
            try
            {
                LiveAuthClient auth = new LiveAuthClient(ConfigurationManager.AppSettings["Live.CLIENT_ID"]);
                LiveLoginResult initializeResult = await auth.InitializeAsync();
                try
                {
                    LiveLoginResult loginResult = await auth.InitializeAsync(new string[] { "wl.signin", "wl.basic" });
                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        LiveConnectClient connect = new LiveConnectClient(auth.Session);
                        LiveOperationResult operationResult = await connect.GetAsync("me/contacts");
                        dynamic result = operationResult.Result;

                        //List<object> data = null;
                        //IDictionary<string, object> contact = null;
                        //List<object> emailHashes = null;
                        //if (result.ContainsKey("data"))
                        //{
                        //    data = (List<object>)result["data"];
                        //    for (int i = 0; i < data.Count; i++)
                        //    {
                        //        contact = (IDictionary<string, object>)data[i];
                        //        if (contact.ContainsKey("email_hashes"))
                        //        {
                        //            emailHashes = (List<object>)contact["email_hashes"];
                        //            for (int j = 0; j < emailHashes.Count; j++)
                        //            {
                        //                message += emailHashes[j].ToString() + "\r\n";
                        //            }
                        //        }
                        //    }
                        //}


                        List<Model.Contact> contactList = new List<Model.Contact>();
                        
                        foreach (var e in result)
                        {
                            if (e.PrimaryEmail != null)
                            {

                                contactList.Add(new Model.Contact()
                                {
                                    Name = !string.IsNullOrEmpty(e.Title) ? e.Title : e.PrimaryEmail.Address,
                                    Email = e.PrimaryEmail.Address,
                                });

                            }
                        }

                        return contactList;
                    }
                }
                catch (LiveAuthException ex)
                {
                }
                catch (LiveConnectException ex)
                {
                }
            }
            catch (LiveAuthException ex)
            {
            }

            return null;
        }
    }
}
