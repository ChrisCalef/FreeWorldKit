
using System;
using UnityEngine;

// Sound manager - controls all the audio in the game
public class SoundManager : MonoBehaviour {
	
	private static SoundManager instance;
	public static SoundManager Instance {
		get {
			return instance;
		}
	}
	
	public AudioClip shotSound;
	public AudioClip reloadSound;

	public AudioClip damageSound;
	public AudioClip deathSound;
	public AudioClip killEnemySound;
	
	public AudioClip pickupAmmoSound;
	public AudioClip pickupHealthPackSound;
	
	
	void Awake() {
		instance = this;
	}
		
	public void PlayShot(AudioSource src) {
		src.PlayOneShot(shotSound);
	}
	
	public void PlayReload(AudioSource src) {
		src.PlayOneShot(reloadSound);
	}

	public void PlayDamage(AudioSource src) {
		src.PlayOneShot(damageSound);
	}

	public void PlayDeath(AudioSource src) {
		src.PlayOneShot(deathSound);
	}

	public void PlayKillEnemy(AudioSource src) {
		src.PlayOneShot(killEnemySound);
	}

	public void PlayPickupAmmo(AudioSource src) {
		src.PlayOneShot(pickupAmmoSound);
	}

	public void PlayPickupHealthPack(AudioSource src) {
		src.PlayOneShot(pickupHealthPackSound);
	}
}

