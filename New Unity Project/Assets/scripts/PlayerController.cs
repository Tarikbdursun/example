using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float looksensitivity = 3f;

    [SerializeField]
    private float ThrusterForce = 1000f;

    
    [Header("Spring settings:")]
    [SerializeField]
    [System.Obsolete]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;
    
    //Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    [System.Obsolete]
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    [System.Obsolete]
    void Update()
    {
        //calculatemovement velocity as a 3d vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * _xMov;
        Vector3 movVertical = transform.forward * _zMov;

        //final movement vector
        Vector3 _velocity = (movHorizontal + movVertical)*speed;

        //Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        //apply movement
        motor.Move(_velocity);

        //Calculate rotation as 3D vector(turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f)*looksensitivity;

        //Apply Rotation
        motor.Rotate(_rotation);
        
        //Calculate rotation as 3D vector(turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _camerarotationX = _xRot * looksensitivity;

        //Apply Cam Rotation
        motor.RotateCamera(_camerarotationX);

        // Calculate the thrustforce based on player input
        Vector3 thrusterforce = Vector3.zero;
        if (Input.GetButton("Jump")) 
        {
            thrusterforce = Vector3.up * ThrusterForce;
            SetJointSettings(0f);
        }
        else 
        {
            SetJointSettings(jointSpring);
        }

        //Apply the thrusterForce
        motor.ApplyThruster(thrusterforce); 

    }

    [System.Obsolete]
    private void SetJointSettings(float jointSpring) 
    {
        joint.yDrive = new JointDrive 
        { 
            mode = jointMode, 
            positionSpring = jointSpring, 
            maximumForce = jointMaxForce };
    }
}
