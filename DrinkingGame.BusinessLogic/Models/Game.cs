using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Game
    {
        private readonly List<Player> _players = new List<Player>();
        private readonly List<Device> _devices = new List<Device>();
        private readonly List<Round> _rounds = new List<Round>();

        private readonly Subject<Player> _playerAdded = new Subject<Player>();
        private readonly Subject<Device> _deviceAdded = new Subject<Device>();
        private readonly Subject<Round> _roundAdded = new Subject<Round>();

        public int Id { get; }

        public IEnumerable<Player> Players => _players.AsReadOnly();
        public IEnumerable<Device> Devices => _devices.AsReadOnly();
        public IEnumerable<Round> Rounds => _rounds.AsReadOnly();

        public Round CurrentRound => _rounds.FirstOrDefault(x => !x.IsCompleted);

        public GameStateMachine Machine { get; }

        public Task<IState> State => Machine.State.FirstAsync().ToTask();

        public Game(int id)
        {
            Id = id;
            Machine = new GameStateMachine(new StateFactory(this));
        }

        public IObservable<Player> PlayerAdded => _playerAdded.AsObservable();
        public IObservable<Device> DeviceAdded => _deviceAdded.AsObservable();
        public IObservable<Round> RoundAdded => _roundAdded.AsObservable();

        public async Task AddPlayer(Player player)
        {
            await CheckState(typeof(Initializing));
            _players.Add(player);
            _playerAdded.OnNext(player);
        }

        public async Task CompleteAddingdPlayers()
        {
            await CheckState(typeof(Initializing));
            _playerAdded.OnCompleted();
        }

        public async Task ComppleteGame()
        {
            await CheckState(typeof(RoundEnding));
            _roundAdded.OnCompleted();
        }

        public async Task CompleteCurrentRound(bool lastRound)
        {
            await CheckState(typeof(LoserDrinking));
            await CurrentRound.CompleteRound(lastRound);
        }

        public async Task AddDevice(Device device)
        {
            await CheckState(typeof(Initializing));
            _devices.Add(device);
            _deviceAdded.OnNext(device);
        }

        public async Task AddGuess(Guess guess)
        {
            await CheckState(typeof(AnswerReading));
            await CurrentRound.AddGuess(guess);
        }

        public async Task AddRound(Round round)
        {
            await CheckState(typeof(RoundEnding));
            round.Game = this;
            _rounds.Add(round);
            _roundAdded.OnNext(round);
        }

        public async Task CheckState(Type type)
        {
            var stateType = (await State).GetType();
            if (!type.IsAssignableFrom(stateType))
            {
                throw new StateException(stateType, type);
            }
        }
    }
}
