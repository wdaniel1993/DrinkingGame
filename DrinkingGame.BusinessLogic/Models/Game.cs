using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
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

        public Round CurrentRound => _rounds.FirstOrDefault(x => !x.Complete);

        public GameStateMachine Machine { get; }

        public Game(int id)
        {
            Id = id;
            Machine = new GameStateMachine(new StateFactory(this));
        }

        public IObservable<Player> PlayerAdded => _playerAdded.AsObservable();
        public IObservable<Device> DeviceAdded => _deviceAdded.AsObservable();
        public IObservable<Round> RoundAdded => _roundAdded.AsObservable();

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            _playerAdded.OnNext(player);
        }

        public void AddDevice(Device device)
        {
            _devices.Add(device);
            _deviceAdded.OnNext(device);
        }

        public void AddGuess(Guess guess)
        {
            CurrentRound.AddGuess(guess);

            if (CurrentRound.Guesses.Count() == Players.Count())
            {
                CurrentRound.CompleteRound();
            }
        }

        public void AddRound(Round round)
        {
            _rounds.Add(round);
            _roundAdded.OnNext(round);
        }
    }
}
