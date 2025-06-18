using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float sidewaySpeed = 7f;
    [SerializeField] private float horizontalLimit = 3f;

    [SerializeField] private bool useAccelerometer = false;
    [SerializeField] private float swipeSensitivity = 0.1f;

    private Vector3 _lastMousePosition; 
    private bool _isMouseDown = false;   

    private float _currentHorizontalInput;
    private Vector3 _startTouchPosition;

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        HandleInput();

        Vector3 newPosition = transform.position;
        newPosition.x += _currentHorizontalInput * sidewaySpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -horizontalLimit, horizontalLimit);
        transform.position = newPosition;
    }

    private void HandleInput()
    {
        _currentHorizontalInput = 0f;

        if (useAccelerometer)
        {
            _currentHorizontalInput = Input.acceleration.x;
        }
        else 
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.position.x - _startTouchPosition.x;
                    _currentHorizontalInput = deltaX * swipeSensitivity;
                    _startTouchPosition = touch.position;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) 
                {
                    _lastMousePosition = Input.mousePosition; 
                    _isMouseDown = true;
                }
                else if (Input.GetMouseButtonUp(0)) 
                {
                    _isMouseDown = false;
                    _currentHorizontalInput = 0f; 
                }
                else if (Input.GetMouseButton(0) && _isMouseDown) 
                {
                    float deltaX = Input.mousePosition.x - _lastMousePosition.x;
                    _currentHorizontalInput = deltaX * swipeSensitivity * 0.1f; 
                    _lastMousePosition = Input.mousePosition; 
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-horizontalLimit, transform.position.y, transform.position.z),
                        new Vector3(horizontalLimit, transform.position.y, transform.position.z));
        Gizmos.DrawWireCube(transform.position, new Vector3(horizontalLimit * 2, 0.1f, 0.1f));
    }
}