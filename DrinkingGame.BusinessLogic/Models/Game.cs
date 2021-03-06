﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        public string Language { get; }

        public IEnumerable<Player> Players => _players.AsReadOnly();
        public IEnumerable<Device> Devices => _devices.AsReadOnly();
        public IEnumerable<Round> Rounds => _rounds.AsReadOnly();

        public Round CurrentRound => _rounds.FirstOrDefault(x => !x.IsCompleted);

        public GameStateMachine Machine { get; }

        public IObservable<IState> State => Machine.State.FirstAsync();

        public Game(int id, string language)
        {
            Id = id;
            Language = language;
            Machine = new GameStateMachine(new StateFactory(this));

            Machine.Initialize();
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

        public async Task IgnoreDrinks()
        {
            await CheckState(typeof(LoserDrinking));
            CurrentRound.IgnoreDrinks();
        }

        public async Task CompleteCurrentRound(bool lastRound)
        {
            await CheckState(typeof(RoundEnding));
            CurrentRound.CompleteRound(lastRound);
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
            CurrentRound.AddGuess(guess);
        }

        public async Task PlayerDrank(Player player)
        {
            await CheckState(typeof(LoserDrinking));
            CurrentRound.PlayerDrank(player);
        }

        public async Task AddRound(Round round)
        {
            await CheckState(typeof(RoundStarting));
            round.Game = this;
            _rounds.Add(round);
            _roundAdded.OnNext(round);
        }

        private async Task CheckState(params Type[] types)
        {
            var stateType = (await State).GetType();
            if (!types.Any(x => x.IsAssignableFrom(stateType)))
            {
                throw new StateException(stateType, types);
            }
        }
    }
}
