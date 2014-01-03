
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns player and items objects, stores them in collections and provides access to them
public class PlayerManager : MonoBehaviour {
	
	public GameObject enemyPrefab;
	public GameObject playerPrefab;
	public GameObject ammoPrefab;
	public GameObject healthPrefab;
	
	public GameObject sparkPrefab;
	public GameObject bloodPrefab;
	
	
	private GameObject playerObj;
	
	private static PlayerManager instance;
	public static PlayerManager Instance {
		get {
			return instance;
		}
	}
	
	public GameObject GetPlayerObject() {
		return playerObj;
	}
	
	private Dictionary<int, NetworkTransformReceiver> recipients = new Dictionary<int, NetworkTransformReceiver>();
	private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();
		
	void Awake() {
		instance = this;
	}

	public void SpawnItem(int id, NetworkTransform ntransform, string itemType) {
		GameObject itemPrefab = null;
		
		if (itemType == "Ammo") {
			itemPrefab = ammoPrefab;
		}
		else {
			itemPrefab = healthPrefab;
		}
		
		GameObject itemObj = GameObject.Instantiate(itemPrefab) as GameObject;
		itemObj.transform.position = ntransform.Position;
		itemObj.transform.localEulerAngles = ntransform.AngleRotationFPS;
		items[id] = itemObj;
	}
	
	public void RemoveItem(int id) {
		if (items.ContainsKey(id)) {
			Destroy(items[id]);
			items.Remove(id);
		}
	}

	public void SpawnPlayer(NetworkTransform ntransform, string name, int score) {
		if (Camera.main!=null) {
			Destroy(Camera.main.gameObject);
		}
		
		GameHUD.Instance.UpdateHealth(100);
		playerObj = GameObject.Instantiate(playerPrefab) as GameObject;
		playerObj.transform.position = ntransform.Position;
		playerObj.transform.localEulerAngles = ntransform.AngleRotationFPS;
		playerObj.SendMessage("StartSendTransform");
				
		PlayerScore.Instance.SetScore(name, score);
	}
	
	public void SpawnEnemy(int id, NetworkTransform ntransform, string name, int score) {
		GameObject playerObj = GameObject.Instantiate(enemyPrefab) as GameObject;
		playerObj.transform.position = ntransform.Position;
		playerObj.transform.localEulerAngles = ntransform.AngleRotationFPS;
		AnimationSynchronizer animator = playerObj.GetComponent<AnimationSynchronizer>();
		animator.StartReceivingAnimation();
				
		PlayerScore.Instance.SetScore(name, score);
		
		Enemy enemy = playerObj.GetComponent<Enemy>();
		enemy.Init(name);
		
		recipients[id] = playerObj.GetComponent<NetworkTransformReceiver>();
	}
	
	public NetworkTransformReceiver GetRecipient(int id) {
		if (recipients.ContainsKey(id)) {
			return recipients[id];
		}
		return null;
	}
	
	public void UpdateHealthForEnemy(int id, int health) {
		NetworkTransformReceiver rec = GetRecipient(id);
		rec.GetComponent<Enemy>().UpdateHealth(health);
		
		BloodEffect(rec.transform);

	}
	
	public void DestroyEnemy(int id) {
		NetworkTransformReceiver rec = GetRecipient(id);
		if (rec == null) return;
		Destroy(rec.gameObject);
		recipients.Remove(id);
	}
	
	public void SyncAnimation(int id, string msg, int layer) {
		NetworkTransformReceiver rec = GetRecipient(id);
		
		if (rec == null) return;
		
		if (layer == 0) {
			rec.GetComponent<AnimationSynchronizer>().RemoteStateUpdate(msg);
		}
		else if (layer == 1) {
			rec.GetComponent<AnimationSynchronizer>().RemoteSecondStateUpdate(msg);
		}
	}
	
	public void KillEnemy(int id) {
		NetworkTransformReceiver rec = GetRecipient(id);
		if (rec == null) return;
		GameObject obj = rec.gameObject;
		
		BloodEffect (obj.transform);
		
		GameObject hero = obj.transform.FindChild("Hero").gameObject;
		hero.transform.parent = null;
		Destroy(obj);
		hero.transform.Rotate(Vector3.right*90);
		hero.animation.Stop();
		Destroy(hero, 10);
		
		recipients.Remove(id);
	}
	
	public void KillMe() {
		GameHUD.Instance.UpdateHealth(0);
		if (playerObj == null) return;
		Camera.main.transform.parent = null;
		Destroy(playerObj);
		playerObj = null;
	}
		
	public void BloodEffect (Transform t) {
			GameObject blood = GameObject.Instantiate(bloodPrefab) as GameObject;
			blood.transform.position = t.position;
			blood.transform.rotation = Quaternion.LookRotation(playerObj.transform.position - t.position);
	}
	
	public void ShotEffect() {
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			if (hit.transform.gameObject.layer != (int)GameLayers.TargetLayer) {  // miss			
				GameObject spark = GameObject.Instantiate(sparkPrefab) as GameObject;
				spark.transform.position = hit.point;
				spark.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
		}				
	}
}

