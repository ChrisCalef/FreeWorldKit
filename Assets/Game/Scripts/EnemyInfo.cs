
using System;
using System.Collections;
using UnityEngine;

// Displaying enemy info like name and health
public class EnemyInfo : MonoBehaviour
{

	public ProgressBar lifeBar;
	public TextMesh name;
	
	private Renderer[] renderers;
	
	void Awake() {
		renderers = this.GetComponentsInChildren<Renderer>();
	}
	
	public void SetName(string name) {
		this.name.text = name;
	}
	
	public void SetLife(float val) {
		lifeBar.SetValue(val);
	}
	
	public void Hide() {
		foreach (Renderer rend in renderers) {
			rend.enabled = false;
		}
	}
	
	public void Show() {
		foreach (Renderer rend in renderers) {
			rend.enabled = true;
		}
	}
	
	void LateUpdate() {
		transform.LookAt(Camera.main.transform);
	}
	
}

