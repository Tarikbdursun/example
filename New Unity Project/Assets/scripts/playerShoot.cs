using UnityEngine.Networking;
using UnityEngine;

[System.Obsolete]
public class playerShoot : NetworkBehaviour
{
    private const string Player_Tag = "Player";
    

    
    public playerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    public Camera Cam { get => cam; set => cam = value; }
    public LayerMask Mask { get => mask; set => mask = value; }

    void Start()
    {
        if (Cam == null) 
        {
            Debug.LogError("Player Shoot: No camera referenced!");
            enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }
            
    }

    [Client]
    void Shoot() 
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, weapon.range, Mask))
        {
            if (hit.collider.CompareTag(Player_Tag))
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot (string _playerID, int _damage) 
    {
        Debug.Log(_playerID + " has been shot");

        Player _player = GamManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
