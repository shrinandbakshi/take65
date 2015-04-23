using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest
    {

        [TestMethod]
        public void ImportFeedContent()
        {
            //Bll.FeedContentImport bllFeedContentImport = new Bll.FeedContentImport();
            //bllFeedContentImport.LoadContent();
        }

        [TestMethod]
        public void UserWidgetSaveFeed()
        {
            //Bll.UserWidget bllUserWidget = new Bll.UserWidget();
            //Model.UserWidget uw = new Model.UserWidget();
            //uw.Name = "Test Widget " + DateTime.Now.ToShortDateString() + ":" + DateTime.Now.ToShortTimeString();
            //uw.SystemTagId = 29; //FEED
            //uw.UserId = 159; //teste@teste.com - 123
            //uw.Id = bllUserWidget.Save(uw);

            //Bll.Tag bllTag = new Bll.Tag();
            //Bll.TrustedSource bllTrustedSource = new Bll.TrustedSource();
            //Bll.UserWidgetTrustedSource bllUWTS = new Bll.UserWidgetTrustedSource();

            ////cycle trough feed categories
            //List<Model.Tag> categories = bllTag.GetSystemTag(Model.Enum.enSystemTagType.CATEGORY);
            //for (int a = 0; a < categories.Count(); a++)
            //{
            //    Model.TrustedSource[] tsList = bllTrustedSource.GetTrustedSource(Model.Enum.enTrustedSourceType.FEED, categories[a].Id );
            //    if (tsList != null)
            //    {
            //        //cycle trought each trusted source feed
            //        for (int i = 0; i < tsList.Length; i++)
            //        {
            //            //save users wiget feed
            //            bllUWTS.SaveTrustedSourceFeed(uw.Id, tsList[i].Id, categories[a].Id);

            //        }
            //    }
            //}
        }
    }
}
