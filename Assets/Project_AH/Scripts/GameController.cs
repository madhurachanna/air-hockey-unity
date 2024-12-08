// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class GameController : MonoBehaviour {

// 	public Puck puck;
// 	public TextMeshProUGUI scoreText;

// 	private int score1;
// 	private int score2;
// 	private bool isGameOver;
// 	private float resetTimer = 3f;

// 	// Use this for initialization
// 	void Start () {
// 		puck.OnGoal += OnGoal;
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
// 		if (isGameOver == true) {
// 			resetTimer -= Time.deltaTime;
// 			if (resetTimer <= 0) {
// 				SceneManager.LoadScene ("Game");
// 			}
// 		}
// 	}

// 	void OnGoal() {
// 		if (puck.transform.position.x > 0) {
// 			score1++;
// 			puck.ResetPosition (false);
// 		} 
// 		else { 
// 			score2++;
// 			puck.ResetPosition (true);
// 		}
// 		scoreText.text = score1 + ":" + score2;

// 		if (score1 == 3) {
// 			scoreText.text = "Player 1 wins!";
// 			puck.gameObject.SetActive (false);
// 			isGameOver = true;
// 		} 
// 		else if (score2 == 3) {
// 			scoreText.text = "Player 2 wins!";
// 			puck.gameObject.SetActive (false);
// 			isGameOver = true;
// 		}
// 	}
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using TMPro;
// using Photon.Pun;

// public class GameController : MonoBehaviour
// {
//     public TextMeshProUGUI scoreText;  // UI for displaying score
//     private Puck puck;  // Reference to the puck
//     private int score1;
//     private int score2;
//     private bool isGameOver;
//     private float resetTimer = 3f;

//     // Use this for initialization
//     void Start()
//     {
//         StartCoroutine(FindPuckAndSubscribe());
//     }

//     // Coroutine to find the puck and subscribe to OnGoal event
//     IEnumerator FindPuckAndSubscribe()
//     {
//         while (puck == null)  // Keep checking until the puck is found
//         {
//             GameObject puckObject = GameObject.FindWithTag("Puck");  // Look for the puck by tag
//             if (puckObject != null)
//             {
//                 puck = puckObject.GetComponent<Puck>();
//                 if (puck != null)
//                 {
//                     Debug.Log("Puck found, subscribing to OnGoal event.");
//                     puck.OnGoal += OnGoal;  // Subscribe to the goal event
//                 }
//                 else
//                 {
//                     Debug.LogWarning("Puck object found, but no Puck component attached.");
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning("Puck not found, waiting...");
//             }
//             yield return null;  // Wait for the next frame and try again
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (isGameOver == true)
//         {
//             resetTimer -= Time.deltaTime;
//             if (resetTimer <= 0)
//             {
//                 SceneManager.LoadScene("Game");
//             }
//         }
//     }

//     // Event handler for when a goal is scored
//     void OnGoal()
//     {
//         if (puck.transform.position.x > 0)
//         {
//             score1++;
//             puck.ResetPosition(false);  // Reset for Player 2's side
//         }
//         else
//         {
//             score2++;
//             puck.ResetPosition(true);  // Reset for Player 1's side
//         }

//         // Update the score display
//         scoreText.text = score1 + " : " + score2;

//         // Check for game over
//         if (score1 == 3)
//         {
//             scoreText.text = "Player 1 wins!";
//             puck.gameObject.SetActive(false);  // Disable the puck
//             isGameOver = true;
//         }
//         else if (score2 == 3)
//         {
//             scoreText.text = "Player 2 wins!";
//             puck.gameObject.SetActive(false);  // Disable the puck
//             isGameOver = true;
//         }
//     }
// }
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class GameController : MonoBehaviourPun
{
    public TextMeshProUGUI scoreText;
    private Puck puck;
    private int score1; // Player 1's score
    private int score2; // Player 2's score
    private bool isGameOver;
    private float resetTimer = 3f;

    // Singleton pattern for easy access from other scripts
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        // Ensure this GameController instance is a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    // This method is called when a goal is scored
    public void OnGoal()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Check the puck's position to determine which player scored
            if (puck.transform.position.x > 0)
            {
                // Player 2 scores because the puck crossed Player 1's side
                score1++;  // Increment Player 2's score
                puck.ResetPosition(false);
            }
            else
            {
                // Player 1 scores because the puck crossed Player 2's side
                score2++;  // Increment Player 1's score
                puck.ResetPosition(true);
            }

            // Synchronize the updated score with all clients
            photonView.RPC("UpdateScore", RpcTarget.All, score1, score2);

            // Check for a winner
            if (score1 >= 5)
            {
                photonView.RPC("EndGame", RpcTarget.All, "Player 1 Wins!");
            }
            else if (score2 >= 5)
            {
                photonView.RPC("EndGame", RpcTarget.All, "Player 2 Wins!");
            }
        }
    }

    // RPC method to update the score across all clients
    [PunRPC]
    public void UpdateScore(int newScore1, int newScore2)
    {
        score1 = newScore1;
        score2 = newScore2;
        scoreText.text = $"{score1}:{score2}";
    }

    // RPC method to end the game across all clients
    [PunRPC]
    public void EndGame(string winner)
    {
        scoreText.text = winner;
        if (puck != null)
        {
            puck.gameObject.SetActive(false);
        }
        isGameOver = true;
    }

    // Method to set the puck reference from outside
    public void SetPuck(Puck newPuck)
    {
        if (puck != null)
        {
            puck.OnGoal -= OnGoal; // Unsubscribe from the previous puck's event (if any)
        }

        puck = newPuck; // Store the new puck reference
        puck.OnGoal += OnGoal; // Subscribe to the new puck's OnGoal event
    }
}

