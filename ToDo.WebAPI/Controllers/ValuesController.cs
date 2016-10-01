using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Collections.Generic;
using System.Web.Http;
using ToDo.Domain;

namespace ToDo.WebAPI.Controllers
{
    [ServiceRequestActionFilter]
    public class ValuesController : ApiController
    {
        // GET api/values 
        public string Get()
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/ToDo.Microservice"));
            return service.GetHelloWorld();
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
