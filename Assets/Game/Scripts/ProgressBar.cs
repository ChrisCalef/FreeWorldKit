using UnityEngine;
using System;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public GameObject lifeMeterFill;
	public Transform leftTransform;
	public Transform rightTransform;
	
	private int axis = 0;	
	private float leftPos;
	private float rightPos;
	
	private float currentValue = 1;
			
	private Renderer fillRenderer;
	private Transform thisTransform;
	private Transform fillTransform;
			
	private bool isEnabled = true;
	
	void Awake() {
		fillRenderer = lifeMeterFill.renderer; 
		fillTransform = lifeMeterFill.transform;
		leftPos = leftTransform.localPosition[axis];
		rightPos = rightTransform.localPosition[axis];
	}

	public void SetValue(float f) {
		if (!isEnabled) return;
		
		f = Mathf.Clamp(f, 0, 1);
		
		Vector3 lp = fillTransform.localPosition;
		lp[axis] = Mathf.Lerp(leftPos, rightPos, f) + (1-f)/2;
		fillTransform.localPosition = lp;
		
		Vector3 sc = fillTransform.localScale;
		sc[axis] = f;
		fillTransform.localScale = sc;
		
		currentValue = f;
	}
	
	public void Enable() {
		isEnabled = true;
		this.gameObject.SetActiveRecursively(true);
	}
	
	public void Disable() {
		isEnabled = false;
		this.gameObject.SetActiveRecursively(false);
	}
	
}
