// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Paddle : MonoBehaviour {

// 	// public int playerNumber;
// 	public float speed;

// 	private Rigidbody paddleRigidbody;

// 	// Use this for initialization
// 	void Start () {
// 		paddleRigidbody = gameObject.GetComponent<Rigidbody> ();
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
// 		float horizontalMovement = Input.GetAxis ("Horizontal") * speed;
// 		float verticalMovement = Input.GetAxis ("Vertical") * speed;

// 		paddleRigidbody.velocity = new Vector3 (horizontalMovement, 0, verticalMovement);
// 	}
// }

using Photon.Pun;
using UnityEngine;

public class Paddle : MonoBehaviourPun
{
    public float speed;
    
    private Rigidbody paddleRigidbody;

    // Use this for initialization
    void Start()
    {
        paddleRigidbody = gameObject.GetComponent<Rigidbody>();

        // Ensure the paddle is controlled only by the local player
        if (!photonView.IsMine)
        {
            // If this is not the local player's paddle, disable the script
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow movement for the local player's paddle
        if (photonView.IsMine)
        {
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;
            float verticalMovement = Input.GetAxis("Vertical") * speed;

            paddleRigidbody.velocity = new Vector3(horizontalMovement, 0, verticalMovement);
        }
    }
}

