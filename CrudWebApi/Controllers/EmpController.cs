using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServicesApi;

namespace CrudWebApi.Controllers
{
    public class EmpController : ApiController
    {
        public IEnumerable<Emp> Get()
        {
            using (EmpDataEntities entities = new EmpDataEntities())
            {
                return entities.Emps.ToList();
            }
        }

        public Emp Get(int id)
        {
            using(EmpDataEntities entities = new EmpDataEntities())
            {
                return entities.Emps.FirstOrDefault(e => e.ID == id);
            }
        }

        public HttpResponseMessage Post([FromBody] Emp emp)
        {
            try
            {
                using (EmpDataEntities entities = new EmpDataEntities())
                {
                    entities.Emps.Add(emp);
                    entities.SaveChanges();

                    var msg = Request.CreateResponse(HttpStatusCode.Created, emp);
                    msg.Headers.Location = new Uri(Request.RequestUri + emp.ID.ToString());
                    return msg;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmpDataEntities entities = new EmpDataEntities())
                {
                    var entity = entities.Emps.FirstOrDefault(e => e.ID == id);
                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id : " + id.ToString() + "not found to delete");
                    }
                    else
                    {
                        entities.Emps.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
