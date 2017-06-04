using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using DrinkingGame.WebService.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;

namespace DrinkingGame.WebService.Dialogs
{
    //[LuisModel("82ff336f-f639-4866-b416-7761d4a8f126", "b0c3c0efe8444b97adf92cce9e31997c")]
    [Serializable]
    public class GameDialog : IDialog<Object>
    {
        [NonSerialized]
        private IGameService _gameService;

        public GameDialog()
        {
            ResolveDependencies();   
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            _gameService = DependencyManager.Current.Resolve<IGameService>();
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            var reply = $"You sent {activity.Text} which was {length} characters";
            await context.SayAsync(reply,reply,new MessageOptions
            {
                InputHint = InputHints.ExpectingInput
            });
            context.Wait(MessageReceivedAsync);
        }
    }
}