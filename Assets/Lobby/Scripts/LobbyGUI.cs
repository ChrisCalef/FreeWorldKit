using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;

public class LobbyGUI : MonoBehaviour {
	private SmartFox smartFox;
	private bool shuttingDown = false;

	private Vector2 gameScrollPosition, userScrollPosition;
	private ChatWindow chatWindow = null;

	private int roomSelection = -1;
	private string [] roomStrings;

	public GUISkin sfsSkin;
	
	private bool started = false;

	/************
	 * Unity callback methods
	 ************/

	void OnApplicationQuit() {
		shuttingDown = true;
	}
	
	void FixedUpdate() {
		smartFox.ProcessEvents();
	}

	void Awake() {
		Application.runInBackground = true;

		if ( SmartFoxConnection.IsInitialized ) {
			smartFox = SmartFoxConnection.Connection;
		} else {
			Application.LoadLevel("login");
			return;
		}

		// Register callbacks
		smartFox.AddEventListener(SFSEvent.LOGOUT, OnLogout);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
		smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
		smartFox.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnCreateRoomError);
		smartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
		smartFox.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
		smartFox.AddEventListener(SFSEvent.ROOM_REMOVE, OnRoomDeleted);
		
		chatWindow = new ChatWindow();
				
		// Lets update internal API room list that might have changed while we played a game
		SetupRoomList();
		
