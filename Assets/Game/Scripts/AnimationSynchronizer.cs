using UnityEngine;
using System;
using System.Collections;

// This class plays FPS animation and synchronizes animation to remote clients
public class AnimationSynchronizer : MonoBehaviour {
	
	public Animation anim; // Animation of the hero (for remote instances)
	public Animation fpsAnim; // Animation of the fps hands model (for local instances)
	
	public enum BaseAnimationState {
		Idle,
		RunForward,
		RunBackward
	}
	
	public enum SecondAnimationState {
		None,
		Shot,
		Reload
	}
	
	private BaseAnimationState baseState = BaseAnimationState.Idle; // Base layer state for moving, idle, jumping
	private SecondAnimationState secondState = SecondAnimationState.None; // Second layer state for shoot and reload
		
	// We call it on remote player model to start receiving anim messages
	public void StartReceivingAnimation() {
		InitAnimations();
		UpdateAnimation();
	}
	
	// Initializing animations parameters
	private void InitAnimations() {
		anim["Idle"].wrapMode = WrapMode.Loop;
		anim["Idle"].blendMode = AnimationBlendMode.Additive;
		anim["Idle"].layer = 1;
			
		anim["RunForward"].wrapMode = WrapMode.Loop;
		anim["RunForward"].blendMode = AnimationBlendMode.Additive;
		anim["RunForward"].layer = 1;
			
		anim["RunBackward"].wrapMode = WrapMode.Loop;
		anim["RunBackward"].blendMode = AnimationBlendMode.Additive;
		anim["RunBackward"].layer = 1;
				
		anim["AimStraight"].layer = 5;
		anim["AimStraight"].weight=1.0f;
		anim["AimStraight"].wrapMode = WrapMode.Loop;
		anim["AimStraight"].blendMode = AnimationBlendMode.Blend;
		anim.Play("AimStraight");
				
		anim["GunReload"].wrapMode = WrapMode.Once;
		anim["GunReload"].layer = 2;
		anim["GunReload"].weight = 1.0f;
		anim["GunReload"].blendMode = AnimationBlendMode.Additive;		
		
		anim["FireGun"].wrapMode = WrapMode.Once;
		anim["FireGun"].layer = 2;
		anim["FireGun"].weight = 1.0f;
		anim["FireGun"].blendMode = AnimationBlendMode.Additive;		
			
	}
	
	// Updating played animation based on state
	private void UpdateAnimation() {
		if (baseState == BaseAnimationState.Idle) {
			if (!anim.IsPlaying("Idle")) {
				anim.Play("Idle");
			}
		}
		
		if (baseState == BaseAnimationState.RunForward) {
			if (!anim.IsPlaying("RunForward")) {
				anim.Play("RunForward");
			}
		}
		
		if (baseState == BaseAnimationState.RunBackward) {
			if (!anim.IsPlaying("RunBackward")) {
				anim.Play("RunBackward");
			}
		}
				
		if (secondState == SecondAnimationState.Shot) {
			if (!anim.IsPlaying("FireGun")) {
				anim.Play("FireGun");
				StartCoroutine(ResetSecondState(0.5f));
			}
		}
		else if (secondState == SecondAnimationState.Reload) {
			if (!anim.IsPlaying("GunReload")) {
				anim.Play("GunReload");
				StartCoroutine(ResetSecondState(0.5f));
			}
		}
					
	}
	
	// Automatically reset reload/shoot state after plating
	private IEnumerator ResetSecondState(float t) {
		yield return new WaitForSeconds(t);
		secondState = SecondAnimationState.None;
	}

	// Updating second state from server
	public void RemoteSecondStateUpdate(string message) {
		SecondAnimationState state = (SecondAnimationState)Enum.Parse(typeof(AnimationSynchronizer.SecondAnimationState), message);
		this.secondState = state;
		UpdateAnimation();
	}
	
	// Updating first state from server
	public void RemoteStateUpdate(string message) {
		BaseAnimationState state = (BaseAnimationState)Enum.Parse(typeof(AnimationSynchronizer.BaseAnimationState), message);
		this.baseState = state;
		UpdateAnimation();
	}	
	
	// Settin first state
	void SetAnimationState(BaseAnimationState state) {
		if (state!=this.baseState) {
			this.baseState = state;
			NetworkManager.Instance.SendAnimationState(state.ToString(), 0);
		}
	}
	
	public void PlayIdle() {
		SetAnimationState(BaseAnimationState.Idle);
	}
	
	public void PlayRunForward() {
		SetAnimationState(BaseAnimationState.RunForward);
	}
	
	public void PlayRunBackward() {
		SetAnimationState(BaseAnimationState.RunBackward);
	}
		
	// Playing shot animation on FPS model
	public void PlayShotAnimation ()
	{
		fpsAnim.Play("fire");
	}
	
	// Playing reload animation on FPS model
	public void PlayReloadAnimation ()
	{
		fpsAnim.Play("reload");
	}
		
}
