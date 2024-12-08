using UnityEngine;
using TMPro; // TextMeshPro
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput, joinRoomInput;
    public TMP_Text feedbackText;
    public byte maxPlayers = 4;

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createRoomInput.text))
        {
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions { MaxPlayers = maxPlayers });
            SetFeedback("Creating room...");
        }
        else SetFeedback("Room name cannot be empty.");
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(joinRoomInput.text))
        {
            PhotonNetwork.JoinRoom(joinRoomInput.text);
            SetFeedback("Joining room...");
        }
        else SetFeedback("Enter a valid room name.");
    }

    public override void OnJoinedRoom() => PhotonNetwork.LoadLevel("Game");

    public override void OnCreateRoomFailed(short returnCode, string message) => SetFeedback("Failed to create room: " + message);

    public override void OnJoinRoomFailed(short returnCode, string message) => SetFeedback("Failed to join room: " + message);

    private void SetFeedback(string message) => feedbackText.text = message;
}