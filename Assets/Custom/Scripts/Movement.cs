using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooterDemo
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer sprite;
        [SerializeField]
        protected float maxSpeed;
        [SerializeField]
        protected float accel;
        [SerializeField]
        protected float decel;
        [SerializeField]
        protected bool keepInBounds;

        protected Camera mainCamera;

        private Vector2 screenBounds;
        private float objectWidth;
        private float objectHeight;

        public Vector2 Inputs { get; set; } = new Vector2(0, 0);
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);

        void Start()
        {
            mainCamera = Camera.main;
            screenBounds = mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width, Screen.height,
                mainCamera.transform.position.z)
                );
            objectWidth = sprite.bounds.extents.x;
            objectHeight = sprite.bounds.extents.y;
        }

        void FixedUpdate()
        {
            if (Velocity.magnitude >= decel)
            {
                Velocity -= Velocity.normalized * decel;
            }
            else
            {
                Velocity = Vector2.zero;
            }

            Velocity += Inputs * accel;
        

            Velocity = Vector2.ClampMagnitude(Velocity, maxSpeed);

            transform.Translate(Velocity * Time.deltaTime);

            if (!keepInBounds)
            {
                return;
            }

            // keep object inside the screen boundaries
            Vector3 position = transform.position;

            position.x = Mathf.Clamp(position.x,
                -screenBounds.x + objectWidth,
                screenBounds.x - objectWidth
                );

            position.y = Mathf.Clamp(position.y,
                -screenBounds.y + objectHeight,
                screenBounds.y - objectHeight
                );

            if (transform.position.x != position.x)
            {
                Velocity = new Vector2(0f, Velocity.y);
            }
            else if (transform.position.y != position.y)
            {
                Velocity = new Vector2(Velocity.x, 0f);
            }

            transform.position = position;
        }
    }
}
