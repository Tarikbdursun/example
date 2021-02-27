using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float camerarotX = 0f;
    private float currentCamRotX = 0f;
    private Vector3 ThrustForce = Vector3.zero;

    [SerializeField]
    private float camRotLim = 85f;
    private Rigidbody rb;

    public Camera Cam1 { get => cam; set => cam = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Gets a rotational vector.
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    
    //Gets a camera rotational vect.
    public void RotateCamera(float _camerarotationX)
    {
        camerarotX = _camerarotationX;
    }

    // Get a vector for our thrusters
    public void ApplyThruster (Vector3 thrusterforce) 
    {
        ThrustForce = thrusterforce;
    }


    // Run every physicsiteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //perform movement based on velocity variable
    void PerformMovement() 
    {
        if (velocity != Vector3.zero) 
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

            if (ThrustForce != Vector3.zero) 
            {
                rb.AddForce(ThrustForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
    }

    void PerformRotation() 
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (Cam1 != null) 
        {
            // set our rotation and clamp it
            currentCamRotX += -camerarotX;
            currentCamRotX = Mathf.Clamp(currentCamRotX, -camRotLim, camRotLim);

            //Apply our rotation to the transform of our camera
            Cam1.transform.localEulerAngles = new Vector3(currentCamRotX, 0f, 0f);
        }
    }
}
