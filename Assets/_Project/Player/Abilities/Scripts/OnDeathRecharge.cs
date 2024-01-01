using Entity;
using Stats;
using UnityEngine;

namespace Player.Abilities
{
    public class OnDeathRecharge : PlayerAbility
    {
        [SerializeField] private float deathDelay = 1f;
        [SerializeField] private EventAction replacementDeathEvent;

        private PlayerCharges _charges;
        private PlayerShoot _shoot;
        private EntityHealth _health;
        private EventAction _previousDieEvent;
        
        public override void OnEquip()
        {
            
        }

        public override void Teardown()
        {
            
        }

        private async void PlayerDied()
        {
            
        }
    }
}