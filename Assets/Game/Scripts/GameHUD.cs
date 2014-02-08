
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ingame GUI class 
public class GameHUD : MonoBehaviour {
	private readonly float respawnDelay = 3f;
	
	public ProgressBar playerLifeBar;
	public GUIStyle healthTextStyle;
	public GUIStyle ammoTextStyle;
			
	private float bloodOverlayAmount = 0.0f;	
	public Renderer bloodOverlay;
	
	
	
	private static GameHUD instance;
	public static GameHUD Instance {
		get {
			return instance;
		}
	}
	
	public int Health {
		get {
			return health;
		}
	}
		
	private int health = 0;
	
	private bool deadState = false;
	
	void Awake() {
		instance = this;
		Application.runInBackground = true;
	}
	
	void Start() {
		LockAndHideCursor();
	}
	
	public void LockAndHideCursor() {
		Screen.showCursor = false;
		Screen.lockCursor = true;
	}
	
	void OnGUI() {
		// GUI.Label(new Rect(10, 10, 300, 20), "RMB - reload");
		
		if (TimeManager.Instance == null) return;
		GUI.Label(new Rect(10, 10, 300, 20), "Time: "+TimeManager.Instance.NetworkTime);
		GUI.Label(new Rect(10, 30, 300, 20), "Ping: "+TimeManager.Instance.AveragePing);
		
			
		GUI.Label(new Rect(Screen.width-160, 30, 150, 20), "Score list: ");
		int pos = 50;
		foreach (KeyValuePair<string, int> score in PlayerScore.Instance.Scores) {
			GUI.Label(new Rect(Screen.width-160, pos, 300, 20), score.Key+" --> "+score.Value);
			pos +=20;
		}
				
		if (health > 0) {
			GUI.Label(new Rect(80, Screen.height-20, 300, 20), ""+health, healthTextStyle);
			GUI.Label(new Rect(Screen.width - 142, Screen.height-50, 300, 20), ShotController.Instance.GetAmmoCountString(), ammoTextStyle);		
		}
		else if (deadState) {
			GUI.Label(new Rect(Screen.width/2-25, Screen.height/2-10, 50, 20), "DEAD");			
			GUI.Label(new Rect(Screen.width/2-90, Screen.height/2+15, 180, 20), "Press Fire button to respawn!");			
		}
		else {
			GUI.Label(new Rect(Screen.width/2-25, Screen.height/2-10, 50, 20), "DEAD");			
			GUI.Label(new Rect(Screen.width/2-75, Screen.height/2+15, 150, 20), "Wait for respawn counter");			
		}
	}
	
	
	void Update() {		
		if (bloodOverlayAmount > 0.0f) {
			bloodOverlay.material.SetColor ("_TintColor", new Color (0.31f, 0.31f, 0.31f, bloodOverlayAmount));
			bloodOverlayAmount -= Time.deltaTime;
		}
		else {
			bloodOverlay.material.SetColor ("_TintColor", new Color (0.31f, 0.31f, 0.31f, 0.0f));
		}
			
		
		//if (Input.GetButton("Escape")) {
		//	Screen.showCursor = true;
		//	Screen.lockCursor = false;
		//} 
		//else 
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown(1)) {
			LockAndHideCursor();
		}
		
		
		
		if (deadState && Input.GetMouseButtonDown(0)) {
			deadState = false;
			NetworkManager.Instance.SendSpawnRequest();
		}
	}
		
	public void UpdateHealth(int val) {
		if (val < health) {
			bloodOverlayAmount = 1.0f;
		}
		
		health = val;
		playerLifeBar.SetValue(health/100.0f);
			

		
		if (health <= 0) {
			StartCoroutine(SetDeadState());
		}
		else {
			deadState = false;
		}
	}
	
	private IEnumerator SetDeadState() {
		yield return new WaitForSeconds(respawnDelay);
		deadState = true;
	}
		
	
}

