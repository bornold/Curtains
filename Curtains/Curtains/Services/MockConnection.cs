using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curtains.Models;
using Renci.SshNet;

namespace Curtains.Services
{
    public class MockConnection : IConnection<CronJob>
    {
        List<CronJob> CronJobs;

        public BaseClient Client => new MockedClient(null, false);

        public MockConnection()
        {
            CronJobs = new List<CronJob>();
            var mockCronJobs = new List<CronJob>
            {
                new CronJob { Raw = $"13 30 * * fri ls" },
                new CronJob { Raw = $"13 30 * * fri,mon,sun pwd" },
                new CronJob { Raw = $"13 30 * * tue ls" },
                new CronJob { Raw = $"13 30 * * sat ls" },
                new CronJob { Raw = $"13 30 * * * ls" },
            };

            foreach (var cronJob in mockCronJobs)
            {
                CronJobs.Add(cronJob);
            }
        }

        public async Task<bool> AddItem(CronJob CronJob)
        {
            CronJobs.Add(CronJob);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItem(string id, CronJob CronJob)
        {
            var oldCronJob = CronJobs.Where((CronJob arg) => arg.Raw == id).FirstOrDefault();
            CronJobs.Remove(oldCronJob);
            CronJobs.Add(CronJob);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItem(string raw)
        {
            var oldCronJob = CronJobs.Where((CronJob arg) => arg.Raw == raw).FirstOrDefault();
            CronJobs.Remove(oldCronJob);

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<CronJob>> GetItems(bool forceRefresh = false) => await Task.FromResult(CronJobs);

        public void Dispose() { }

        public Task<string> RunCommand(string command) => Task.FromResult("ok");
        

        class MockedClient : BaseClient
        {

            public MockedClient(ConnectionInfo connectionInfo, bool ownsConnectionInfo) : base(connectionInfo, ownsConnectionInfo)
            {
            }
        }
    }
}