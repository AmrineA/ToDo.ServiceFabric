using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ToDo.Domain;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Data;

namespace ToDo.Microservice
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Microservice : StatefulService, IToDoService
    {
        private const string ToDoDictionaryName = "ToDoDictionary";

        public Microservice(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //return new ServiceReplicaListener[0];
            return new[] { new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context)) };
        }

        public async Task<string> GetHelloWorld()
        {
            return await Task.Run(() => { return "Hello World"; });
        }
        

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        //protected override async Task RunAsync(CancellationToken cancellationToken)
        //{
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            //var toDoDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);

            //while (true)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();

            //    using (var tx = this.StateManager.CreateTransaction())
            //    {
            //        var result = await myDictionary.TryGetValueAsync(tx, "Counter");

            //        ServiceEventSource.Current.ServiceMessage(this, "Current Counter Value: {0}",
            //            result.HasValue ? result.Value.ToString() : "Value does not exist.");

            //        await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

            //        // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
            //        // discarded, and nothing is saved to the secondary replicas.
            //        await tx.CommitAsync();
            //    }

            //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            //}
        //}

        public async Task<Guid> AddToDoAsync(ToDoItem item)
        {
            IReliableDictionary<Guid, ToDoItem> toDoItems = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);
            item.Id = Guid.NewGuid();
            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                await toDoItems.AddAsync(tx, item.Id, item);
                await tx.CommitAsync();
                ServiceEventSource.Current.ServiceMessage(this, "Created to do item: {0}", item);
            }
            return item.Id;
        }

        public async Task DeleteToDoAsync(Guid id)
        {
            var toDoDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);

            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                await toDoDictionary.TryRemoveAsync(tx, id);
                await tx.CommitAsync();
            }
        }

        public async Task<bool> UpdateToDoAsync(ToDoItem item)
        {
            var toDoDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);

            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                ConditionalValue<ToDoItem> internalItem = await toDoDictionary.TryGetValueAsync(tx, item.Id);

                if (internalItem.HasValue)
                {
                    internalItem.Value.Completed = item.Completed;
                    internalItem.Value.Effort = item.Effort;
                    internalItem.Value.Name = item.Name;

                    await toDoDictionary.SetAsync(tx, item.Id, internalItem.Value);
                    await tx.CommitAsync();
                    ServiceEventSource.Current.ServiceMessage(this, "Update to do item: {0}", item.Id);

                    return true;
                }
                else
                    return false;
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetToDosAsync(CancellationToken ct)
        {
            var toDoDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);

            IList<ToDoItem> results = new List<ToDoItem>();

            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                IAsyncEnumerator<KeyValuePair<Guid, ToDoItem>> enumerator = (await toDoDictionary.CreateEnumerableAsync(tx)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(ct))
                {
                    results.Add(enumerator.Current.Value);
                }
            }

            return results;
        }

        public async Task<ToDoItem> GetToDoAsync(Guid Id)
        {
            var toDoDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, ToDoItem>>(ToDoDictionaryName);

            using (ITransaction tx = this.StateManager.CreateTransaction())
            {
                ConditionalValue<ToDoItem> internalItem = await toDoDictionary.TryGetValueAsync(tx, Id);

                if (internalItem.HasValue)
                {
                    return internalItem.Value;
                }
                else
                    return null;
            }
        }
    }
}
