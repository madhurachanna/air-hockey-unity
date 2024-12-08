// 

using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to load scenes

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();  // Connect to Photon master server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon!");
        PhotonNetwork.JoinLobby();  // Join a lobby after connection
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby! Redirecting to Lobby scene...");

        // Load the Lobby scene
        SceneManager.LoadScene("Lobby");
    }
}
