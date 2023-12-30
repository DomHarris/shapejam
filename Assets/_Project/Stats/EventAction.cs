using System;
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
            Debug.Log("Triggering event " + name);
            ActionTriggered?.Invoke(data);
        }
        
        /// <summary>
        /// Trigger an event with no data
        /// </summary>
        public void TriggerAction()
        {
            Debug.Log("Triggering event " + name);
            ActionTriggered?.Invoke(null);
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