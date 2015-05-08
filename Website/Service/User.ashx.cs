using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;


using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using System.Threading;
using Website.App_Start;
using Model;




namespace Website.Service
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User : REST
    {
        // Stores token response info such as the access token and refresh token.
        private TokenResponse token;

        // Used to peform API calls against Google+.
        private PlusService ps = null;

        public void Logout()
        {
            this.SetSessionUser(null);
            this.Response<Model.REST.Response>(new Model.REST.Response()
            {
                status = true,
                response = "User Logout"
            });
        }

        protected override void Get(string parameter)
        {
            Bll.User bllUser = new Bll.User();

            if (parameter.ToUpper() == "LOGOUT")
            {
                this.Logout();
            }
            else if (parameter.ToUpper() == "FORGOTPASSWORD")
            {
                Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();

                this.ForgotPassword(restUser.email);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override void Get()
        {

            try
            {
                this.Response<Model.REST.User>(this.ModelToRestModel(this.GetSessionUser()));
            }
            catch
            {
                this.Response<Model.REST.User>(new Model.REST.Response());
            }
        }

        protected override void Delete(string parameter)
        {
            throw new NotImplementedException();
        }

        public void RegisterNewUserEmail()
        {
            Bll.User bllUser = new Bll.User();

            try
            {
                Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                Model.User user = this.RestModelToModel(restUser);
                              
                if (!string.IsNullOrEmpty(user.Email) & !String.IsNullOrEmpty(user.Password))
                {
                    //Usual login by user and password
                    user = bllUser.Get(user.Email, Bll.Util.EncodeMD5(user.Password));

                   
                }
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

       
        public void Login()
        {
            Bll.User bllUser = new Bll.User();

            try
            {
                Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                Model.User user = this.RestModelToModel(restUser);

                if (!String.IsNullOrEmpty(user.FacebookId))
                {
                    //Login through facebook
                    user = bllUser.Get(user.FacebookId);
                    if (user != null)
                    {
                        Bll.FacebookCustom bllFacebook = new Bll.FacebookCustom(Facebook.FacebookApplication.Current.AppId, Facebook.FacebookApplication.Current.AppSecret);
                        bllFacebook.GetTokenLongLive(user.Id, restUser.facebookToken);

                    }
                }
                else if (!string.IsNullOrEmpty(user.GoogleId))
                {
                    //login through google
                    user = bllUser.GetGoogleUser(user.GoogleId);
                    if (user == null)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "You must register your Google account"
                        });
                        return;
                    }
                }
                else if (!string.IsNullOrEmpty(user.Email) & !String.IsNullOrEmpty(user.Password))
                {
                    //Usual login by user and password
                    user = bllUser.Get(user.Email, Bll.Util.EncodeMD5(user.Password));
                }

                if (user == null && !String.IsNullOrEmpty(restUser.facebookId))
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "You must register your Facebook account"
                    });
                }
                else if (user == null || user.Id <= 0)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Invalid user or password"
                    });
                }
                else
                {
                    this.SetSessionUser(user);

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = (user.ChangePasswordNextLogin == DateTime.MinValue) ? "OK" : "Change Password"
                    });
                }


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

        /// <summary>
        /// main
        /// </summary>
        public void RegisterUserEmail()
        {
            Bll.User bllUser = new Bll.User();

            try
            {
                Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                Model.User user = this.RestModelToModel(restUser);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    //Usual login by user and password
                    bllUser.RegisterNewUserEmail(user.Email);
                }
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

        public bool CheckFacebook(string pFacebookId)
        {
            Bll.User bllUser = new Bll.User();

            if (!String.IsNullOrEmpty(pFacebookId))
            {
                Model.User userFacebook = bllUser.Get(pFacebookId);
                if (userFacebook != null)
                {
                    throw new Exception("Facebook Id already registered");
                }
            }

            return true;
        }

        //check google account is regustered with take65
        protected bool CheckGoogleAccount(string gAccountId)
        {
            //return true;
            Bll.User bllUser = new Bll.User();
            if (!String.IsNullOrEmpty(gAccountId))
            {
                Model.User userGoogle = bllUser.GetGoogleUser(gAccountId);
                if (userGoogle != null)
                {
                    throw new Exception(userGoogle.Email + "  already registered");
                }
            }

            return true;

        }



        public bool Check(Model.User user, bool register = false)
        {
            Bll.User bllUser = new Bll.User();
            Model.User userLogged = this.GetSessionUser();

            try
            {
                if (!String.IsNullOrEmpty(user.FacebookId))
                {
                    CheckFacebook(user.FacebookId);
                }
                else if (!string.IsNullOrEmpty(user.GoogleId))
                {
                    CheckGoogleAccount(user.GoogleId);
                }
                else
                {
                    //Test email
                    if (!String.IsNullOrEmpty(user.Email))
                    {
                        Model.User userEmail = bllUser.GetByEmail(user.Email);
                        if (userEmail != null)
                        {
                            if (userLogged != null)
                            {
                                if (userLogged.Id != userEmail.Id)
                                {
                                    throw new Exception("E-mail already registered");
                                }
                                else
                                {
                                    this.Response<Model.REST.Response>(new Model.REST.Response()
                                    {
                                        status = true,
                                        response = "SAME"
                                    });

                                    return true;
                                }
                            }
                            else
                            {
                                throw new Exception("E-mail already registered");
                            }
                        }
                    }

                    //Test login
                    if (!String.IsNullOrEmpty(user.Login))
                    {
                        Model.User userLogin = bllUser.GetByLogin(user.Login);
                        if (userLogin != null)
                        {
                            throw new Exception("Login already registered");
                        }

                    }
                }

                if (!register)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "OK"
                    });
                }
                return true;
            }
            catch (Exception e)
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = "Error: " + e.Message
                });

                return false;
            }

        }

        public void ForgotPassword(String email)
        {
            String newPassword = Bll.Util.RandomString(6);
            NameValueCollection sendmailParameters = new NameValueCollection();
            Bll.User bllUser = new Bll.User();
            Model.User user = null;

            try
            {
                user = bllUser.GetByEmail(email);
            }
            catch { }

            if (user == null)
            {
                this.Response<Model.REST.Response>(new Model.REST.Response()
                {
                    status = false,
                    response = email + " is not registered in Take65."
                });
            }
            else
            {
                try
                {
                    //this check will make sure to restrict users to request 
                    // password reset only to explicit take65 registered accounts
                    //
                    if (user.FacebookId != null)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Password reset not allowed - Take65 does not store facebook signin account passwords"
                        });
                        return;
                    }
                    else if (user.GoogleId != null)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Password reset not allowed - Take65 does not store google signin account passwords"
                        });
                        return;
                    }
                    else
                    {
                        sendmailParameters.Add("name", user.Name);
                        sendmailParameters.Add("email", user.Email);
                        sendmailParameters.Add("password", newPassword);

                        Bll.Util.SendEmail(string.Empty, user.Email, "[Take65] Forgot password", "ForgotPassword.html", sendmailParameters);

                        //save new password in database
                        bllUser.Save(new Model.User()
                        {
                            Id = user.Id,
                            Password = Bll.Util.EncodeMD5(newPassword),
                            ChangePasswordNextLogin = DateTime.Now
                        });


                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "New password sent for " + email
                        });
                    }
                }
                catch (Exception error)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Error: " + error.Message
                    });
                }

            }
        }

        protected override void Post(string parameter)
        {

            if (parameter.ToUpper() == "LOGIN")
            {
                this.Login();
            }
            else if (parameter.ToUpper() == "REGISTERNEWUSEREMAIL")
            {
                this.RegisterUserEmail();
            }
            else if (parameter.ToUpper() == "CHECK")
            {
                try
                {
                    Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                    this.Check(new Model.User()
                    {
                        Email = restUser.email,
                        Login = restUser.login
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
            else if (parameter.ToUpper() == "CHECKFACEBOOK")
            {
                try
                {
                    Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                    if (string.IsNullOrEmpty(restUser.facebookId))
                    {
                        throw new Exception("facebookId can not be null");
                    }
                    this.Check(new Model.User()
                    {
                        FacebookId = restUser.facebookId
                    });

                    if (!(string.IsNullOrEmpty(restUser.email)))
                    {
                        this.Check(new Model.User()
                        {
                            Email = restUser.email
                        });
                    }
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
            else if (parameter.ToUpper() == "FORGOTPASSWORD")
            {
                try
                {
                    Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                    if (!string.IsNullOrEmpty(restUser.email))
                    {
                        this.ForgotPassword(restUser.email);
                    }
                    else
                    {
                        throw new Exception("No email was provided");
                    }
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
            else if (parameter.ToUpper() == "UPDATEPASSWORD")
            {
                Model.User CurrentUser = this.GetSessionUser();
                if (CurrentUser != null)
                {
                    Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                    if (!string.IsNullOrEmpty(restUser.password))
                    {
                        CurrentUser.Password = Bll.Util.EncodeMD5(restUser.password);
                        CurrentUser.ChangePasswordNextLogin = DateTime.MinValue;

                        Bll.User bllUser = new Bll.User();
                        long userId = bllUser.Save(CurrentUser);

                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "User Updated ID: " + userId
                        });
                    }

                }

            }
            else if (parameter.ToUpper() == "SENDSUPPORTREQUESTEMAIL")
            {
                NameValueCollection sendmailParameters = new NameValueCollection();
                try
                {
                    sendmailParameters.Add("name", GetParameterValue("p2"));
                    sendmailParameters.Add("email", GetParameterValue("p3"));
                    sendmailParameters.Add("body", GetParameterValue("p5"));

                    Bll.Util.SendEmailWithBody(string.Empty, GetParameterValue("p4"), "SupportIssue.html", sendmailParameters);

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Issue submitted."
                    });
                }
                catch (Exception ex)
                {
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = false,
                        response = "Error: " + ex.Message
                    });
                }
            }
            else if (parameter.ToUpper() == "UPDATE")
            {
                Model.User CurrentUser = this.GetSessionUser();
                if (CurrentUser != null)
                {
                    Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                    Model.User user = this.RestModelToModel(restUser);
                    user.Id = CurrentUser.Id;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        user.Password = Bll.Util.EncodeMD5(user.Password);
                    }

                    Bll.User bllUser = new Bll.User();

                    long userId = bllUser.Save(user);
                    user = bllUser.Get(Convert.ToInt32(userId));
                    this.SetSessionUser(user);

                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "User Updated ID: " + userId
                    });

                }
            }
            else if (parameter.ToUpper() == "REGISTERGOOGLEACCOUNT")
            {
                GoogleAccount gAcc = null;
                try
                {
                    //parse the parameter
                    Dictionary<string, string> postparams = new Dictionary<string, string>();
                    string gUserId = string.Empty;
                    postparams = this.ReadJsonRequest<Dictionary<string, string>>();
                    // ensure the request is from trusted source
                    if (!this.IsLegitimateUser(postparams["state"]))
                    {
                        throw new Exception("request can not be processed from un-trusted source....");
                    }
                    //store the auth state in session
                    this.SetSessionState(postparams["state"]);
                    //get the google user id 
                    gUserId = GoogleAuthServer.Instance.ExchangeCode(postparams["authcode"]);
                    if (string.IsNullOrEmpty(gUserId))
                        throw new Exception("Invalid Authentication code :   " + postparams["authcode"]);
                    // check the account exist in Take65
                    CheckGoogleAccount(gUserId);
                    gAcc = RegisterGoogleUser(gUserId);
                    //check if the email exist in Take65
                    try
                    {
                        Bll.User buser = new Bll.User();
                        Model.User muser = buser.GetByEmail(gAcc.EmailId);
                        if (muser != null)
                        {
                            //email exist in Take65
                            if (string.IsNullOrEmpty(muser.FacebookId))
                            {
                                this.Response<Model.REST.Response>(new Model.REST.Response()
                                {
                                    status = false,
                                    response = muser.Email + "  already registered with Take65"
                                });
                            }
                            else
                            {
                                this.Response<Model.REST.Response>(new Model.REST.Response()
                                {
                                    status = false,
                                    response = muser.Email + " already registered with facebook"
                                });
                            }
                            return;
                        }

                    }
                    catch (Exception e)
                    {
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = false,
                            response = "Error: " + e.Message
                        });
                        return;

                    }
                    this.SetGoogleData(postparams["state"], gAcc);
                    // store user in take65 and initiate customise home page flow
                    this.Response<Model.REST.Response>(new Model.REST.Response()
                    {
                        status = true,
                        response = "Registered successfully..."
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
                throw new NotImplementedException();
            }
        }

        protected override void Post()
        {
            Bll.User bllUser = new Bll.User();

            try
            {
                Model.REST.User restUser = this.ReadJsonRequest<Model.REST.User>();
                GoogleAccount gAcc = null;
                //fillup with google data whcih is not available at the client 
                if (!string.IsNullOrEmpty(restUser.googleId))
                {
                    gAcc = this.GetGoogleData(AntiForgeryToken.Instance.ReferenceToken);
                    if (gAcc != null)
                    {
                        GoogleAccountToRestUser(gAcc, restUser);
                    }
                }

                Model.User user = this.RestModelToModel(restUser);
                user.Password = Bll.Util.EncodeMD5(user.Password);

                if (string.IsNullOrEmpty(user.FacebookId) && string.IsNullOrEmpty(user.Email))
                {
                    throw new Exception("You cannot create a user without an email or Facebook User Id");
                }
                else
                {
                    if (this.Check(user, true))
                    {
                        long userId = bllUser.Save(user);
                        user = bllUser.Get(Convert.ToInt32(userId));
                        this.SetSessionUser(user);
                        CreateDefaultWidgets();
                        this.Response<Model.REST.Response>(new Model.REST.Response()
                        {
                            status = true,
                            response = "User registered ID: " + userId
                        });
                    }
                    else
                    {
                        throw new Exception("Unchecked data, please use /REST/User/Check before registering a new user");
                    }
                }
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

        private void CreateDefaultWidgets()
        {
            Bll.UserWidget bllUserWidget = new Bll.UserWidget();


            long userWidgetWeather = bllUserWidget.Save(new Model.UserWidget()
            {
                Name = "Weather",
                Size = 1,
                SystemTagId = (int)Model.Enum.enWidgetType.WEATHER,
                Order = 997,
                UserId = this.GetSessionUser().Id,
                DefaultWidget = true
            });

            long userWidgetFacebookNews = bllUserWidget.Save(new Model.UserWidget()
            {
                Name = "Facebook News Feed",
                Size = 1,
                Order = 998,
                SystemTagId = (int)Model.Enum.enWidgetType.FACEBOOK,
                UserId = this.GetSessionUser().Id,
                DefaultWidget = true
            });

            long userWidgetEmail = bllUserWidget.Save(new Model.UserWidget()
            {
                Name = "Email",
                Size = 1,
                Order = 999,
                SystemTagId = (int)Model.Enum.enWidgetType.EMAIL,
                UserId = this.GetSessionUser().Id,
                DefaultWidget = true
            });
        }

        private Model.User RestModelToModel(Model.REST.User restUser)
        {
            Model.User user = new Model.User();

            user.FacebookId = restUser.facebookId;
            user.GoogleId = restUser.googleId;
            user.GoogleAccessToken = restUser.googleAccessToken;
            user.GoogleRefreshToken = restUser.googleRefreshToken;
            user.GoogleAccessTokenExpires = restUser.googleAccessTokenExpires;
            user.Name = restUser.name;
            user.Email = restUser.email;
            user.Login = restUser.login;
            user.Password = restUser.password;
            user.AddressCity = restUser.city;
            user.AddressPostalCode = restUser.postalCode;
            user.AddressState = restUser.state;
            if (restUser.preference != null)
            {
                if (restUser.preference.Count() > 0)
                {
                    user.Preferences = restUser.preference.Select(x => x.id).ToArray();
                }
            }

            user.Gender = restUser.gender;
            try
            {
                //Client asked to use only year of birth instead the date of birth
                user.Birthdate = new DateTime(Convert.ToInt32(restUser.yearofbirth), 1, 1);
            }
            catch { }

            return user;
        }

        private Model.REST.User ModelToRestModel(Model.User model)
        {
            Model.REST.User restUser = new Model.REST.User();
            restUser.facebookId = model.FacebookId;
            restUser.facebookId = model.FacebookId;
            restUser.googleId = model.GoogleId;
            restUser.googleAccessToken = model.GoogleAccessToken;
            restUser.googleRefreshToken = model.GoogleRefreshToken;
            restUser.googleAccessTokenExpires = model.GoogleAccessTokenExpires;
            restUser.name = model.Name;
            restUser.email = model.Email;
            restUser.login = model.Login;
            //restUser.password = model.Password;
            restUser.city = model.AddressCity;
            restUser.postalCode = model.AddressPostalCode;
            restUser.state = model.AddressState;
            //restUser.birthdate = model.Birthdate;
            if (model.Birthdate != DateTime.MinValue)
            {
                restUser.yearofbirth = model.Birthdate.Year.ToString();
            }
            restUser.gender = model.Gender;
            if (model.Preferences != null)
            {
                restUser.preference = (from tbPref in model.Preferences
                                       select new Model.REST.Preference
                                       {
                                           id = tbPref
                                       }).ToArray();
            }

            return restUser;
        }

        private GoogleAccount RegisterGoogleUser(string gUserId)
        {
            GoogleAccount gAcc = null;
            try
            {
                gAcc = GoogleAuthServer.Instance.GetGoogleUserDetails(gUserId);
            }
            catch (Exception e)
            {
                throw;
            }
            return gAcc;
        }

        protected void GoogleAccountToRestUser(GoogleAccount gAcc, Model.REST.User restUser)
        {
            restUser.googleId = gAcc.UserId;
            restUser.googleAccessToken = gAcc.AccessToken;
            restUser.googleRefreshToken = gAcc.RefreshToken;
            restUser.googleAccessTokenExpires = (long)gAcc.Expiry;
            if (string.IsNullOrEmpty(restUser.name))
                restUser.name = gAcc.UserDisplayName;
            if (string.IsNullOrEmpty(restUser.email))
                restUser.email = gAcc.EmailId;
            if (string.IsNullOrEmpty(restUser.gender))
                restUser.gender = gAcc.UserGender;
        }

        private bool IsRegistered(string googleUserId)
        {
            //TODO Check database
            return false;
        }
    }
}