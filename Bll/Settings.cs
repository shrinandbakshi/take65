using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class Settings
    {
        #region Instance
        private static Settings instance = default(Settings);
        /// <summary>
        /// New Settings instance
        /// </summary>
        public static Settings Instance
        {
            get
            {
                return instance ?? new Settings();
            }
        }
        #endregion
    }
}
