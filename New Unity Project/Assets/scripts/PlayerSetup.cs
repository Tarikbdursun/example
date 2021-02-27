using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string DontdrawLayerName = "Dontdraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    public GameObject playerUIprefab;
    private GameObject playerUIinstance;

    public PlayerSetup(GameObject playerGraphics)
    {
        this.playerGraphics = playerGraphics;
    }

    Camera sceneCam;

    public Behaviour[] ComponentsToDisable { get => componentsToDisable; set => componentsToDisable = value; }

    void Start()
    {
        if (!isLocalPlayer) 
        {
            DisableComponents();
            AssingnRemoteLayer();
        }
        else 
        {
            sceneCam = Camera.main;
            if (sceneCam != null) 
            {
                sceneCam.gameObject.SetActive(false);
            }

            //disable player graphics for localplayer
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(DontdrawLayerName));

            //create player UI
            playerUIinstance = Instantiate(playerUIprefab);
            playerUIinstance.name = playerUIprefab.name;
            
        }

        GetComponent<Player>().Setup();
    }

    void SetLayerRecursively(GameObject obj , int newLayer) 
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform) 
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GamManager.RegisterPlayer(netID, _player);
        
    }



    void AssingnRemoteLayer() 
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    void DisableComponents() 
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    private void OnDisable()
    {
        Destroy(playerUIinstance);

        if (sceneCam != null) 
        {
            sceneCam.gameObject.SetActive(true);
        }

        GamManager.UnregisterPlayer(transform.name);
    }
}
