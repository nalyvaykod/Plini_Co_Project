using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float sidewaySpeed = 7f;
    [SerializeField] private float horizontalLimit = 3f;
    [SerializeField] private float inputSmoothTime = 0.1f; 

    [Header("Input Settings")]
    [SerializeField] private bool useAccelerometer = false;
    [SerializeField] private float swipeSensitivity = 0.005f;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private float _rawHorizontalInput; 
    private float _smoothedHorizontalInput; 
    private float _currentSmoothVelocity; 

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        GetRawInput();

        _smoothedHorizontalInput = Mathf.SmoothDamp(
            _smoothedHorizontalInput,
            _rawHorizontalInput,
            ref _currentSmoothVelocity,
            inputSmoothTime
        );

        Vector3 newPosition = transform.position;
        newPosition.x += _smoothedHorizontalInput * sidewaySpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -horizontalLimit, horizontalLimit);
        transform.position = newPosition;
    }

    private void GetRawInput()
    {
        _rawHorizontalInput = 0f; 

        if (useAccelerometer)
        {
            _rawHorizontalInput = Input.acceleration.x;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    _rawHorizontalInput = touch.deltaPosition.x * swipeSensitivity;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    _rawHorizontalInput = Input.GetAxis("Mouse X") * mouseSensitivity;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-horizontalLimit, transform.position.y, transform.position.z),
                        new Vector3(horizontalLimit, transform.position.y, transform.position.z));
        Gizmos.DrawWireCube(transform.position, new Vector3(horizontalLimit * 2, 0.1f, 0.1f));
    }
}