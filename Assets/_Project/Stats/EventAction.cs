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
            ActionTriggered?.Invoke(data);
        }
    }
}