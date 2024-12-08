// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Puck : MonoBehaviour {

// 	public delegate void GoalHandler();
// 	public event GoalHandler OnGoal;

// 	public float deceleration;
// 	public float startingHorizontalPosition;

// 	private Rigidbody puckRigidbody;

// 	// Use this for initialization
// 	void Start () {
// 		puckRigidbody = gameObject.GetComponent<Rigidbody> ();
// 		ResetPosition (true);
// 	}
	
// 	void FixedUpdate () {
// 		puckRigidbody.velocity = new Vector3 (
// 			puckRigidbody.velocity.x * deceleration,
// 			puckRigidbody.velocity.y * deceleration,
// 			puckRigidbody.velocity.z * deceleration
// 		);
// 	}

// 	void OnCollisionEnter (Collision collision) {
// 		if (collision.gameObject.tag == "Goal") {
// 			if (OnGoal != null) {
// 				OnGoal ();
// 	 		}
// 	 	} 
// 	else {
// 			gameObject.GetComponent<AudioSource> ().Play ();
// 		}
// 	}

// 	public void ResetPosition (bool isLeft) {
// 		transform.position = new Vector3 (
// 			startingHorizontalPosition * (isLeft ? -1 : 1),
// 			transform.position.y,
// 			transform.position.z
// 		);
// 		puckRigidbody.velocity = Vector3.zero;
// 	}
// }

using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Puck : MonoBehaviourPun, IPunObservable  // Implement IPunObservable for synchronization
{
    public delegate void GoalHandler();
    public event GoalHandler OnGoal;

    public float deceleration;
    public float startingHorizontalPosition;

    private Rigidbody puckRigidbody;

    // Use this for initialization
    void Start() {
        puckRigidbody = gameObject.GetComponent<Rigidbody>();

        // Only allow one player (the master client) to control the puck’s position resets
        if (PhotonNetwork.IsMasterClient)
        {
            ResetPosition(true);
        }
    }

    void FixedUpdate() {
        if (PhotonNetwork.IsMasterClient)
        {
            // Apply deceleration on the puck only on the MasterClient
            Vector3 velocity = puckRigidbody.velocity;
            velocity.x *= deceleration;
            velocity.y *= deceleration;
            velocity.z *= deceleration;
            puckRigidbody.velocity = velocity;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Goal")) {
            if (PhotonNetwork.IsMasterClient && OnGoal != null) {
                Debug.Log("Puck collided with Goal. Triggering OnGoal event.");
                OnGoal();  // Trigger goal event only on the MasterClient
            }
        } else {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void ResetPosition(bool isLeft) {
        if (PhotonNetwork.IsMasterClient) {
            // Reset puck position only on the MasterClient
            Vector3 newPosition = new Vector3(
                startingHorizontalPosition * (isLeft ? -1 : 1),
                transform.position.y,
                transform.position.z
            );
            puckRigidbody.velocity = Vector3.zero;
            transform.position = newPosition;

            // Send the updated position and velocity to other clients
            photonView.RPC("RPC_ResetPosition", RpcTarget.Others, newPosition, Vector3.zero);
        }
    }

    // This method is called to synchronize the puck’s position and velocity over the network
    [PunRPC]
    public void RPC_ResetPosition(Vector3 newPosition, Vector3 newVelocity) {
        transform.position = newPosition;
        puckRigidbody.velocity = newVelocity;
    }

    // Implement IPunObservable to synchronize puck's position and velocity
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            // Send the position and velocity data from the master client to others
            stream.SendNext(puckRigidbody.position);
            stream.SendNext(puckRigidbody.velocity);
        } else {
            // Receive and update the position and velocity for non-master clients
            puckRigidbody.position = (Vector3)stream.ReceiveNext();
            puckRigidbody.velocity = (Vector3)stream.ReceiveNext();
        }
    }
}
