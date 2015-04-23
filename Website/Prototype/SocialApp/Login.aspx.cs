using Facebook.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.Prototype.SocialApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLoginFacebook.Click += new EventHandler(lnkLoginFacebook_Click);
        }

        /// <summary>
        /// User authenticates and redirects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkLoginFacebook_Click(object sender, EventArgs e)
        {
            try
            {
                FacebookWebClient fb;
                try
                {

                    fb = new FacebookWebClient(FacebookWebContext.Current);
                    if (string.IsNullOrEmpty(fb.AccessToken))
                    {
                        fb.AccessToken = txtFbToken.Text;
                    }
                }
                catch
                {
                    fb = new FacebookWebClient(txtFbToken.Text);
                }

                dynamic result = fb.Get("/me/friends?limit=5000");
                JObject friendListJson = JObject.Parse(result.ToString());

                
                foreach (var friend in friendListJson["data"].Children())
                {

                    
                    Response.Write(friend["id"].ToString().Replace("\"", ""));
                    Response.Write(friend["name"].ToString().Replace("\"", ""));
                    //Response.Write(friend["email"].ToString().Replace("\"", ""));
                    Response.Write("<br/>");
                }

                dynamic albums = fb.Get("/20701952/albums");
                JObject albumnsListJson = JObject.Parse(albums.ToString());

                dynamic photos = fb.Get("/722825150648/photos");
                JObject photosListJson = JObject.Parse(photos.ToString());

                dynamic test = fb.Get("/20719096/photos");
                JObject testJson = JObject.Parse(test.ToString());

                //string encTicket = oAccount.LoginByFb(result.id);
                /*
                if (!string.IsNullOrEmpty(encTicket))
                {
                    LoginAccount(encTicket, fb.AccessToken);
                    if (oAccount.oAccount.Status != (Int16)Model.enAccountStatus.ACTIVE)
                    {
                        if (Bll.Settings.Instance.Store != null)
                        {
                            if ((Model.enSourceType)Bll.Settings.Instance.Store.SourceTypeId == Model.enSourceType.Etsy)
                            {
                                Bll.Store oStore = new Bll.Store();
                                Model.CompanyCode oCode = oStore.GetCompanyCode(ConfigurationManager.AppSettings["Landing.Etsy.Code"]);
                                if (oCode != null)
                                {
                                    Session["Account.CompanyCode"] = ConfigurationManager.AppSettings["Landing.Etsy.Code"];
                                    Session["Account.CompanyCodeObject"] = oCode;
                                }
                            }
                        }
                        Response.Redirect("Register-Step-1", true);
                    }
                    else
                    {
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(encTicket, true));
                    }
                }
                else
                {
                    lblFeedbackFacebook.Visible = true;
                }
                 * */

            }
            catch (Exception Error)
            {
                throw Error;
            }
        }

        


    }
}