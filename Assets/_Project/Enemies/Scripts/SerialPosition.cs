using System;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu]
    public class SerialPosition : ScriptableObject
    {
        public Vector3 Position { get; private set; }
        public event Action<Vector3> OnPositionUpdate; 
        
        public void SetPosition (Vector3 position)
        {
            Position = position;
            OnPositionUpdate?.Invoke(Position);
        }
    }
}