using System.Collections;
using System.Collections.Generic;
using ExitGames.UtilityScripts;
using Photon;
using UnityEngine;

public class PlayerInRoomGUI : PunBehaviour
{
    /// <summary>
    /// Defines the available colors per room. There should be at least one color per available player spot.
    /// </summary>
    public Color[] Colors = new Color[] { Color.blue, Color.yellow, Color.green };

    /// <summary>
    /// Property-key for Player Color. the value will be the index of the player's color in array Colors (0...)
    /// </summary>
    public const string ColorProp = "pc";

    public bool ShowColorLabel;
    public Rect ColorLabelArea = new Rect(0, 50, 100, 200);
    public Texture2D img;

    /// <summary>
    /// Color this player selected. Defaults to grey.
    /// </summary>
    public Color MyColor = Color.grey;

    public bool ColorPicked { get; set; }

	// we need to reach the PlayerRoomindexing Component. So for safe initialization, we avoid having to mess with script execution order
	bool isInitialized;

	void OnEnable()
	{
		if (!isInitialized)
		{
			Init();
		}
	}

	void Start()
	{
		if (!isInitialized)
		{
			Init();
		}
	}

	void Init()
	{
		if (!isInitialized && PlayerRoomIndexing.instance!=null)
		{
			PlayerRoomIndexing.instance.OnRoomIndexingChanged += Refresh;
			isInitialized = true;
		}
	}


	void OnDisable()
	{
		PlayerRoomIndexing.instance.OnRoomIndexingChanged -= Refresh;
	}

	void Refresh()
	{
		int _index = PhotonNetwork.player.GetRoomIndex();
		if (_index == -1)
		{
			this.Reset();
		}else{
			this.MyColor = this.Colors[_index];
			this.ColorPicked = true;
		}

	}

	public override void OnJoinedRoom()
	{
		if (!isInitialized)
		{
			Init();
		}
	}

    public override void OnLeftRoom()
    {
        // colors are select per room.
        this.Reset();
    }

 
	/// <summary>
    /// Resets the color locally. In this class and the PhotonNetwork.player instance.
    /// </summary>
    public void Reset()
    {
        this.MyColor = Color.grey;
        this.ColorPicked = false;	
    }


    // simple UI to show color
    private void OnGUI()
    {
	    
	    if (!this.ColorPicked || !this.ShowColorLabel)
	    {
		    return;
	    }
	    GUIStyle style1 = new GUIStyle();
	    style1.fontSize = 25;
	    style1.normal.textColor = Color.white;
	    
	    GUILayout.Label("\n Number of players: " + PhotonNetwork.room.PlayerCount, style1);
	    GUILayout.Label("");
	    GUILayout.Label("");
	    GUILayout.Label("");
	    GUILayout.Label("");
        
	    GUILayout.BeginArea(this.ColorLabelArea);
		
	    GUILayout.BeginHorizontal();
	    Color c = GUI.color;
	    GUI.color = this.MyColor;
	    GUILayout.Label(this.img);
	    GUI.color = c;

	    GUIStyle style2 = new GUIStyle();
	    style2.fontSize = 20;
	    style2.normal.textColor = this.MyColor;

	    GUILayout.Label("You join \n", style2);
	    GUILayout.Label(" ", style2);
	    
	    foreach (var player in PhotonNetwork.otherPlayers)
	    {
		    GUI.color = Color.red;
		    GUILayout.Label(this.img);
		    GUI.color = c;
		    style2.normal.textColor = Color.red;
		    GUILayout.Label("Player2", style2);
	    }
	    //string playerNote = " In room";
	    //GUILayout.Label(playerNote, style2);
	    GUILayout.EndHorizontal();
		
	    GUILayout.EndArea();
    }
}