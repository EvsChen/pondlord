using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class PCNetwork : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    public byte Version = 1;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;
    public GameObject mJoinRoomBtn;

    RoomInfo[] currentRoomList;

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.autoCleanUpPlayerObjects = true;    // we join randomly. always. no need to join a lobby to get the list of rooms.
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
            //PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, "1", "cluster3");       // connecting to a specific cluster may be necessary, when regions get sharded and you support friends
        }
    }


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        // PhotonNetwork.JoinRandomRoom();
    }


    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        GetRoomListAndShow();
    }

    public void GetRoomListAndShow() {
      currentRoomList = PhotonNetwork.GetRoomList();
      Debug.Log("The length of the rooms is " + currentRoomList.Length);
      GameObject[] existBtns = GameObject.FindGameObjectsWithTag("roomButtons");
      foreach (GameObject btn in existBtns) {
        Destroy(btn);
      }
      GameObject canvas = GameObject.Find("Canvas");
      for (int i = 0; i < currentRoomList.Length; i++) {
        RoomInfo ri = currentRoomList[i];
        if (!ri.open || !ri.visible) {
          Debug.Log("Room " + ri.Name + " is not open or visible");
          continue;
        }
        GameObject joinRoomBtn = Instantiate(mJoinRoomBtn);
        joinRoomBtn.transform.parent = canvas.transform;
        joinRoomBtn.tag = "roomButtons";
        RectTransform rt = joinRoomBtn.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 300 - i * 100);
        Button btn = joinRoomBtn.GetComponent<Button>();
        string roomName = currentRoomList[i].Name;
        btn.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomName));
        GameObject tc = joinRoomBtn.transform.GetChild(0).gameObject;
        Text btnText = tc.GetComponent<Text>();
        btnText.text = roomName;
      }
    }

    public void CreateNewRoom() {
        bool suc = PhotonNetwork.CreateRoom(null);
        if (!suc) {
          Debug.Log("Create room failed!");
        }
        GetRoomListAndShow();
    }

    // public virtual void OnPhotonRandomJoinFailed()
    // {
    //     Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
    // }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        Room r = PhotonNetwork.room;
        if (r != null) {
          Debug.Log("Joined " + r.Name);
        }
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
