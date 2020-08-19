using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// An interface that provides properties for knowing if it's
    /// enabled and ready to fire, as well as a method for firing.
    /// </summary>
    public interface IWeapon
    {
        // properties
        Transform Transform { get; }

        bool Enabled { get; set; }
        bool ReadyToFire { get; }

        // methods

        void Fire();
    }
}