using NuBot.Adapters.Gitter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Adapters.Gitter.Messages;

namespace NuBot.Adapters.Gitter
{
    public class GitterAdapter : Adapter
    {
        private static readonly string UserUrl = "https://api.gitter.im/v1/user";

        private readonly string _token;

        private User _user;
        private IEnumerable<Room> _rooms;

        public GitterAdapter(string token)
        {
            _token = token;
        }

        public override string UserName => _user?.UserName;

        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            var readTasks = new List<Task>();

            foreach (var room in _rooms)
            {
                readTasks.Add(ReadRoomEventsAsync(room.Id, cancellationToken));
            }

            await Task.WhenAll(readTasks);
        }

        public async Task SendAsync(string channel, string message)
        {
            var serializer = new DataContractJsonSerializer(typeof(Message));

            using (var client = new HttpClient())
            {
                var url = $"https://api.gitter.im/v1/rooms/{channel}/chatMessages?access_token={_token}";
                var body = new Message
                {
                    Text = message
                };

                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, body);

                    var json = Encoding.UTF8.GetString(ms.ToArray());
                    await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                }
            }
        }

        public override async Task SetupAsync()
        {
            var user = await GetUserAsync();

            if(user == null)
            {
                throw new Exception("Could not read user.");
            }

            _user = user;
            _rooms = await GetRoomsAsync(user.Id);
        }

        private async Task<User> GetUserAsync()
        {
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync($"{UserUrl}?access_token={_token}"))
            {
                var serializer = new DataContractJsonSerializer(typeof(User[]));
                var users = serializer.ReadObject(stream) as User[];

                return users?.FirstOrDefault();
            }
        }

        private async Task<IEnumerable<Room>> GetRoomsAsync(string userId)
        {
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync($"{UserUrl}/{userId}/rooms?access_token={_token}"))
            {
                var serializer = new DataContractJsonSerializer(typeof(Room[]));
                return serializer.ReadObject(stream) as Room[];
            }
        }

        private async Task ReadRoomEventsAsync(string roomId, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                var url = $"https://stream.gitter.im/v1/rooms/{roomId}/chatMessages?access_token={_token}";
                var stream = await client.GetStreamAsync(url);
                var serializer = new DataContractJsonSerializer(typeof(Message));

                using (var reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                    {
                        var ev = await reader.ReadLineAsync();

                        if(string.IsNullOrWhiteSpace(ev))
                        {
                            continue;
                        }

                        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(ev)))
                        {
                            var msg = serializer.ReadObject(ms) as Message;

                            // Ignore messages from ourself.

                            if(msg.FromUser.Id == _user.Id)
                            {
                                continue;
                            }

                            Emit(new GitterTextMessage(null, null, msg.Text));
                        }
                    }
                }
            }
        }
    }
}
