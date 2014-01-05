package dk.fullcontrol.fps.simulation;

import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;

import java.util.Random;

// Transform class - keeps player or items position and rotation

// Also provides methods like calculating distance
public class Transform {
	private double x;
	private double y;
	private double z;

	private double rotx;
	private double roty;
	private double rotz;

	private long timeStamp = 0;  // Timestamp is stored to perform interpolation and extrapolation on client

	private static Random rnd = new Random();

	// Create random transform choosing from the predefined spawnPoints list
	public static Transform random() {
		Transform[] spawnPoints = getSpawnPoints();

		int i = rnd.nextInt(spawnPoints.length);
		return spawnPoints[i];
	}

	// Create random transform using the specified bounds
	public static Transform randomWorld() {
		double x = rnd.nextDouble() * (World.maxX - World.minX) + World.minX;
		double z = rnd.nextDouble() * (World.maxZ - World.minZ) + World.minZ;
		double y = 6;
		return new Transform(x, y, z, 0, 0, 0);
	}

	// Hardcoded spawnPoints - where players will spawn
	private static Transform[] getSpawnPoints() {
		Transform[] spawnPoints = new Transform[3];
		spawnPoints[0] = new Transform(25, 6, 3, 0, 0, 0);
		spawnPoints[1] = new Transform(-24, 6, -20, 0, 0, 0);
		spawnPoints[2] = new Transform(18, 6, -63, 0, 0, 0);
		return spawnPoints;
	}

	public Transform(double x, double y, double z, double rotx, double roty, double rotz) {
		this.x = x;
		this.y = y;
		this.z = z;

		this.rotx = rotx;
		this.roty = roty;
		this.rotz = rotz;
	}

	public double getRotx() {
		return rotx;
	}

	public double getRoty() {
		return roty;
	}

	public double getX() {
		return x;
	}


	public double getY() {
		return y;
	}

	public double getZ() {
		return z;
	}

	public void setTimeStamp(long timeStamp) {
		this.timeStamp = timeStamp;
	}

	public long getTimeStamp() {
		return timeStamp;
	}

	public static Transform fromSFSObject(ISFSObject data) {
		ISFSObject transformData = data.getSFSObject("transform");

		double x = transformData.getDouble("x");
		double y = transformData.getDouble("y");
		double z = transformData.getDouble("z");

		double rx = transformData.getDouble("rx");
		double ry = transformData.getDouble("ry");
		double rz = transformData.getDouble("rz");

		long timeStamp = transformData.getLong("t");

		Transform transform = new Transform(x, y, z, rx, ry, rz);
		transform.setTimeStamp(timeStamp);
		return transform;
	}

	public void toSFSObject(ISFSObject data) {
		ISFSObject transformData = new SFSObject();
		transformData.putDouble("x", x);
		transformData.putDouble("y", y);
		transformData.putDouble("z", z);

		transformData.putDouble("rx", rotx);
		transformData.putDouble("ry", roty);
		transformData.putDouble("rz", rotz);

		transformData.putLong("t", this.timeStamp);

		data.putSFSObject("transform", transformData);
	}

	// Copy another transform to this one
	public void load(Transform another) {
		this.x = another.x;
		this.y = another.y;
		this.z = another.z;

		this.rotx = another.rotx;
		this.roty = another.roty;
		this.rotz = another.rotz;

		this.setTimeStamp(another.getTimeStamp());
	}

	// Calculate distance to another transform
	public double distanceTo(Transform transform) {
		double dx = Math.pow(this.getX() - transform.getX(), 2);
		double dy = Math.pow(this.getY() - transform.getY(), 2);
		double dz = Math.pow(this.getZ() - transform.getZ(), 2);
		return Math.sqrt(dx + dy + dz);
	}


}
