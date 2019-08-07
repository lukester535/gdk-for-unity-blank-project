using System;
using System.Collections;
using BlankProject;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

namespace Scripts.Sphere
{
    public class SphereHealthManager : MonoBehaviour
    {
        [Require] private HealthWriter healthWriter;

        [Require] private HealthCommandReceiver healthCommandReceiver;

        public int damage = 10;
        public float healthReductionInterval = 5f;

        private Option<int> healthToSend;

        private void OnEnable()
        {
            healthCommandReceiver.OnHealRequestReceived += OnHealRequest;

            StartCoroutine(ReduceHealth());
        }

        private void OnHealRequest(Health.Heal.ReceivedRequest obj)
        {
            if (!healthToSend.TryGetValue(out var health))
            {
                health = healthWriter.Data.Health;
            }

            if (health >= GameConstants.MaxHealth)
            {
                return;
            }

            health += obj.Payload.HealAmount;
            healthToSend = Math.Min(health, GameConstants.MaxHealth);

            healthWriter.SendHealedEvent(new HealInfo
            {
                HealType = healthToSend == GameConstants.MaxHealth
                    ? HealType.FULL
                    : HealType.PARTIAL
            });
        }

        private void OnDisable()
        {
            StopCoroutine(ReduceHealth());
        }

        private void LateUpdate()
        {
            SendHealthUpdate();
        }

        IEnumerator ReduceHealth()
        {
            while (true)
            {
                if (!healthToSend.TryGetValue(out var health))
                {
                    health = healthWriter.Data.Health;
                }

                healthToSend = Math.Max(health - damage, 0);
                if (healthToSend == 0)
                {
                    break;
                }

                yield return new WaitForSeconds(healthReductionInterval);
            }
        }

        private void SendHealthUpdate()
        {
            if (!healthToSend.TryGetValue(out var health))
            {
                return;
            }

            healthWriter.SendUpdate(new Health.Update
            {
                Health = health
            });

            healthToSend = Option<int>.Empty;
        }
    }
}