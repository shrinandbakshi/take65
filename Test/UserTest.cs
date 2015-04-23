using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    
    public class UserTest
    {
        private int UserId = 1;
        private String UserName = "Sebastião Dávilla";
        private String UserEmail = "RicardoBastos@GmAiL.com";
        private String NonExistingEmail = "***testemail@test****";
        private String UserPassword = "DoOiÁPoqueAoChuÝ**##121312";
        private String UserPasswordMD5 = "DA49426539A852FFADB864B4F6CB5F66";

        
        public void SaveUser()
        {
            //Bll.User bllUser = new Bll.User();
            //Model.User user = new Model.User();

            //user.Name = this.UserName;
            //user.Email = this.UserEmail;

            ////test if md5 encode is supporting null
            //String testNullPassword = Bll.Util.EncodeMD5(null);
            //user.Password = Bll.Util.EncodeMD5(this.UserPassword);
            //int userId = bllUser.Save(user);

            ////test if md5 is enconding correctly
            //Assert.AreEqual(user.Password, this.UserPasswordMD5);
            ////test if user is saving in database, and its id is > 0
            //Assert.AreNotEqual(userId, 0);
        }

        
        public void CheckEmail()
        {
            //Bll.User bllUser = new Bll.User();

            ////check if check email is finding the user correctly
            //Model.User user = bllUser.Get(this.UserEmail);
            //Assert.AreEqual(user.Name, this.UserName);

            ////check if check email is bringing a result if email is non-existent email
            //user = bllUser.Get(this.NonExistingEmail);
            //Assert.AreEqual(user, null);
        }

        
        public void InsertPreferences()
        {
            //delete all user preference from database
            //Bll.UserPreference bllUserPreference = new Bll.UserPreference();
            //bllUserPreference.Delete(this.UserId);

            /*
            //list and insert all preferences
            Bll.Tag bllTag = new Bll.Tag();
            List<Model.Tag> tagList = bllTag.GetSystemTag(Model.Enum.enSystemTagType.USER_PREFERENCE);
            for (int i = 0; i < tagList.Count; i++)
            {
                bllUserPreference.Save(this.UserId, tagList[i].Id);
            }
             */
        }

        
        public void InsertUserWidget()
        {
            //Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            //Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            //Model.TrustedSource[] trustedSources = bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED,0);

            //long userWidthId = bllUserWidget.Save(new Model.UserWidget()
            //{
            //    Name = "Test Widget",
            //    UserId = this.UserId,
            //    SystemTagId = 1
            //});

            //Assert.AreNotEqual(userWidthId, 0);

            ////Save widget's trusted sources
            //Bll.UserWidgetTrustedSource bllUserWidgetTrustedSource = new Bll.UserWidgetTrustedSource();
            //bllUserWidgetTrustedSource.DeleteTrustedSource(userWidthId);
            //for (int i = 0; i < trustedSources.Length; i++)
            //{
            //    bllUserWidgetTrustedSource.SaveTrustedSourceFeed(userWidthId, trustedSources[i].Id,0);
            //}

            //Assert.Inconclusive("Need to check about WidgetTemplateId, how does it works");
            
        }
    }
}
