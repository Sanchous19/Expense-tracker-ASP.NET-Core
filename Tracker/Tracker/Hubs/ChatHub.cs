using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Models;


namespace Tracker.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;

        public ChatHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendMessage(string chatId, string text, string userFromId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                User user = context.Users.Find(userFromId);

                Message message = new Message() { Text = text, UserFromId = userFromId };
                context.Messages.Add(message);

                Chat chat = context.Chats.Find(chatId);
                context.Entry(chat).Collection("Messages").Load();
                chat.Messages.Add(message);
                chat.DateTimeUpdate = message.Date;

                context.SaveChanges();

                string date = message.Date.ToShortTimeString() + " " + message.Date.ToShortDateString();

                await Clients.All.SendAsync("ReceiveMessage", chatId, text, userFromId, user.UserName, date);
            }
        }
    }
}