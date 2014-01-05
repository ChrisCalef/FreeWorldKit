package dk.fullcontrol.fps.simulation;

// Collider describes the player size that is used in the shot calculation

// This class can be extended to work in world collision model.
public class Collider {

	public Collider(double x, double y, double z, double radius, double height) {
		this.centerx = x;
		this.centery = y;
		this.centerz = z;

		this.radius = radius;
		this.height = height;
	}

	private double centerx;
	private double centery;
	private double centerz;

	private double radius;

	private double height;

	public double getCenterx() {
		return centerx;
	}

	public double getCentery() {
		return centery;
	}

	public double getCenterz() {
		return centerz;
	}

	public double getRadius() {
		return radius;
	}

	public double getHeight() {
		return height;
	}

}
