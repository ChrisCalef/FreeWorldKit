using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Logging;

public class LoginGUI : MonoBehaviour {
	private SmartFox smartFox;
	private bool shuttingDown = false;

	public string serverName = "127.0.0.1";
	public int serverPort = 9933;
	public string zone = "BasicExamples";
	public bool debug = true;

	public GUISkin sfsSkin;

	private string username = "";
	private string loginErrorMessage = "";

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
		
		// In a webplayer (or editor in webplayer mode) we need to setup security policy negotiation with the server first
		if (Application.isWebPlayer || Application.isEditor) {
			if (!Security.PrefetchSocketPolicy(serverName, serverPort, 500)) {
				Debug.LogError("Security Exception. Policy file load failed!");
			}
		}		

		if (SmartFoxConnection.IsInitialized) {
			smartFox = SmartFoxConnection.Connection;
		} else {
			smartFox = new SmartFox(debug);
		}

		// Register callback delegate
		smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
		smartFox.AddEventListener(SFSEvent.UDP_INIT, OnUdpInit);

		smartFox.AddLogListener(LogLevel.DEBUG, OnDebugMessage);

		smartFox.Connect(serverName, serverPort);
	}

	void OnGUI() {
		GUI.skin = sfsSkin;
			
		// Determine which state we are in and show the GUI accordingly
		if (smartFox.IsConnected) {
			DrawLoginGUI();
		}
		else {
			string message = "Waiting for connection";
			if (loginErrorMessage != "") {
				message = "Connection error. "+loginErrorMessage;
			}
			DrawMessagePanelGUI(message);
		}
	}
	
	/************
	 * GUI methods
	 ************/
	
	// Generic single message panel
	void DrawMessagePanelGUI(string message) {
		// Lets just quickly set up some GUI layout variables
		float panelWidth = 400;
		float panelHeight = 300;
		float panelPosX = Screen.width/2 - panelWidth/2;
		float panelPosY = Screen.height/2 - panelHeight/2;
		
		// Draw the box
		GUILayout.BeginArea(new Rect(panelPosX, panelPosY, panelWidth, panelHeight));
		GUILayout.Box ("FPS Example", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.BeginVertical();
		GUILayout.BeginArea(new Rect(20, 25, panelWidth-40, panelHeight-60), GUI.skin.customStyles[0]);
		
		// Center label
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
			
		GUILayout.Label(message);
			
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea ();		
			
		GUILayout.EndVertical();
		GUILayout.EndArea ();		
	}
	
	// Login GUI allowing for username, password and zone selection
	private void DrawLoginGUI() {
		// Lets just quickly set up some GUI layout variables
		float panelWidth = 400;
		float panelHeight = 300;
		float panelPosX = Screen.width/2 - panelWidth/2;
		float panelPosY = Screen.height/2 - panelHeight/2;
		
		// Draw the box
		GUILayout.BeginArea(new Rect(panelPosX, panelPosY, panelWidth, panelHeight));
		GUILayout.Box ("Login", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.BeginVertical();
		GUILayout.BeginArea(new Rect(20, 25, panelWidth-40, panelHeight-60), GUI.skin.customStyles[0]);
		
		// Lets show login box!
		GUILayout.FlexibleSpace();
				
		GUILayout.BeginHorizontal();
		GUILayout.Label("Username: ");
		username = GUILayout.TextField(username, 25, GUILayout.MinWidth(200));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Server Name: ");
		serverName = GUILayout.TextField(serverName, 25, GUILayout.MinWidth(200));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Server Port: ");
		serverPort = int.Parse(GUILayout.TextField(serverPort.ToString(), 4, GUILayout.MinWidth(200)));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Invert Mouse Y: ");
		OptionsManager.InvertMouseY = GUILayout.Toggle(OptionsManager.InvertMouseY, "");
		GUILayout.EndHorizontal();
		
		GUILayout.Label(loginErrorMessage);
			
		// Center login button
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();		
		if (GUILayout.Button("Login")  || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
			Debug.Log("Sending login request");
			smartFox.Send(new LoginRequest(username, "", zone));
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		
		GUILayout.EndArea ();		
				
		GUILayout.EndVertical();
		GUILayout.EndArea ();		
	}

	/************
	 * Helper methods
	 ************/

	private void UnregisterSFSSceneCallbacks() {
		// This should be called when switching scenes, so callbacks from the backend do not trigger code in this scene
		smartFox.RemoveAllEventListeners();
	}

	/************
	 * Callbacks from the SFS API
	 ************/

	public void OnConnection(BaseEvent evt) {
		bool success = (bool)evt.Params["success"];
		string error = (string)evt.Params["error"];
		
		Debug.Log("On Connection callback got: " + success + " (error : <" + error + ">)");
		
		loginErrorMessage = "";
		if (success) {
			SmartFoxConnection.Connection = smartFox;
		} else {
			loginErrorMessage = error;
		}
	}

	public void OnConnectionLost(BaseEvent evt) {
		loginErrorMessage = "Connection lost / no connection to server";
		UnregisterSFSSceneCallbacks();
	}

	public void OnDebugMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		Debug.Log("[SFS DEBUG] " + message);
	}

	public void OnLogin(BaseEvent evt) {
		bool success = true;
		if (evt.Params.ContainsKey("success") && !(bool)evt.Params["success"]) {
			// Login failed - lets display the error message sent to us
			loginErrorMessage = (string)evt.Params["errorMessage"];
			Debug.Log("Login error: "+loginErrorMessage);
		} else {
			// Startup up UDP
			Debug.Log("Login ok");
			smartFox.InitUDP(serverName, serverPort);
		}			
	}

	public void OnUdpInit(BaseEvent evt) {
		if (evt.Params.ContainsKey("success") && !(bool)evt.Params["success"]) {
			loginErrorMessage = (string)evt.Params["errorMessage"];
			Debug.Log("UDP error: "+loginErrorMessage);
		} else {
			Debug.Log("UDP ok");
		
			// On to the lobby
			loginErrorMessage = "";
			UnregisterSFSSceneCallbacks();
			Application.LoadLevel("lobby");
		}
	}
}