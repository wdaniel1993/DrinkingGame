using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.Shared.DataTransfer;
using Microsoft.AspNet.SignalR.Client;
using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace DrinkingGame.Client.Core.Hubs
{
    public class DrinkingGameHubProxy : IDisposable, IGameServer
    {
        private readonly IDisposable _disposable;
        private HubConnection _hubConnection;
        private string _path;

        public readonly IHubProxy HubProxy;

        private readonly Subject<CorrectAnswerDto> _correctAnswer = new Subject<CorrectAnswerDto>();
        public IObservable<CorrectAnswerDto> CorrectAnswer => _correctAnswer.AsObservable();

        private readonly Subject<NewAnswerDto> _newAnswer = new Subject<NewAnswerDto>();
        public IObservable<NewAnswerDto> NewAnswer => _newAnswer.AsObservable();

        private readonly Subject<NewRoundDto> _newRound = new Subject<NewRoundDto>();
        public IObservable<NewRoundDto> NewRound => _newRound.AsObservable();

        private readonly Subject<ShouldDrinkDto> _shouldDrink = new Subject<ShouldDrinkDto>();
        public IObservable<ShouldDrinkDto> ShouldDrink => _shouldDrink.AsObservable();

        private readonly Subject<UpdateGameDetailsDto> _updateGameDetails = new Subject<UpdateGameDetailsDto>();
        public IObservable<UpdateGameDetailsDto> UpdateGameDetails => _updateGameDetails.AsObservable();

        private readonly Subject<UpdateScoresDto> _updateScores = new Subject<UpdateScoresDto>();
        public IObservable<UpdateScoresDto> UpdateScores => _updateScores.AsObservable();

        public DrinkingGameHubProxy(HubConnection hubConnection, String path)
        {
            var disposable = new CompositeDisposable();
            _path = path;
            _hubConnection = hubConnection;
            HubProxy = hubConnection.CreateHubProxy(path);

            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.CorrectAnswer), _correctAnswer));
            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.NewAnswer), _newAnswer));
            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.NewRound), _newRound));
            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.ShouldDrink), _shouldDrink));
            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.UpdateGameDetails), _updateGameDetails));
            disposable.Add(ConnectToEvent(HubProxy, nameof(IGameClient.UpdateScores), _updateScores));

            _disposable = disposable;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private static IDisposable ConnectToEvent<T>(IHubProxy hubProxy,string methodName, Subject<T> subject)
        {
            return hubProxy.On<T>(ConvertToCamelCase(methodName), subject.OnNext);
        }

        public async Task ConnectToGame(ConnectToGameDto dto)
        {
            await HubProxy.Invoke(nameof(IGameServer.ConnectToGame),dto);
        }

        public async Task PlayerDrank(PlayerDrankDto dto)
        {
            await HubProxy.Invoke(nameof(IGameServer.PlayerDrank), dto);
        }

        public async Task GaveAnswer(GaveAnswerDto dto)
        {
            await HubProxy.Invoke(nameof(IGameServer.GaveAnswer), dto);
        }

        private static string ConvertToCamelCase(string phrase)
        {
            string[] splittedPhrase = phrase.Split(' ', '-', '.');
            var sb = new StringBuilder();

            foreach (String s in splittedPhrase)
            {
                char[] splittedPhraseChars = s.ToCharArray();
                if (splittedPhraseChars.Length > 0)
                {
                    splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper().ToCharArray())[0];
                }
                sb.Append(new String(splittedPhraseChars));
            }
            return sb.ToString();
        }

    }
}
