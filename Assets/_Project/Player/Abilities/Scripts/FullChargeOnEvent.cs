using Stats;
using UnityEngine;

namespace Player.Abilities
{
    [CreateAssetMenu]
    public class FullChargeOnEvent : PlayerAbility
    {
        [SerializeField] private EventAction trigger;

        private PlayerCharges _charges;
        
        public override void OnEquip()
        {
            trigger += OnEvent;
            _charges = FindFirstObjectByType<PlayerCharges>();
        }

        public override void Teardown()
        {
            trigger -= OnEvent;
        }

        private void OnEvent(EventParams data)
        {
            _charges.Refill();
        }
    }
}