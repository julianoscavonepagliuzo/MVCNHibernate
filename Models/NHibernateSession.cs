using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNHibernate.Models
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {

            var configuration = new Configuration();

            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");

            configuration.Configure(configurationPath);

            var configurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\ToDo.hbm.xml");

            configuration.AddFile(configurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory.OpenSession();

        }
    }
}