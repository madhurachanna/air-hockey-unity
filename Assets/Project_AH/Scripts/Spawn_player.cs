// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;

// public class SpwanPlayers : MonoBehaviour
// {
//     public GameObject playerPrefab;
    
//     // Define the fixed spawn position for the player
//     public Vector3 spawnPosition;

//     // Start is called before the first frame update
//     public void Start()
//     {
//         // Set the spawn position (example with x = 10.71175, y = 1.0, z = 0.0)
//         spawnPosition = new Vector3(-6.0f, 8.78f,0f);
//     // Use the set spawn position
//         PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
//     }
// }

// using UnityEngine;
// using Photon.Pun;

// public class SpawnPlayers : MonoBehaviour
// {
//     public GameObject playerPrefab;
    
//     // Define spawn positions for the two players
//     private Vector3[] spawnPositions = new Vector3[2];

//     // Start is called before the first frame update
//     public void Start()
//     {
//         // Define two fixed spawn positions for the two players
//         spawnPositions[0] = new Vector3(-6.0f, 8.78f, 0f);  // Position for Player 1
//         spawnPositions[1] = new Vector3(6.0f, 8.78f, 0f);   // Position for Player 2

//         // Get the player's index (Photon provides a unique ActorNumber for each player)
//         int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;  // Subtract 1 to make it 0-based

//         // Ensure that the player index is valid (in case more than 2 players join)
//         if (playerIndex < 2)
//         {
//             // Use the spawn position corresponding to the player's index
//             Vector3 spawnPosition = spawnPositions[playerIndex];

//             // Instantiate the player over the network using Photon
//             PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
//         }
//         else
//         {
//             Debug.LogError("More than 2 players are trying to join!");
//         }
//     }
// }
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;  // Prefab for the players
    public GameObject puckPrefab;    // Prefab for the puck

    // Define spawn positions for the two players
    private Vector3[] spawnPositions = new Vector3[2];
    // Define spawn rotations for the two players
    private Quaternion[] spawnRotations = new Quaternion[2];

    // Reference to the puck GameObject
    private GameObject puck;

    // Start is called before the first frame update
    public void Start()
    {
        // Define two fixed spawn positions for the two players
        spawnPositions[0] = new Vector3(-6.0f, 8.78f, 0f);  // Position for Player 1
        spawnPositions[1] = new Vector3(6.0f, 8.78f, 0f);   // Position for Player 2

        // Define rotations for each player (can be customized)
        spawnRotations[0] = Quaternion.Euler(-90, 0 , -90); 
        spawnRotations[1] = Quaternion.Euler(-90, 0 , -90); 

        // Get the player's index (Photon provides a unique ActorNumber for each player)
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;  // Subtract 1 to make it 0-based

        // Ensure that the player index is valid (in case more than 2 players join)
        if (playerIndex < 2)
        {
            // Use the spawn position and rotation corresponding to the player's index
            Vector3 spawnPosition = spawnPositions[playerIndex];
            Quaternion spawnRotation = spawnRotations[playerIndex];

            // Instantiate the player over the network using Photon with the assigned rotation
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogError("More than 2 players are trying to join!");
        }

        // Ensure only Master Client spawns the puck
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPuck();
        }
    }

    private void SpawnPuck()
    {
        // Instantiate the puck if it doesn't already exist
        if (puck == null)
        {
            // Spawn the puck at the center of the field
            puck = PhotonNetwork.Instantiate(puckPrefab.name, new Vector3(0f, 8.78f, 0f), Quaternion.Euler(-90f, 0f, 0f));

            // Find the GameController and pass the puck reference
            GameController gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.SetPuck(puck.GetComponent<Puck>());
            }
            else
            {
                Debug.LogError("GameController not found in the scene!");
            }

            Debug.Log("Puck spawned successfully by Master Client.");
        }
    }

    // Optional: Handle Master Client changes if the original Master Client disconnects
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        // Only the new Master Client should check if the puck needs to be instantiated
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPuck();
        }
    }
}
