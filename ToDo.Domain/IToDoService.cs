using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToDo.Domain
{
    public interface IToDoService : IService
    {
        Task<string> GetHelloWorld();

        Task<Guid> AddToDoAsync(ToDoItem item);
        Task DeleteToDoAsync(Guid id);
        Task<bool> UpdateToDoAsync(ToDoItem item);

        Task<IEnumerable<ToDoItem>> GetToDosAsync(CancellationToken ct);

        Task<ToDoItem> GetToDoAsync(Guid Id);
    }
}
