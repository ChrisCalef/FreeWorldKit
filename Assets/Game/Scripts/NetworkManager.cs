
using System;
using System.Collections;
using UnityEngine;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Logging;

// The Neywork manager sends the messages to server and handles the response.
public class NetworkManager : MonoBehaviour
{
	private bool running = false;
	
	public readonly static string ExtName = "fps";  // The server extension we work with
	public readonly static string ExtClass = "dk.fullcontrol.fps.FpsExtension"; // The class name of the extension
	
	private static NetworkManager instance;
	public static NetworkManager Instance {
		get {
			return instance;
		}
	}
	
	private SmartFox smartFox;  // The reference to SFS client
	
	void Awake() {
		instance = this;	
	}
	
	void Start() {
		smartFox = SmartFoxConnection.Connection;
		if (smartFox == null) {
			Application.LoadLevel("lobby");
			return;
		}	
		
		SubscribeDelegates();
		SendSpawnRequest();
		
		TimeManager.Instance.Init();
		
		running = true;
	}
			
	// This is needed to handle server events in queued mode
	void FixedUpdate() {
		if (!running) return;
		smartFox.ProcessEvents();
	}
		
	private void SubscribeDelegates() {
		smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
		smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
	}
	
	private void UnsubscribeDelegates() {
		smartFox.RemoveAllEventListeners();
	}
	
	/// <summary>
	/// Send the request to server to spawn my player
	/// </summary>
	public void SendSpawnRequest() {
		Room room = smartFox.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("spawnMe", new SFSObject(), room);
		smartFox.Send(request);
	}
	
	/// <summary>
	///  Send a request to shoot
	/// </summary>
	public void SendShot() {
		Room room = smartFox.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("shot", new SFSObject(), room);
		smartFox.Send(request);
	}
	
	/// <summary>
	/// Send a request to reload
	/// </summary>
	public void SendReload() {
		Room room = smartFox.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("reload", new SFSObject(), room);
		smartFox.Send(request);
	}
	
	/// <summary>
	/// Send local transform to the server
	/// </summary>
	/// <param name="ntransform">
	/// A <see cref="NetworkTransform"/>
	/// </param>
	public void SendTransform(NetworkTransform ntransform) {
		Room room = smartFox.LastJoinedRoom;
		ISFSObject data = new SFSObject();
		ntransform.ToSFSObject(data);
		ExtensionRequest request = new ExtensionRequest("sendTransform", data, room, true); // True flag = UDP
		smartFox.Send(request);
	}
	
	/// <summary>
	/// Send local animation state to the server
	/// </summary>
	/// <param name="message">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="layer">
	/// A <see cref="System.Int32"/>
	/// </param>
	public void SendAnimationState(string message, int layer) {
		Room room = smartFox.LastJoinedRoom;
		ISFSObject data = new SFSObject();
		data.PutUtfString("msg", message);
		data.PutInt("layer", layer);
		ExtensionRequest request = new ExtensionRequest("sendAnim", data, room);
		smartFox.Send(request);
	}
	
	/// <summary>
	/// Request the current server time. Used for time synchronization
	/// </summary>	
	public void TimeSyncRequest() {
		Room room = smartFox.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("getTime", new SFSObject(), room);
		smartFox.Send(request);
	}
	
	/// <summary>
	/// When connection is lost we load the login scene
	/// </summary>
	private void OnConnectionLost(BaseEvent evt) {
		UnsubscribeDelegates();
		Screen.lockCursor = false;
		Screen.showCursor = true;
		Application.LoadLevel("lobby");
	}
	
