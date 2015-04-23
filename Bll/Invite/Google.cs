using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Contacts;
using Google.GData.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;

namespace Bll.Invite
{
    public class Google : IInvite
    {
        private String user = "";
        private String pass = "";

        public Google(String user, String pass)
        {
            this.user = user;
            this.pass = pass;
        }

        public List<Model.Contact> GetContact()
        {
            RequestSettings settings = new RequestSettings("<var>Take65</var>", this.user, this.pass);
            settings.PageSize = 300;
            ContactsRequest contactsRequest = new ContactsRequest(settings);
            

            // Get the feed
            Feed<Contact> feed = contactsRequest.GetContacts();

            List<Model.Contact> contactList = new List<Model.Contact>();

            Feed<Contact> f = contactsRequest.GetContacts();
            foreach (Contact e in f.Entries)
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
}
