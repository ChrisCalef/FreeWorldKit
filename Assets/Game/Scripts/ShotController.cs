using UnityEngine;
using System.Collections;

// Performs shooting
public class ShotController : MonoBehaviour {

	private static ShotController instance;
	public static ShotController Instance {
		get {
			return instance;
		}
	}
			
	private int loadedAmmo = 0;
	private int maxAmmo = 0;
	private int ammo = 0;
		
	public string GetAmmoCountString() {
		return loadedAmmo +" ["+ammo+"]";
	}
	
	void Awake() {
		instance = this;
	}
		
	// Update is called once per frame
	void Update () {
		if (loadedAmmo > 0 && Input.GetMouseButtonDown(0)) {
			DoShot();
		}
		else if (Input.GetMouseButtonDown(1)) {
			Reload();
		}
		
		CheckRaycastWithEnemy();
		
	}
	
	private void DoShot() {
		NetworkManager.Instance.SendShot();
	}
	
	private void Reload() {
		NetworkManager.Instance.SendReload();
	}
	
	public void UpdateAmmoCount(int loadedAmmo, int maxAmmo, int ammo) {
		this.loadedAmmo = loadedAmmo;
		this.ammo = ammo;
		this.maxAmmo = maxAmmo;
	}
	
	/// <summary>
	/// This method checks raycast with enemy to display the information about him
	/// </summary>
	private void CheckRaycastWithEnemy() {
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1<<(int)GameLayers.TargetLayer)) {
			hit.collider.SendMessage("RaycastMessage", SendMessageOptions.DontRequireReceiver);
			
		}
	}
		
}
