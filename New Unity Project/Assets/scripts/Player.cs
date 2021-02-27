using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

[System.Obsolete]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead 
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }


    
    [SerializeField]
    private int MaxHealth = 100;

    [SyncVar]
    private int CurrentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    public Player(Behaviour[] disableOnDeath)
    {
        this.disableOnDeath = disableOnDeath;
    }

    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++) 
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    /*private void Update()
    {
        if (!isLocalPlayer) 
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K)) 
        {
            RpcTakeDamage(99999);
        }
    } */

    [ClientRpc]
    public void RpcTakeDamage(int _amount) 
    {
        if (isDead) 
        {
            return;
        }
        
        CurrentHealth -= _amount;
        Debug.Log(transform.name + " now has " + CurrentHealth + " health.");

        if (CurrentHealth <= 0) 
        {
            Die();
        }
    }

    private void Die() 
    {
        isDead = true;

        for (int i=0; i<disableOnDeath.Length; i++) 
        {
            disableOnDeath[i].enabled = false;
        }
        
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        Debug.Log(transform.name + "is DEAD!");

        //Call Respawn method
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() 
    {
        yield return new WaitForSeconds(GamManager.instance.matchsettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " Respawned.");
    }

    public void SetDefaults() 
    {
        isDead = false;
        
        CurrentHealth = MaxHealth;

        for (int i=0; i<disableOnDeath.Length; i++) 
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null) 
        {
            _col.enabled = true;
        }
    }
}
