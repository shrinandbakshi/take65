using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserWidgetTag
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long UserWidgetId { get; set; }
        public long SystemTagId {get;set;}
    }
}
