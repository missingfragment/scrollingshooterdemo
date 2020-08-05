using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer _sprite;
    [SerializeField]
    protected float _maxSpeed;
    [SerializeField]
    protected float _accel;
    [SerializeField]
    protected float _decel;

    protected Camera _camera;

    private Vector2 _screenBounds;
    private float _objectWidth;
    private float _objectHeight;

    public Vector2 Inputs { get; set; } = new Vector2(0, 0);
    public Vector2 Velocity { get; private set; } = new Vector2(0, 0);

    void Start()
    {
        _camera = Camera.main;
        _screenBounds = _camera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height,
            _camera.transform.position.z)
            );
        _objectWidth = _sprite.bounds.extents.x;
        _objectHeight = _sprite.bounds.extents.y;
    }

    void FixedUpdate()
    {
        Velocity -= Velocity.normalized * _decel;
        Velocity += Inputs * _accel;
        

        Velocity = Vector2.ClampMagnitude(Velocity, _maxSpeed);

        transform.Translate(Velocity * Time.deltaTime);

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x,
            -_screenBounds.x + _objectWidth,
            _screenBounds.x - _objectWidth
            );

        position.y = Mathf.Clamp(position.y,
            -_screenBounds.y + _objectHeight,
            _screenBounds.y - _objectHeight
            );

        transform.position = position;
    }
}
