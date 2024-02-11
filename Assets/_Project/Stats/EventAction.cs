using System;
using DG.Tweening;
using UnityEngine;

namespace Stats
{
    /// <summary>
    /// A scriptable object that can be used to trigger events
    /// </summary>
    [CreateAssetMenu]
    public class EventAction : ScriptableObject
    {
        // Events
        public event Action<EventParams> ActionTriggered;
        
        /// <summary>
        /// Trigger the event
        /// </summary>
        /// <param name="data"></param>
        public void TriggerAction(EventParams data)
        {
            ActionTriggered?.Invoke(data);
        }
        
        /// <summary>
        /// Trigger an event with no data
        /// </summary>
        public void TriggerAction()
        {
            ActionTriggered?.Invoke(null);
        }

        public void TriggerActionWithDelay(float delay)
        {
            DOVirtual.DelayedCall(delay, TriggerAction);
        }
        
        // operator override += to add a listener
        public static EventAction operator +(EventAction action, Action<EventParams> listener)
        {
            action.ActionTriggered += listener;
            return action;
        }
        
        // operator override -= to remove a listener
        public static EventAction operator -(EventAction action, Action<EventParams> listener)
        {
            action.ActionTriggered -= listener;
            return action;
        }
    }
}