using System;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class PlasmaBolt : Projectile
    {
        public override void Remove()
        {
            PlasmaBoltPool.Instance.ReturnToPool(this);
        }
    }
}
