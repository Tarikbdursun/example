using System.Collections.Generic;
using UnityEngine;

public class GamManager : MonoBehaviour
{
    public static GamManager instance;
    public MatchSettings matchsettings;
    

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("More than one Game Manager in scene.");
        }
        else 
        {
           instance = this; 
        }
        
    }

    #region Player Tracking


    private const string PLAYER_ID_PREFIX = "Player ";
    [System.Obsolete]
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public MatchSettings MatchSettings { get; set; }

    [System.Obsolete]
    public static void RegisterPlayer(string netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    [System.Obsolete]
    public static void UnregisterPlayer(string _playerID) 
    {
        players.Remove(_playerID);
    }

    [System.Obsolete]
    public static Player GetPlayer(string _playerID) 
    {
        return players[_playerID];
    }

    /*private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));

        GUILayout.BeginVertical();

        foreach(string _playerID in players.Keys) 
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }



        GUILayout.EndVertical();

        GUILayout.EndArea();
    } */
    #endregion
}
