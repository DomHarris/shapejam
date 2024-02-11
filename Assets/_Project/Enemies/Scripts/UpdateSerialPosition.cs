using System;
using UnityEngine;

namespace Enemies
{
    public class UpdateSerialPosition : MonoBehaviour
    {
        [SerializeField] private SerialPosition position;

        private void Update()
        {
            position.SetPosition(transform.position);
        }
    }
}