
using System;
using System.Collections;
using UnityEngine;
using Sfs2X.Entities.Data;

// This class holds transform values and can load and send the data to server
public class NetworkTransform
{
	private NetworkTransform ()
	{
	}
		
	private Vector3 position; // Position as Vector3
	private Vector3 angleRotation; // Rotation as 3 euler angles - x, y, z
	
	private double timeStamp = 0;
		
	public Vector3 Position {
		get {
			return position;
		}
	}
		
	public Vector3 AngleRotation {
		get {
			return angleRotation;
		}
	}
	
	/// <summary>
	/// Returns the rotation with y component only
	/// </summary>
	public Vector3 AngleRotationFPS {
		get {
			return new Vector3(0, angleRotation.y, 0);
		}
	}
	
	public Quaternion Rotation {
		get {
			return Quaternion.Euler(AngleRotationFPS);
		}
	}
	
	/// <summary>
	/// Timestamp needed for interpolation and extrapolation
	/// </summary>
	public double TimeStamp {
		get {
			return timeStamp;
		}
		set {
			timeStamp = value;
		}
	}
	
	// Check if this transform is different from given one with specified accuracy
	public bool IsDifferent(Transform transform, float accuracy) {
		float posDif = Vector3.Distance(this.position, transform.position);
		float angDif = Vector3.Distance(this.AngleRotation, transform.localEulerAngles);
		
		return (posDif>accuracy || angDif > accuracy);
	}
	
	// Stores the transform values to SFSObject to send them to server
	public void ToSFSObject(ISFSObject data) {
		ISFSObject tr = new SFSObject();
				
		tr.PutDouble("x", Convert.ToDouble(this.position.x));
		tr.PutDouble("y", Convert.ToDouble(this.position.y));
		tr.PutDouble("z", Convert.ToDouble(this.position.z));
		
		tr.PutDouble("rx", Convert.ToDouble(this.angleRotation.x));
		tr.PutDouble("ry", Convert.ToDouble(this.angleRotation.y));
		tr.PutDouble("rz", Convert.ToDouble(this.angleRotation.z));
		
		tr.PutLong("t", Convert.ToInt64(this.timeStamp));
			
		data.PutSFSObject("transform", tr);
	}
	
	// Copies another NetworkTransform to itself
	public void Load(NetworkTransform ntransform) {
		this.position = ntransform.position;
		this.angleRotation = ntransform.angleRotation;
		this.timeStamp = ntransform.timeStamp;
	}
	
	// Copu the Unity transform to itself
	public void Update(Transform trans) {
		trans.position = this.Position;
		trans.localEulerAngles = this.AngleRotation;
	}
	
	// Creating NetworkTransform from SFS object
	public static NetworkTransform FromSFSObject(ISFSObject data) {
		NetworkTransform trans = new NetworkTransform();
		ISFSObject transformData = data.GetSFSObject("transform");
		
		float x = Convert.ToSingle(transformData.GetDouble("x"));
		float y = Convert.ToSingle(transformData.GetDouble("y"));
		float z = Convert.ToSingle(transformData.GetDouble("z"));
		
		float rx = Convert.ToSingle(transformData.GetDouble("rx"));
		float ry = Convert.ToSingle(transformData.GetDouble("ry"));
		float rz = Convert.ToSingle(transformData.GetDouble("rz"));
					
		trans.position = new Vector3(x, y, z);
		trans.angleRotation = new Vector3(rx, ry, rz);
				
		if (transformData.ContainsKey("t")) {
			trans.TimeStamp = Convert.ToDouble(transformData.GetLong("t"));
		}
		else {
			trans.TimeStamp = 0;
		}
		
		return trans;
	}
	
	// Creating NetworkTransform from Unity transform
	public static NetworkTransform FromTransform(Transform transform) {
		NetworkTransform trans = new NetworkTransform();
				
		trans.position = transform.position;
		trans.angleRotation = transform.localEulerAngles;
				
		return trans;
	}
	
	// Clone itself
	public static NetworkTransform Clone(NetworkTransform ntransform) {
		NetworkTransform trans = new NetworkTransform();
		trans.Load(ntransform);
		return trans;
	}
	
}
