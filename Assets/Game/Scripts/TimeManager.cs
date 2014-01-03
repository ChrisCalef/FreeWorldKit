using UnityEngine;
using System;
using System.Collections;

// Synchronizes client time with the server time
public class TimeManager : MonoBehaviour {
	private readonly float period = 3.0f;
	
	private static TimeManager instance;
	public static TimeManager Instance {
		get {
			return instance;
		}
	}
		
	private float lastRequestTime = float.MaxValue;
	private float timeBeforeSync = 0;
	private bool synchronized = false;
		
	private double lastServerTime = 0;
	private double lastLocalTime = 0;
	
	private bool running = false;
	
	private double averagePing = 0;
	private int pingCount = 0;
	
	private readonly int averagePingCount = 10;
	private double[] pingValues;
	private int pingValueIndex;

	void Awake() {
		instance = this;
	}
	
	public void Init() {
		pingValues = new double[averagePingCount];
		pingCount = 0;
		pingValueIndex = 0;
		running = true;
	}
		
	public void Synchronize(double timeValue) {
		// Measure the ping in milliseconds
		double ping = (Time.time - timeBeforeSync)*1000;
		CalculateAveragePing(ping);
				
		// Take the time passed between server sends response and we get it 
		// as half of the average ping value
		double timePassed = averagePing / 2.0f;
		lastServerTime = timeValue + timePassed;
		lastLocalTime = Time.time;
		
		synchronized = true;	
	}
		
	void Update () {
		if (!running) return;
		
		if (lastRequestTime > period) {
			lastRequestTime = 0;
			timeBeforeSync = Time.time;
			NetworkManager.Instance.TimeSyncRequest();
		}
		else {
			lastRequestTime += Time.deltaTime;
		}
	}
	
	/// <summary>
	/// Network time in msecs
	/// </summary>
	public double NetworkTime {
		get {
			// Taking server timestamp + time passed locally since the last server time received			
			return (Time.time - lastLocalTime)*1000 + lastServerTime;
		}
	}
			
	public double AveragePing {
		get {
			return averagePing;
		}
	}
	
	
	private void CalculateAveragePing(double ping) {
		pingValues[pingValueIndex] = ping;
		pingValueIndex++;
		if (pingValueIndex >= averagePingCount) pingValueIndex = 0;
		if (pingCount < averagePingCount) pingCount++;
					
		double pingSum = 0;
		for (int i=0; i<pingCount; i++) {
			pingSum += pingValues[i];
		}
		
		averagePing = pingSum / pingCount;
	}

		
}
