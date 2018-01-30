using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNHibernate.Models
{
    public class ToDo
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}