	// This method handles all the responses from the server
	private void OnExtensionResponse(BaseEvent evt) {
		try {
			string cmd = (string)evt.Params["cmd"];
			ISFSObject dt = (SFSObject)evt.Params["params"];
											
			if (cmd == "spawnPlayer") {
				HandleInstantiatePlayer(dt);
			}
			else if (cmd == "transform") {
				HandleTransform(dt);
			}
			else if (cmd == "notransform") {
				HandleNoTransform(dt);
			}
			else if (cmd == "killed") {
				HandleKill(dt);
			}
			else if (cmd == "health") {
				HandleHealthChange(dt);
			}
			else if (cmd == "anim") {
				HandleAnimation(dt);
			}
			else if (cmd == "score") {
				HandleScoreChange(dt);
			}
			else if (cmd == "ammo") {
				HandleAmmoCountChange(dt);
			}
			else if (cmd == "spawnItem") {
				HandleItem(dt);
			}
			else if (cmd == "removeItem") {
				HandleRemoveItem(dt);
			}
			else if (cmd == "enemyShotFired") {
				HandleShotFired(dt);
			}
			else if (cmd == "time") {
				HandleServerTime(dt);
			}
			else if (cmd=="reloaded") {
				HandleReload(dt);
			}
		}
		catch (Exception e) {
			Debug.Log("Exception handling response: "+e.Message+" >>> "+e.StackTrace);
		}
		
	}
	
	// Instantiating player (our local FPS model, or remote 3rd person model)
	private void HandleInstantiatePlayer(ISFSObject dt) {
		ISFSObject playerData = dt.GetSFSObject("player");
		int userId = playerData.GetInt("id");
		int score = playerData.GetInt("score");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(playerData);
						
		User user = smartFox.UserManager.GetUserById(userId);
		string name = user.Name;
		
		if (userId == smartFox.MySelf.Id) {
			PlayerManager.Instance.SpawnPlayer(ntransform, name, score);
		}
		else {
			PlayerManager.Instance.SpawnEnemy(userId, ntransform, name, score);
		}
	}
	
	// Updating transform of the remote player from server
	private void HandleTransform(ISFSObject dt) {
		int userId = dt.GetInt("id");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(dt);
		if (userId != smartFox.MySelf.Id) {
			// Update transform of the remote user object
		
			NetworkTransformReceiver recipient = PlayerManager.Instance.GetRecipient(userId);
			if (recipient!=null) {
				recipient.ReceiveTransform(ntransform);
			}
		}
	}
	
	// Server rejected transform message - force the local player object to what server said
	private void HandleNoTransform(ISFSObject dt) {
		int userId = dt.GetInt("id");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(dt);
		
		if (userId == smartFox.MySelf.Id) {
			// Movement restricted!
			// Update transform of the local object
			ntransform.Update(PlayerManager.Instance.GetPlayerObject().transform);
		}
	}
	
	// Synchronize the time from server
	private void HandleServerTime(ISFSObject dt) {
		long time = dt.GetLong("t");
		TimeManager.Instance.Synchronize(Convert.ToDouble(time));
	}
	
	// Handle player killed
	private void HandleKill(ISFSObject dt) {
		int userId = dt.GetInt("id");
		int killerId = dt.GetInt("killerId");
		
		if (userId != smartFox.MySelf.Id) {
			PlayerManager.Instance.KillEnemy(userId);
			if (killerId == smartFox.MySelf.Id) {
				SoundManager.Instance.PlayKillEnemy(PlayerManager.Instance.GetPlayerObject().audio);
			}
		}
		else {
			PlayerManager.Instance.KillMe();
		}
	}
	
	// Health of the player changed - updating GUI and playing sounds if it's damage
	private void HandleHealthChange(ISFSObject dt) {
		int userId = dt.GetInt("id");
		int health = dt.GetInt("health");
		if (userId == smartFox.MySelf.Id) {
			if (health<GameHUD.Instance.Health) {
				SoundManager.Instance.PlayDamage(PlayerManager.Instance.GetPlayerObject().audio);
			}
			
			GameHUD.Instance.UpdateHealth(health);
		}
		else {
			PlayerManager.Instance.UpdateHealthForEnemy(userId, health);
		}
	}
	
