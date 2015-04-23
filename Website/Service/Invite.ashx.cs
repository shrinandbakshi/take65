using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Website.Service
{
    /// <summary>
    /// Summary description for Invite
    /// </summary>
    public class Invite : REST
    {

        /// <summary>
        /// Return a list of trusted sources , by type and category (wagner.leonardi@netbiis.com)
        /// </summary>
        protected override void Get()
        {
            throw new NotImplementedException();
        }

        protected async override void Get(string parameter)
        {
            //Get parameters
            String service = "";
            String user = "";
            String password = "";

            try
            {
                service = this.GetParameterValue("p1");
            }
            catch { }

            try
            {
                user = this.GetParameterValue("p2");
            }
            catch { }

            try
            {
                password = this.GetParameterValue("p3");
            }
            catch { }

            try
            {
                List<Model.Contact> contactList = default(List<Model.Contact>);

                switch (service)
                {
                    case "Gmail":
                        Bll.Invite.Google bllInviteGoogle = new Bll.Invite.Google(user, password);
                        contactList = bllInviteGoogle.GetContact();
                        break;
                    case "Hotmail":
                        Bll.Invite.Outlook bllInviteOutlook = new Bll.Invite.Outlook(user, password);
                        contactList = await bllInviteOutlook.GetContact();
                        break;
                }

                this.Response<Model.REST.Contact>(this.ModelListToRESTModelList(contactList));
            }
            catch (Exception e)
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "Error: " + e.Message
                });
            }

        }

        
        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post(string parameter)
        {
            throw new NotImplementedException();
        }

        protected override void Post()
        {
            Model.User user = this.GetSessionUser();

            if (user != null)
            {
                List<Model.REST.Contact> contactList = this.ReadJsonRequest<List<Model.REST.Contact>>();

                NameValueCollection sendmailParameters = null;

                try
                {
                    foreach (Model.REST.Contact contact in contactList)
                    {
                        string Subject = "Your Friend invited you to join Take 65";
                        
                        try
                        {
                            sendmailParameters = new NameValueCollection();
                            sendmailParameters.Add("name", !String.IsNullOrEmpty(user.Name) ? user.Name : "Your Friend");
                            sendmailParameters.Add("email", user.Email);
                            Subject = sendmailParameters["name"] + " invited you to join Take 65";
                        }
                        catch { }

                        Bll.Util.SendEmail(user.Name, contact.email, Subject, "Invite.html", sendmailParameters);
                    }

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Done"
                    });
                }
                catch (Exception e)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Error: " + e.Message
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

        private List<Model.REST.Contact> ModelListToRESTModelList(List<Model.Contact> modelList)
        {
            List<Model.REST.Contact> restModelList = new List<Model.REST.Contact>();

            if (modelList != null)
            {
                for (int i = 0; i < modelList.Count; i++)
                {
                    restModelList.Add(this.ModelToRESTModel(modelList[i]));
                }
            }

            return restModelList;
        }

        private Model.REST.Contact ModelToRESTModel(Model.Contact model)
        {
            Model.REST.Contact restModel = new Model.REST.Contact();
            restModel.name = model.Name ;
            restModel.email = model.Email;
            restModel.image = model.Image;

            return restModel;
        }


    }
}