using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace WebApiSample.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get(string gender="all")
        {
            using (PracticeEntities entities = new PracticeEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(emp=>emp.gender == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(emp => emp.gender == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "Value for gender must be all, male or female");
                }
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (PracticeEntities entities = new PracticeEntities())
            {
                var entity = entities.Employees.FirstOrDefault(emp => emp.employee_id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with id" + id.ToString() + " doesnot exist");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee emp)
        {
            try
            {
                using (PracticeEntities entities = new PracticeEntities())
                {
                    entities.Employees.Add(emp);
                    entities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                message.Headers.Location = new Uri(Request.RequestUri + emp.employee_id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (PracticeEntities entities = new PracticeEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(emp => emp.employee_id == id);
                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound,
                            "Employee with id" + id.ToString() + "not found");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (PracticeEntities entities = new PracticeEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(emp => emp.employee_id == id);
                    if (entity != null)
                    {
                        entity.months = employee.months;
                        entity.name = employee.name;
                        entity.salary = employee.salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with id" + id.ToString() + "not found");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
    }
}
