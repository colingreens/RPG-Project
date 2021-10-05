using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.GameEvents
{
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {

        [SerializeField] private E gameEvent;
        [SerializeField] private UER unityEventResponse;

        public E GameEvent
        {
            get
            {
                return gameEvent;
            }
            set
            {
                gameEvent = value;
            }
        }

    private void OnEnable()
    {
        if (gameEvent == null)
            return;
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
            return;
        GameEvent.UnregisterListener(this);
    }

        public void OnEventRaised(T item)
        {
            unityEventResponse?.Invoke(item);
        }
    }
}
