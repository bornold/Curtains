using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Curtains.Services
{
    public interface IConnection<T> : IDisposable
    {
        Task<bool> AddItem(T item);
        Task<bool> UpdateItem(string id, T item);
        Task<bool> DeleteItem(string id);
        Task<IEnumerable<T>> GetItems(bool forceRefresh = false);
        Renci.SshNet.BaseClient Client { get; }
        Task<string> RunCommand(string command);
    }
}