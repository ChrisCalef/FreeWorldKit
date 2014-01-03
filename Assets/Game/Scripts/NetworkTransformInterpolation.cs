
using System;
using System.Collections;
using UnityEngine;

// This script is used to interpolate and predict values to make the position smooth
// And correspond to the real one.
public class NetworkTransformInterpolation : MonoBehaviour
{
	public enum InterpolationMode {
		INTERPOLATION,
		EXTRAPOLATION
	}
	
	public InterpolationMode mode = InterpolationMode.INTERPOLATION;
	
	private double interpolationBackTime = 200; 
	
	// The maximum time we try to extrapolate
	private float extrapolationForwardTime = 1000; // Can make this depend on ping if needed
		
	private bool running = false;
	
	// We store twenty states with "playback" information
	NetworkTransform[] bufferedStates = new NetworkTransform[20];
	// Keep track of what slots are used
	int statesCount = 0;
	
	// We call it on remote player to start receiving his transform
	public void StartReceiving() {
		running = true;
	}
	
	public void ReceivedTransform(NetworkTransform ntransform) {
		if (!running) return;
		
		// When receiving, buffer the information
		// Receive latest state information
		Vector3 pos = ntransform.Position;
		Quaternion rot = ntransform.Rotation;
						
		// Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
		for ( int i=bufferedStates.Length-1;i>=1;i-- ) {
			bufferedStates[i] = bufferedStates[i-1];
		}
		  
		// Save currect received state as 0 in the buffer, safe to overwrite after shifting
		bufferedStates[0] = ntransform;
		
		// Increment state count but never exceed buffer size
		statesCount = Mathf.Min(statesCount + 1, bufferedStates.Length);

		// Check integrity, lowest numbered state in the buffer is newest and so on
		for (int i=0; i<statesCount-1; i++) {
			if (bufferedStates[i].TimeStamp < bufferedStates[i+1].TimeStamp) {
				Debug.Log("State inconsistent");
			}
		}
	}
	

	// This only runs where the component is enabled, which is only on remote peers (server/clients)
	void Update() {
		if (!running) return;
		if (statesCount == 0) return;
		
		UpdateValues();
		
		double currentTime = TimeManager.Instance.NetworkTime;
		double interpolationTime = currentTime - interpolationBackTime;
		
		// We have a window of interpolationBackTime where we basically play 
		// By having interpolationBackTime the average ping, you will usually use interpolation.
		// And only if no more data arrives we will use extrapolation
		
		// Use interpolation
		// Check if latest state exceeds interpolation time, if this is the case then
		// it is too old and extrapolation should be used
		if (mode == InterpolationMode.INTERPOLATION && bufferedStates[0].TimeStamp > interpolationTime) {
			for (int i=0;i < statesCount; i++) {
				// Find the state which matches the interpolation time (time+0.1) or use last state
				if (bufferedStates[i].TimeStamp <= interpolationTime || i == statesCount-1) {
					// The state one slot newer (<100ms) than the best playback state
					NetworkTransform rhs = bufferedStates[Mathf.Max( i-1, 0 )];
					// The best playback state (closest to 100 ms old (default time))
					NetworkTransform lhs = bufferedStates[i];
					
					// Use the time between the two slots to determine if interpolation is necessary
					double length = rhs.TimeStamp - lhs.TimeStamp;
					float t = 0.0F;
					// As the time difference gets closer to 100 ms t gets closer to 1 in 
					// which case rhs is only used
					if (length > 0.0001) {
						t = (float)((interpolationTime - lhs.TimeStamp) / length);
					}
					
					// if t=0 => lhs is used directly
					transform.position = Vector3.Lerp(lhs.Position, rhs.Position, t);
					transform.rotation = Quaternion.Slerp(lhs.Rotation, rhs.Rotation, t);
					return;
				}
			}
		} 
		else {
			// If the value we have is too old, use extrapolation based on 2 latest positions
			float extrapolationLength = Convert.ToSingle(currentTime - bufferedStates[0].TimeStamp)/1000.0f;
			if (mode == InterpolationMode.EXTRAPOLATION && extrapolationLength < extrapolationForwardTime && statesCount>1) {
				Vector3 dif = bufferedStates[0].Position - bufferedStates[1].Position;
				float distance = Vector3.Distance(bufferedStates[0].Position, bufferedStates[1].Position);
				float timeDif = Convert.ToSingle(bufferedStates[0].TimeStamp - bufferedStates[1].TimeStamp)/1000.0f;
				
				if (Mathf.Approximately(distance, 0) || Mathf.Approximately(timeDif, 0)) {
					transform.position = bufferedStates[0].Position;
					transform.rotation = bufferedStates[0].Rotation;
					return;
				}
						
				float speed = distance / timeDif;
				
				dif = dif.normalized;
				Vector3 expectedPosition = bufferedStates[0].Position + dif * extrapolationLength * speed;
				transform.position = Vector3.Lerp(transform.position, expectedPosition, Time.deltaTime*speed); 
			}
			else {
				transform.position = bufferedStates[0].Position;
			}
					
			// No extrapolation for the rotation
			transform.rotation = bufferedStates[0].Rotation;
		}
	}
	
	private void UpdateValues() {
		double ping = TimeManager.Instance.AveragePing;
		if (ping < 50) {
			interpolationBackTime = 50;
		}
		else if (ping < 100) {
			interpolationBackTime = 100;
		}
		else if (ping < 200) {
			interpolationBackTime = 200;
		}
		else if (ping < 400) {
			interpolationBackTime = 400;
		}
		else if (ping < 600) {
			interpolationBackTime = 600;
		}
		else {
			interpolationBackTime = 1000;
		}
	}
	
}
