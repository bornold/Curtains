using Curtains.Helpers;
using Curtains.Models;
using Renci.SshNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Curtains.Services
{
    public class SSHClient : IDataStore<CronJob>
    {
        public SSHClient(string host, int port, string username, string passKey, Stream fileStream)
        {
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(fileStream, passKey);
            var privateKeyAuth = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            ConnectionInfo connectionInfo = new ConnectionInfo(host, port, username, authenticationMethods: privateKeyAuth);

            Connection = new SshClient(connectionInfo);
            Connection.Connect();
        }

        public SshClient Connection;

        BaseClient IDataStore<CronJob>.Client => Connection;

        readonly string addCommand = "(crontab -l ; echo \"{0}\") | crontab -";
        public Task<bool> AddItem(CronJob item)
        {
            var command = string.Format(addCommand, item.Raw);
            return Task.Run(() => Connection.RunCommand(command).Error == null);
        }

        readonly string removeCommand = "crontab -l | grep -v '{0}'  | crontab -";
        public Task<bool> DeleteItem(string id)
        {
            var escaped = id.EscapeStarForGrep();
            var command = string.Format(removeCommand, escaped);
            return 
                Task.Run(() => Connection.RunCommand(command).Error == null);
        }

        public Task<IEnumerable<CronJob>> GetItems(bool forceRefresh = false)
            => Task.Run(() =>
                    Connection
                        .RunCommand("crontab -l")
                        .Result
                        .Split('\n')
                        .Where(NotCommentsAndNotWhiteSpace)
                        .Select(AsCronJob));

        CronJob AsCronJob(string raw) 
            => string.IsNullOrWhiteSpace(raw) ? null : new CronJob { Raw = raw };
        bool NotCommentsAndNotWhiteSpace(string raw) 
            => !string.IsNullOrWhiteSpace(raw) && !raw.StartsWith("#");
        public async Task<bool> UpdateItem(string id, CronJob item)
        {
            await DeleteItem(id);
            return await AddItem(item);
        }
        public void Dispose() 
            => Connection.Dispose();
    }
}