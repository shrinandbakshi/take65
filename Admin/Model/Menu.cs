using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Model
{
    public class Menu
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public List<Menu> SubMenu { get; set; }
    }
}
