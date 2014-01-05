package dk.fullcontrol.fps.simulation;

// Weapon class describes the weapon user has
public class Weapon {
	public static final int maxAmmo = 10;  // Max loaded ammo

	private static final double shotTime = 0.6 * 1000; // The minimum time between 2 shots
	private static final double reloadTime = 1.3 * 1000; // The time weapon is being reloaded (cannot shot this time)

	private int ammoCount = maxAmmo;

	private long lastShotTime = 0;
	private long lastReloadTime = 0;

	public Weapon() {
		resetAmmo();
	}

	public void resetAmmo() {
		ammoCount = maxAmmo;
	}

	// Reload the weapon
	public int reload(int ammoReserve) {
		lastReloadTime = System.currentTimeMillis();
		int usedAmmo = maxAmmo - ammoCount;
		if (usedAmmo > ammoReserve) {
			usedAmmo = ammoReserve;
		}
		ammoCount += usedAmmo;
		return usedAmmo;
	}

	// Shoot from this weapon
	public void shoot() {
		if (ammoCount > 0) {
			ammoCount--;
			lastShotTime = System.currentTimeMillis();
		}
	}

	public int getAmmoCount() {
		return ammoCount;
	}

	public boolean isFullyLoaded() {
		return ammoCount >= maxAmmo;
	}

	// Check if it's posible to shoot at this moment
	public boolean isReadyToFire() {
		if (ammoCount == 0) {
			return false;
		}
		if (lastReloadTime + reloadTime > System.currentTimeMillis()) {
			return false;
		}
		if (lastShotTime + shotTime > System.currentTimeMillis()) {
			return false;
		}
		return true;
	}

	// Check if it's posible to reload at this moment
	public boolean canReload() {
		if (this.isFullyLoaded()) {
			return false;
		}
		if (lastReloadTime + reloadTime > System.currentTimeMillis()) {
			return false;
		}
		if (lastShotTime + shotTime > System.currentTimeMillis()) {
			return false;
		}

		return true;
	}

}
