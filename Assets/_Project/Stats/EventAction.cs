using System;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu]
    public class EventAction : ScriptableObject
    {
        public event Action<EventParams> ActionTriggered;
        
        public void TriggerAction(EventParams data)
        {
            ActionTriggered?.Invoke(data);
        }
    }
}