		started = true;
	}

	void OnGUI() {
		if (!started) return;

		GUI.skin = sfsSkin;
		DrawLobbyGUI();
	}
	
	
	private void DrawLobbyGUI() {
		Room currentActiveRoom = smartFox.LastJoinedRoom;
		if ( currentActiveRoom == null ) {
			// Wait until active room has been set up in the API before drawing anything
			return;
		}
		
		float chatPanelWidth = Screen.width * 3/4 - 10;
		float chatPanelHeight = Screen.height - 80;
		float chatPanelPosX = 10;
		float chatPanelPosY = 10;
				
				
		float roomPanelWidth = Screen.width * 1/4 - 10;
		float roomPanelHeight = Screen.height / 2 - 40 - 5;
		float roomPanelPosX = chatPanelPosX + chatPanelWidth;
		float roomPanelPosY = chatPanelPosY;

		float userPanelWidth = roomPanelWidth;
		float userPanelHeight = roomPanelHeight;
		float userPanelPosX = chatPanelPosX + chatPanelWidth;
		float userPanelPosY = chatPanelPosY + roomPanelHeight + 10;
							
		// Room list
		GUILayout.BeginArea(new Rect(roomPanelPosX, roomPanelPosY, roomPanelWidth, roomPanelHeight));
		GUILayout.Box ("Game List", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		
		if (smartFox.RoomList.Count != 1 ) {
			GUILayout.BeginVertical();
			GUILayout.BeginArea(new Rect(20, 25, roomPanelWidth-40, roomPanelHeight-80), GUI.skin.customStyles[0]);
			// We want some padding between buttons in the grid selection
			GUIStyle selectionStyle = new GUIStyle(GUI.skin.button);
			selectionStyle.margin = new RectOffset(4,4,4,4);
			gameScrollPosition = GUILayout.BeginScrollView (gameScrollPosition);
			roomSelection = GUILayout.SelectionGrid (roomSelection, roomStrings, 1, selectionStyle);
							
			if (roomSelection>=0 && roomStrings[roomSelection] != currentActiveRoom.Name) {
				smartFox.Send(new JoinRoomRequest(roomStrings[roomSelection], null, smartFox.LastJoinedRoom.Id));
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea ();
			GUILayout.EndVertical();
		}
		else {
			GUILayout.BeginVertical();
			GUILayout.BeginArea(new Rect(20, 25, roomPanelWidth-40, roomPanelHeight-80), GUI.skin.customStyles[0]);
			// We always have 1 non-game room - Main Lobby
			GUILayout.Label("No games available to join");	
			GUILayout.EndArea ();
			GUILayout.EndVertical();
		}
				
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();		
		if (GUILayout.Button("New game") ) {
			RoomSettings settings = new RoomSettings(smartFox.MySelf.Name + "'s game");
			settings.GroupId = "games";
			settings.IsGame = true;
			settings.MaxUsers = 4;
			settings.MaxSpectators = 0;
			settings.Extension = new RoomExtension(NetworkManager.ExtName, NetworkManager.ExtClass);
			smartFox.Send(new CreateRoomRequest(settings, true, smartFox.LastJoinedRoom));
		}
		GUILayout.FlexibleSpace();		
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
								
		// User list
		GUILayout.BeginArea(new Rect(userPanelPosX, userPanelPosY, userPanelWidth, userPanelHeight));
		GUILayout.Box ("Users", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.BeginVertical();
		GUILayout.BeginArea(new Rect(20, 25, userPanelWidth-40, userPanelHeight-80), GUI.skin.customStyles[0]);
				
		userScrollPosition = GUILayout.BeginScrollView (userScrollPosition, GUILayout.Width (150), GUILayout.Height (160));
		foreach (User user in currentActiveRoom.UserList) {
			GUILayout.Label(user.Name);
		}
		GUILayout.EndScrollView ();

		GUILayout.EndArea ();
		
		// Current user info
		GUILayout.BeginArea(new Rect(20, 25 + userPanelHeight - 70, userPanelWidth-40, 40));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Logged in as " + smartFox.MySelf.Name);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Logout")) {
			smartFox.Send( new LogoutRequest() );
		}
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea ();
		GUILayout.EndVertical();
		GUILayout.EndArea ();
				
		// Room chat window
		chatWindow.Draw(chatPanelPosX, chatPanelPosY, chatPanelWidth, chatPanelHeight);
	}

	/************
	 * Helper methods
	 ************/

	private void UnregisterSFSSceneCallbacks() {
		// This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
		smartFox.RemoveAllEventListeners();
	}

	private void SetupRoomList() {	
		List<Room> roomList = smartFox.RoomManager.GetRoomList();
		List<string> roomNames = new List<string>();
		foreach (Room room in roomList) {
			// Show only game rooms
			if (!room.IsGame || room.IsHidden || room.IsPasswordProtected) {
				continue;
			}	
			
			roomNames.Add(room.Name);
			Debug.Log("Room id: " + room.Id + " has name: " + room.Name);
				
		}
		
		roomStrings = roomNames.ToArray();
		
		if (smartFox.LastJoinedRoom==null)
			smartFox.Send(new JoinRoomRequest("The Lobby"));
	}

	/************
	 * Callbacks from the SFS API
	 ************/

	void OnLogout(BaseEvent evt) {
		UnregisterSFSSceneCallbacks();
		Application.LoadLevel("login");
	}

	void OnConnectionLost(BaseEvent evt) {
		UnregisterSFSSceneCallbacks();
		if ( shuttingDown == true ) return;
		Application.LoadLevel("login");
	}

	void OnPublicMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		User sender = (User)evt.Params["sender"];
		chatWindow.AddChatMessage(sender.Name + " said " + message);
	}

	void OnJoinRoom(BaseEvent evt) {
		Room room = (Room)evt.Params["room"];
		// If we joined a game room, then we either created it (and auto joined) or manually selected a game to join
		if ( room.IsGame ) {
			started = false;
			Debug.Log("Joining game room " + room.Name);
			UnregisterSFSSceneCallbacks();
			Application.LoadLevel("game");
		}
	}

	public void OnCreateRoomError(BaseEvent evt) {
		string error = (string)evt.Params["error"];
		Debug.Log("Room creation error; the following error occurred: " + error);
	}

	public void OnUserEnterRoom(BaseEvent evt) {
		User user = (User)evt.Params["user"];
		chatWindow.AddPlayerJoinMessage(user.Name + " joined room");
	}

	private void OnUserLeaveRoom(BaseEvent evt) {
		User user = (User)evt.Params["user"];
		chatWindow.AddPlayerLeftMessage(user.Name + " left room");
	}

	/*
* Handle a new room in the room list
*/
	public void OnRoomAdded(BaseEvent evt) { //Room room) {
		Room room = (Room)evt.Params["room"];
		// Update view (only if room is game)
		if ( room.IsGame ) {
			SetupRoomList();
		}
	}

	/*
	* Handle a room that was removed
	*/
	public void OnRoomDeleted(BaseEvent evt) { //Room room) {
		SetupRoomList();
	}

}