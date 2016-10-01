using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ToDo.Domain;

namespace ToDo.WebAPI.Controllers
{

    [ServiceRequestActionFilter]
    public class ToDoController: ApiController
    {
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/Microservice"));
            return await service.GetToDosAsync(CancellationToken.None);
        }

        [Route("api/ToDo/{Id}")]
        public async Task<ToDoItem> Get(Guid Id)
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/Microservice"));
            return await service.GetToDoAsync(Id);
        }

        public async Task<Guid> Post(ToDoItem Item)
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/Microservice"));
            return await service.AddToDoAsync(Item);
        }

        public async Task<bool> Put(ToDoItem Item)
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/Microservice"));
            return await service.UpdateToDoAsync(Item);
        }

        [Route("api/ToDo/{Id}")]
        public async Task Delete(Guid Id)
        {
            IToDoService service = ServiceProxy.Create<IToDoService>(new System.Uri("fabric:/ToDo.ServiceFabric/Microservice"));
            await service.DeleteToDoAsync(Id);
        }
    }
}
