using UnityEngine;

namespace SpaceShooterDemo
{
    /// <summary>
    /// Provides functionality for moving a GameObject.
    /// </summary>
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
        [SerializeField]
        protected bool rotateWithMovement;

        protected Camera mainCamera;

        private Vector2 screenBounds;
        private float objectWidth;
        private float objectHeight;

        public Vector2 Inputs { get; set; } = new Vector2(0, 0);
        public Vector2 Velocity { get; set; } = new Vector2(0, 0);

        public bool RotateWithMovement
        {
            get => rotateWithMovement;
            set => rotateWithMovement = value;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            screenBounds = mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width, Screen.height,
                mainCamera.transform.position.z)
                );
            objectWidth = sprite.bounds.extents.x;
            objectHeight = sprite.bounds.extents.y;
        }

        private void OnDisable()
        {
            Inputs = Vector2.zero;
            Velocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (Inputs.magnitude <= 0.1f)
            {
                if (Velocity.magnitude >= decel)
                {
                    Velocity -= Velocity.normalized * decel;
                }
                else
                {
                    Velocity = Vector2.zero;
                }
            }

            Velocity += Inputs * accel;
       
            Velocity = Vector2.ClampMagnitude(Velocity, maxSpeed);

            if (rotateWithMovement)
            {
                Vector3 velocity3 = new Vector3(Velocity.x, Velocity.y, 0);

                transform.position += velocity3 * Time.deltaTime;

                float angle = Mathf.Atan2(Velocity.x, Velocity.y)
                    * Mathf.Rad2Deg;

                transform.rotation = Quaternion.identity;

                transform.Rotate(0f, 0f, -angle);
            }
            else
            {
                transform.Translate(Velocity * Time.deltaTime);
            }

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

            // If our position changed from moving back onscreen,
            // that means we're up against the screen,
            // so we should zero our velocity in that direction.
            if (transform.position.x != position.x)
            {
                Velocity = new Vector2(0f, Inputs.y * maxSpeed);
            }
            else if (transform.position.y != position.y)
            {
                Velocity = new Vector2(Inputs.x * maxSpeed, 0f);
            }

            transform.position = position;
        }
    }
}
