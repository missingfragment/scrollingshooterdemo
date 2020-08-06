using UnityEngine;

namespace SpaceShooterDemo
{
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