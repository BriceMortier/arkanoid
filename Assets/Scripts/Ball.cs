using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static readonly float ARROW_ROTATION_PER_SECOND = 360;
    private static readonly float ACCELERATION_PER_SECOND = .3f;

    public float power = 1;
    public float initialSpeed = 7;
    public float minimumSpeed = 5;
    public float maximumSpeed = 10;
    public float maximumIncreasedSpeed = 30;
    public float minimumXAxisAngle = 20;
    public float minimumYAxisAngle = 5;

    private Rigidbody2D _body;
    private GameObject _directionArrowBox;
    private bool _isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();

        // Start directional arrow animation
        _directionArrowBox = transform.Find("DirectionArrowBox").gameObject;
        _directionArrowBox.SetActive(true);
        StartCoroutine(nameof(MoveInitialDirection));
    }

    void Update()
    {
        // Launch the ball
        if (!_isMoving && Input.GetMouseButtonDown(0))
        {
            // Init direction and speed
            var direction = _directionArrowBox.transform.rotation;
            _body.velocity = direction.normalized * Vector2.up * initialSpeed;

            // Stop directional arrow animation
            _directionArrowBox.gameObject.SetActive(false);
            StopCoroutine(nameof(MoveInitialDirection));

            _isMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (_isMoving)
        {
            ApplyMinimumAngle();
            ApplySpeedBounds();
            ApplyAcceleration(Time.fixedDeltaTime);
        }
    }

    private void ApplyAcceleration(float deltaTime)
    {
        if (minimumSpeed < maximumIncreasedSpeed)
        {
            minimumSpeed += ACCELERATION_PER_SECOND * deltaTime;
            minimumSpeed = Mathf.Min(minimumSpeed, maximumIncreasedSpeed);
        }

        if (maximumSpeed < maximumIncreasedSpeed)
        {
            maximumSpeed += ACCELERATION_PER_SECOND * deltaTime;
            maximumSpeed = Mathf.Min(maximumSpeed, maximumIncreasedSpeed);
        }
    }

    private IEnumerator MoveInitialDirection()
    {
        float signedRotation = ARROW_ROTATION_PER_SECOND;
        float lowerDegreeBound = -75;
        float upperDegreeBound = 75;
        while (true)
        {
            _directionArrowBox.transform.Rotate(0, 0, signedRotation * Time.fixedDeltaTime);

            // Calculate if bounds are reached
            float angle = Vector2.SignedAngle(Vector2.up, _directionArrowBox.transform.rotation * Vector2.up);
            var outOfBounds = Mathf.Max(lowerDegreeBound - angle, angle - upperDegreeBound);

            if (outOfBounds > 0)
            {
                // Go to opposite direction and apply correction
                signedRotation *= -1;
                _directionArrowBox.transform.Rotate(0, 0, Mathf.Sign(signedRotation) * outOfBounds * 2);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void ApplySpeedBounds()
    {
        if (_body.velocity.magnitude < minimumSpeed)
        {
            Debug.LogFormat("Increasing speed from {0} to {1}", _body.velocity.magnitude, minimumSpeed);
            _body.velocity = _body.velocity.normalized * minimumSpeed;
        }
        else if (_body.velocity.magnitude > maximumSpeed)
        {
            Debug.LogFormat("Lowering speed from {0} to {1}", _body.velocity.magnitude, maximumSpeed);
            _body.velocity = _body.velocity.normalized * maximumSpeed;
        }
    }

    private void ApplyMinimumAngle()
    {
        float rightAngle = Vector2.Angle(Vector2.right, _body.velocity);
        float leftAngle = Vector2.Angle(Vector2.left, _body.velocity);
        float upAngle = Vector2.Angle(Vector2.up, _body.velocity);
        float downAngle = Vector2.Angle(Vector2.down, _body.velocity);
        float xAxisAngle = Mathf.Min(rightAngle, leftAngle);
        float yAxisAngle = Mathf.Min(upAngle, downAngle);

        if (!Mathf.Approximately(xAxisAngle, minimumXAxisAngle) && xAxisAngle < minimumXAxisAngle)
        {
            float angleSign = Mathf.Sign(_body.velocity.x) * Mathf.Sign(_body.velocity.y);
            _body.velocity = Quaternion.Euler(0, 0, angleSign * (minimumXAxisAngle - xAxisAngle)) * _body.velocity;
            Debug.LogFormat("Minimum angle applied, velocity = {0}, angle from X positive axis = {1}", _body.velocity, Vector2.Angle(Vector2.right, _body.velocity));
        }
        else if (!Mathf.Approximately(yAxisAngle, minimumYAxisAngle) && yAxisAngle < minimumYAxisAngle)
        {
            float angleSign = -Mathf.Sign(_body.velocity.x) * Mathf.Sign(_body.velocity.y);
            _body.velocity = Quaternion.Euler(0, 0, angleSign * (minimumYAxisAngle - yAxisAngle)) * _body.velocity;
            Debug.LogFormat("Minimum angle applied, velocity = {0}, angle from Y positive axis = {1}", _body.velocity, Vector2.Angle(Vector2.up, _body.velocity));
        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
