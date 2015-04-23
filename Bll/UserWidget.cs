using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class UserWidget
    {
        private Dal.UserWidget DalUserWidget = new Dal.UserWidget();

        public Model.UserWidget Get(long id)
        {
            return this.DalUserWidget.GetUserWidget(id);
        }

        public Model.UserWidget[] GetUserWidget(long userId)
        {
 
            return this.GetUserWidget(userId, 0);
        }

        public Model.UserWidget[] GetUserWidget(long userId, int systemTagId)
        {
            return this.DalUserWidget.GetUserWidget(userId, systemTagId);
        }

        public long Save(Model.UserWidget userWidget)
        {
            return this.DalUserWidget.Save(userWidget);
        }

        public void SaveExtraInfo(long pUserId, long pUserWidgetId, string pExtraInfo)
        {
            this.DalUserWidget.SaveExtraInfo(pUserId, pUserWidgetId, pExtraInfo);
        }

        public Model.Tag[] GetUserWidgetCategory(long userId)
        {
            return this.DalUserWidget.GetUserWidgetCategory(userId);
        }

        

        public void Search(String searchArray, int userWidthId)
        {
            
        }

        public void Delete(long userWidgetId)
        {
            this.DalUserWidget.Delete(userWidgetId);
        }       
        public void SavePosition(List<Model.UserWidget> widgets)
        {
            foreach (Model.UserWidget uw in widgets)
            {
                DalUserWidget.SavePosition(uw);
            }
        }

        //public static List<Model.UserWidget> OrderWidgets(List<Model.UserWidget> widgets)
        //{
        //    int row = 1;
        //    int col = 1;
        //    List<Model.UserWidget> ltWidgets = widgets;
        //    int maxRow = ltWidgets.Max(x => x.Row);
        //    int maxCol = ltWidgets.Max(x => x.Col);

        //    row = (maxRow > 0) ? maxRow : row;
        //    var maxLine = ltWidgets.Where(x => x.Row == row);
        //    if (maxLine.Count() > 0)
        //    {
        //        col = ltWidgets.Where(x => x.Row == row).Max(x => x.Col);
        //    }
            
        //    foreach (Model.UserWidget uw in widgets.Where(x => x.Row <= 0).ToList())
        //    {
                
        //        if (uw.Size >= 3)
        //        {
        //            uw.Row = row;
        //            row++;
        //            uw.Col = 1;
        //        }
        //        else
        //        {
        //            if (col > 3)
        //            {
        //                col = 1;
        //                row++;
        //            }
        //            uw.Row = row;
        //            uw.Col = col;
        //            col++;
        //        }
        //    }

        //    foreach (Model.UserWidget uw in widgets)
        //    {
        //        if (widgets.Where(x => x.Col == uw.Col && x.Row == uw.Row && x.Id != uw.Id).Count() > 0)
        //        {
        //            uw.Row = 0;
        //            if (uw.Size >= 3)
        //            {
        //                uw.Row = row;
        //                row++;
        //                uw.Col = 1;
        //            }
        //            else
        //            {
        //                if (col > 3)
        //                {
        //                    col = 1;
        //                    row++;
        //                }
        //                uw.Row = row;
        //                uw.Col = col;
        //                col++;
        //            }
        //        }
        //    }
        //    Bll.UserWidget bllUserWidget = new UserWidget();
        //    bllUserWidget.SavePosition(widgets);

        //    return widgets;
        //}

        #region OrderWidgets
        /// <summary>
        /// Order widgets when new widget/frame is added.
        /// </summary>
        /// <param name="widgets"></param>
        /// <returns></returns>
        public List<Model.UserWidget> OrderWidgets(List<Model.UserWidget> widgets)
        {            
            List<Model.UserWidget> newWidgetsList = widgets.Where(x => x.Row <= 0).ToList();

            if (newWidgetsList == null || newWidgetsList.Count == 0)
                return widgets;

            int currRow = 1;            
            int maxRow = widgets.Max(x => x.Row);                      
            currRow = (maxRow > 0) ? maxRow : currRow;


            int widgetMaxRow = widgets.Where(x => x.Size < 3 ).Max(x => x.Row);
            int widgetCurrRow = widgetMaxRow > 0 ? widgetMaxRow : currRow;                       
            int widgetMaxCol = widgetMaxRow == 0 ? 0 : widgets.Where(x => x.Row == widgetCurrRow).Max(x => x.Col);
            int widgetCurrCol = widgetMaxCol + 1;

            foreach (Model.UserWidget uw in widgets.Where(x => x.Row <= 0).ToList())
            {
                if (uw.Size >= 3)
                {
                    if (maxRow == 0 && currRow == 1)
                        currRow = 1;                    
                    
                    uw.Row = currRow;                    
                    uw.Col = 1;

                    currRow++;
                    widgetCurrCol = 1;
                }
                else
                {                  
                    if (widgetCurrCol > 3)
                    {
                        widgetCurrCol = 1;
                        currRow = widgetCurrRow + 1 > currRow ? widgetCurrRow + 1 : currRow + 1;
                        widgetCurrRow = currRow;
                    }
                    uw.Row = currRow;
                    uw.Col = widgetCurrCol;
                    widgetCurrCol++;
                }
            }
          
            Bll.UserWidget bllUserWidget = new UserWidget();
            bllUserWidget.SavePosition(widgets);           

            return widgets;
        }
        #endregion
    }
}
