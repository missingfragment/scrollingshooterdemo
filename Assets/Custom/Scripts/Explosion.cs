using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class Explosion : MonoBehaviour
    {
        public Coroutine coroutine;

        public void OnEnable()
        {
            IEnumerator DestroyAfterSetTime()
            {
                yield return new WaitForSeconds(1);
                Remove();
            }

            coroutine = StartCoroutine(DestroyAfterSetTime());
            
        }

        public void Remove()
        {
            ExplosionPool.Instance.ReturnToPool(this);
        }
    }
}
