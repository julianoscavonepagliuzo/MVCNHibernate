using MVCNHibernate.Models;
using NHibernate;
using NHibernate.Linq; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCNHibernate.Controllers
{
    [RoutePrefix("todo")]
    public class WebApiController : ApiController
    {
        ISession session = NHibernateSession.OpenSession();
        
        [Route("all")]
        public List<ToDo> GetListToDo()
        {
            List<ToDo> toDo = session.Query<ToDo>().ToList();
            return toDo;
        }
        
        [HttpPost]
        public HttpResponseMessage AddToDo(ToDo toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(toDo);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{id:int}")]
        [HttpGet]
        public ToDo DetailsToDo(int id)
        {
            var toDo = session.Get<ToDo>(id);
            return toDo;
        }

        [HttpPut]
        public HttpResponseMessage UpdateToDo(ToDo toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var toDoObj = session.Get<ToDo>(toDo.ID);
                    toDoObj.Name = toDo.Name;
                    toDoObj.Description = toDo.Description;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(toDoObj);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
  
        [HttpDelete]
        public HttpResponseMessage DeleteToDo(int id)
        {
            try
            {
                var toDo = session.Get<ToDo>(id);
                if (toDo != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(toDo);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error !");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
