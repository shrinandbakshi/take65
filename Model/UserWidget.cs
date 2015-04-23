using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserWidget
    {
        public long Id { get; set; }
        public long UserId {get;set;}

        public String Name { get; set; }
        public int SystemTagId { get; set; }
        public int Size { get; set; }
        public int Order { get; set; }
        public bool DefaultWidget { get; set; }

        public string UserWidgetExtraInfoXML { get; set; }

        public int Row { get; set; }
        public int Col { get; set; }

        public String Category { get; set; }
        public DateTime Register { get; set; }
    }
}
