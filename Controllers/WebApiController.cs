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
    [RoutePrefix("api")]
    public class WebApiController : ApiController
    {
        ISession session = NHibernateSession.OpenSession();

        [Route("todo")]
        [HttpGet]
        public IHttpActionResult GetListToDo(int page, int size)
        {
            List<ToDo> toDo = session.Query<ToDo>()?.ToList();

            if (toDo == null || toDo.Count == default(int))
            {
                return Ok();
            }

            if (page < 1)
            {
                return BadRequest();
            }

            if (size < 1)
            {
                return BadRequest();
            }

            if (page == 1)
            {
                return Json(toDo.Take(size));
            }

            return Json(toDo.Skip((page - 1) * size).Take(size));
        }

        [Route("todo")]
        [HttpPost]
        public IHttpActionResult AddToDo(ToDo toDo)
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
                    return Created(Url.Link("GetById", new { id = toDo.ID }), toDo);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("todo/{id}", Name = "GetById")]
        [HttpGet]
        public IHttpActionResult DetailsToDo(int id)
        {
            var toDo = session.Get<ToDo>(id);

            if (toDo == null)
            {
                return NotFound();
            }

            return Json(toDo);
        }

        [Route("todo")]
        [HttpPut]
        public IHttpActionResult UpdateToDo(ToDo toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var toDoObj = session.Get<ToDo>(toDo.ID);

                    if (toDoObj == null)
                    {
                        return NotFound();
                    }

                    toDoObj.Name = toDo.Name;
                    toDoObj.Description = toDo.Description;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(toDoObj);
                        transaction.Commit();
                    }
                    return Json(toDoObj);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("todo/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteToDo(int id)
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
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }
    }
}
