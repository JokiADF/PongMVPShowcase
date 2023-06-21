using System;
using UniRx;

namespace CodeBase.Helpers
{
    public abstract class StateModel<T> where T : Enum
    {
        public ReactiveProperty<T> State { get; private set; }

        public StateModel(T state)
        {
            State = new ReactiveProperty<T>(state);
        }
    }
}
