using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class UserWidget : IUserWidget
    {
        /// <summary>
        /// Retorna vários user widgets de um usuário
        /// </summary>
        /// <param name="pUserWidgetId"></param>
        /// <param name="pSkip"></param>
        /// <param name="pCount"></param>
        /// <returns></returns>
        public List<Model.UserWidget> Get(long pUserWidgetId, int pSkip, int pCount)
        {
            return null;
        }

        /// <summary>
        /// Salva um novo user widget
        /// </summary>
        /// <param name="pUserWidget"></param>
        /// <returns></returns>
        public Model.UserWidget Save(Model.UserWidget pUserWidget)
        {
            return null;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
