﻿namespace RPG.GameEvents
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}