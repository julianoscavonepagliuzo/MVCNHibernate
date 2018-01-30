using MVCNHibernate.Models;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCNHibernate.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var toDo = session.Query<ToDo>().ToList();

                return View(toDo);
            }

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ToDo objToDo)
        {
            try
            {

                using (ISession session = NHibernateSession.OpenSession())
                {

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(objToDo);
                        transaction.Commit();

                    }

                }


                return RedirectToAction("Index");

            }

            catch (Exception exception)
            {

                return View();

            }

        }

        public ActionResult Edit(int ID)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var objToDo = session.Get<ToDo>(ID);

                return View(objToDo);
            }
        }

        [HttpPost]
        public ActionResult Edit(int ID, ToDo objToDo)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {

                    var objToDoUpdate = session.Get<ToDo>(ID);

                    objToDoUpdate.Name = objToDo.Name;
                    objToDoUpdate.Description = objToDo.Description;

                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        session.Save(objToDoUpdate);

                        transaction.Commit();
                    }

                }

                return RedirectToAction("Index");

            }

            catch
            {

                return View();

            }


        }

        public ActionResult Delete(int ID)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var objToDo = session.Get<ToDo>(ID);


                return View(objToDo);
            }

        }
        [HttpPost]

        public ActionResult Delete(int ID, ToDo objToDo)
        {
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(objToDo);
                        transaction.Commit();
                    }

                }

                return RedirectToAction("Index");

            }

            catch (Exception exception)
            {

                return View();

            }

        }
    }
}
