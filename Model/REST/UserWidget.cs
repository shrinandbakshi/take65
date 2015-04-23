using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.REST
{
    public class UserWidget
    {
        public long id { get; set; }
        public int typeId { get; set; }
        public int[] categoryId { get; set; }
        public String typeName { get; set; }
        public String title { get; set; }
        public int size { get; set; }
        public bool isDeletable { get; set; }
        public bool isDefault { get; set; }//change this variable name to isPublicWidget
        public bool isSystemDefault { get; set; }
        public string token { get; set; }
        public int row { get; set; }
        public int col { get; set; }

        public string zipCode { get; set; } //used for weather
    }
}
