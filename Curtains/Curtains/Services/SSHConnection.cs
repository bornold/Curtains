using Curtains.Helpers;
using Curtains.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Curtains.Services
{
    public class SSHConnection : IConnection<CronJob>
    {
        public SSHConnection(string host, int port, string username, string passKey, Stream fileStream)
        {
            var privateKeyFile = new PrivateKeyFile(fileStream, passKey);
            var privateKeyAuth = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            var connectionInfo = new ConnectionInfo(host, port, username, authenticationMethods: privateKeyAuth)
            {
                Timeout = TimeSpan.FromSeconds(4)
            };
            Connection = new SshClient(connectionInfo);
            Connection.Connect();
        }

        public SshClient Connection;

        List<CronJob> cache;

        BaseClient IConnection<CronJob>.Client => Connection;

        readonly string addCommand = "(crontab -l ; echo \"{0}\") | crontab -";
        public Task<bool> AddItem(CronJob item)
        {
            if (cache.Contains(item)) return Task.FromResult(false);
            else 
            {
                var command = string.Format(addCommand, item.Raw);
                return Task.Run(() => Connection.RunCommand(command).Error == null);
            }
        }

        readonly string removeCommand = "crontab -l | grep -v '^{0}$' | crontab -";
        public Task<bool> DeleteItem(string id)
        {
            var escaped = id.EscapeSpecialCharacterForGrep();
            var command = string.Format(removeCommand, escaped);
            return Task.Run(() => Connection.RunCommand(command).Error == null);
        }

        public async Task<IEnumerable<CronJob>> GetItems(bool forceRefresh = false)
        {
            cache = await Task.Run(() =>
                   Connection
                       .RunCommand("crontab -l")
                       .Result
                       .Split('\n')
                       .Where(NotCommentsAndNotWhiteSpace)
                       .Select(AsCronJob)
                       .ToList());
            
            return cache;
        }

        CronJob AsCronJob(string raw) 
            => string.IsNullOrWhiteSpace(raw) ? null : new CronJob { Raw = raw };

        bool NotCommentsAndNotWhiteSpace(string raw) 
            => !string.IsNullOrWhiteSpace(raw) && !raw.StartsWith("#");

        readonly string updateCommand = "(crontab -l | grep -v '^{0}$' ; echo \"{1}\") | crontab -";
        public Task<bool> UpdateItem(string id, CronJob item)
        {
            var escaped = id.EscapeSpecialCharacterForGrep();
            var command = string.Format(updateCommand, escaped, item.Raw);
            return Task.Run(() => Connection.RunCommand(command).Error == null);
        }

        public Task<string> RunCommand(string command) =>
            Task.Run(() => Connection.RunCommand(command).Result);

        public void Dispose() => Connection.Dispose();
    }
}