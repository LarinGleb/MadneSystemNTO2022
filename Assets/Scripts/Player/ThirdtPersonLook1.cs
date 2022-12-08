using UnityEngine;

public class ThirdtPersonLook1 : MonoBehaviour
{
    [SerializeField]
    public Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    private Transform myTransform;
    [Header("Visual")]
    public float characterRotationSpeed;
    
    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        //character = GetComponentInParent<FirstPersonMovement>().transform;
        
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        myTransform = transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y") / 10f);
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -1, 2);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);

        myTransform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {
            //character.localRotation = Quaternion.Lerp(character.localRotation, Quaternion.AngleAxis(velocity.x, Vector3.up), characterRotationSpeed * Time.deltaTime);
        }
    }
}
