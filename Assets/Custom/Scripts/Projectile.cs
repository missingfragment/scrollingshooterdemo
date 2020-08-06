using UnityEngine;
using System;

namespace SpaceShooterDemo
{
    [RequireComponent(typeof(Movement))]
    public abstract class Projectile : MonoBehaviour
    {
        private Vector2 direction = Vector2.up;

        private VisibilityChecker visibilityChecker;

        // properties
        public Movement Mover { get; private set; }

        public int Power { get; private set; }
        public Team Alignment { get; private set; }

        // private methods

        private void Awake()
        {
            Mover = gameObject.GetComponent<Movement>();
            visibilityChecker = GetComponentInChildren<VisibilityChecker>();
        }

        private void OnEnable()
        {
            visibilityChecker.VisibilityChanged += OnVisibilityChanged;
        }

        private void OnDisable()
        {
            visibilityChecker.VisibilityChanged -= OnVisibilityChanged;
        }

        private void Update()
        {
            Mover.Inputs = direction;
        }

        private void OnVisibilityChanged(bool visible)
        {
            if (!visible)
            {
                Remove();
            }
        }
        
        // public methods

        public abstract void Remove();

        public void Init(int power, Team alignment, Vector2 direction)
        {
            Power = power;
            Alignment = alignment;
            this.direction = direction;
        }
    }
}
