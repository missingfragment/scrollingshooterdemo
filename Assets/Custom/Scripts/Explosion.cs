using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class Explosion : MonoBehaviour
    {
        public IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            Remove();
        }

        public void Remove()
        {
            ExplosionPool.Instance.ReturnToPool(this);
        }
    }
}
