using System;
using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// A basic type of Projectile that moves forward in a straight line.
    /// </summary>
    public class PlasmaBolt : Projectile
    {
        public override void Remove()
        {
            PlasmaBoltPool.Instance.ReturnToPool(this);
        }
    }
}