	// Ammo count changed message from server
	private void HandleAmmoCountChange(ISFSObject dt) {
		int userId = dt.GetInt("id");
		
		if (userId != smartFox.MySelf.Id) return;
		
		int loadedAmmo = dt.GetInt("ammo");
		int maxAmmo = dt.GetInt("maxAmmo");
		int ammo = dt.GetInt("unloadedAmmo");
				
		ShotController.Instance.UpdateAmmoCount(loadedAmmo, maxAmmo, ammo);
	}
	
	// Score changed message from server
	private void HandleScoreChange(ISFSObject dt) {
		int userId = dt.GetInt("id");
		int score = dt.GetInt("score");
		
		User user = smartFox.UserManager.GetUserById(userId);
		if (user!=null) {
			string name = user.Name;
			PlayerScore.Instance.SetScore(name, score);
		}
	}
	
	// New item spawned message. Instantiating the item object.
	private void HandleItem(ISFSObject dt) {
		ISFSObject item = dt.GetSFSObject("item");
		int id = item.GetInt("id");
		string itemType = item.GetUtfString("type");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(item);
		
		PlayerManager.Instance.SpawnItem(id, ntransform, itemType);
	}
	
	// Removing item (hwen it was picked up by someone)
	private void HandleRemoveItem(ISFSObject dt) {
		int playerId = dt.GetInt("playerId");
		ISFSObject item = dt.GetSFSObject("item");
		int id = item.GetInt("id");
		string type = item.GetUtfString("type");
		if (playerId == smartFox.MySelf.Id) {
			if (type == "HealthPack") {
				SoundManager.Instance.PlayPickupHealthPack(PlayerManager.Instance.GetPlayerObject().audio);				
			} 
			else if (type == "Ammo") {
				SoundManager.Instance.PlayPickupAmmo(PlayerManager.Instance.GetPlayerObject().audio);							
			}
		}
		
		PlayerManager.Instance.RemoveItem(id);
	}
	
	// Synchronizing remote animation
	private void HandleAnimation(ISFSObject dt) {
		int userId = dt.GetInt("id");
		string msg = dt.GetUtfString("msg");
		int layer = dt.GetInt("layer");
		
		if (userId != smartFox.MySelf.Id) {
			PlayerManager.Instance.SyncAnimation(userId, msg, layer);
		}
	}
	
	// When someon shots handle it and play corresponding animation 
	private void HandleShotFired(ISFSObject dt) {
		int userId = dt.GetInt("id");
		if (userId != smartFox.MySelf.Id) {
			SoundManager.Instance.PlayShot(PlayerManager.Instance.GetRecipient(userId).audio);
			PlayerManager.Instance.SyncAnimation(userId, "Shot", 1);
		}
		else {
			GameObject obj = PlayerManager.Instance.GetPlayerObject();
			if (obj == null) return;
						
			SoundManager.Instance.PlayShot(obj.audio);
			obj.GetComponent<AnimationSynchronizer>().PlayShotAnimation();
			
			PlayerManager.Instance.ShotEffect();
		}
	}
	
	// When someone reloaded the weapon - play corresponding animation
	private void HandleReload(ISFSObject dt) {
		int userId = dt.GetInt("id");
		if (userId != smartFox.MySelf.Id) {
			SoundManager.Instance.PlayReload(PlayerManager.Instance.GetRecipient(userId).audio);
			PlayerManager.Instance.SyncAnimation(userId, "Reload", 1);
		}
		else {
			GameObject obj = PlayerManager.Instance.GetPlayerObject();
			if (obj == null) return;
			
			SoundManager.Instance.PlayReload(obj.audio);
			obj.GetComponent<AnimationSynchronizer>().PlayReloadAnimation();
		}
	}

	// When a user leaves room destroy his object
	private void OnUserLeaveRoom(BaseEvent evt) {
		User user = (User)evt.Params["user"];
		Room room = (Room)evt.Params["room"];
				
		PlayerManager.Instance.DestroyEnemy(user.Id);
		Debug.Log("User "+user.Name+" left");
	}
	
	void OnApplicationQuit() {
		UnsubscribeDelegates();
	}
}
