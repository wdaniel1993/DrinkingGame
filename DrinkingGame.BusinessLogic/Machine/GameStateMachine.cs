﻿using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using DrinkingGame.BusinessLogic.States;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.Machine
{
    public class GameStateMachine
    {
        private readonly IStateFactory _factory;
        private readonly Subject<IState> _state;

        public IObservable<IState> State => _state.AsObservable();

        public GameStateMachine(IStateFactory factory)
        {
            _factory = factory;

            _state = new Subject<IState>();
        }

        public IDisposable Initialize(IState startState = null)
        {
            IObservable<Transition> transitions = _state
                .StartWith(startState ?? _factory.GameStarting())
                .Select(state => state.Enter())
                .Switch()
                .Publish()
                .RefCount();
            
            var states = Observable.Merge(
                transitions.Where(transition => transition == Transition.ToInitializing).Select(_ => _factory.Initializing()),
                transitions.Where(transition => transition == Transition.ToRoundStarting).Select(_ => _factory.RoundStarting()),
                transitions.Where(transition => transition == Transition.ToAnswerReading).Select(_ => _factory.AnswerReading()),
                transitions.Where(transition => transition == Transition.ToLoserDrinking).Select(_ => _factory.LoserDrinking()),
                transitions.Where(transition => transition == Transition.ToRoundEnding).Select(_ => _factory.RoundEnding()),
                transitions.Where(transition => transition == Transition.ToGameEnding).Select(_ => _factory.GameEnding())
            );
            
            return states
                .ObserveOn(Scheduler.CurrentThread)
                .Subscribe(_state);
        }
    }
}
