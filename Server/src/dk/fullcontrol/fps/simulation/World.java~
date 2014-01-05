package dk.fullcontrol.fps.simulation;

import com.smartfoxserver.v2.entities.User;
import dk.fullcontrol.fps.FpsExtension;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;


// The main World server model. Contains players, items, and all the other needed world objects
public class World {
	private static final int maxSpawnedItems = 6; // Maximum items that can be spawned at once

	// World bounds - to create random transforms
	public static final double minX = -35;
	public static final double maxX = 35;
	public static final double minZ = -70;
	public static final double maxZ = 5;

	private static final int maxItemsOfSingleType = 3; // Maximum items of the particular type that can be present on the scene

	private static Random rnd = new Random();

	private int itemId = 0; // Item id counter - to generate unique ids

	private FpsExtension extension; // Reference to the server extension

	// Players
	private List<CombatPlayer> players = new ArrayList<CombatPlayer>();

	// Items
	private List<Item> items = new ArrayList<Item>();

	public World(FpsExtension extension) {
		this.extension = extension;
		rnd.setSeed((new Date()).getTime());
	}

	public List<CombatPlayer> getPlayers() {
		return players;
	}

	// Spawning new items
	public void spawnItems() {
		int itemsCount = rnd.nextInt(maxSpawnedItems);

		int healthItemsCount = itemsCount / 2;
		int hc = 0;
		extension.trace("Spawn " + itemsCount + " items.");

		for (int i = 0; i < itemsCount; i++) {
			ItemType itemType = (hc++ < healthItemsCount) ? ItemType.HealthPack : ItemType.Ammo;
			if (hasMaxItems(itemType)) {
				continue;
			}

			Item item = new Item(itemId++, Transform.randomWorld(), itemType);
			items.add(item);
			extension.clientInstantiateItem(item);
		}
	}

	private boolean hasMaxItems(ItemType itemType) {
		int counter = 0;

		for (Item item : items) {
			if (item.getItemType() == itemType) {
				counter++;
			}
		}

		return counter > maxItemsOfSingleType;
	}

	// Add new player if he doesn't exist, or resurrect him if he already added
	public boolean addOrRespawnPlayer(User user) {
		CombatPlayer player = getPlayer(user);

		if (player == null) {
			player = new CombatPlayer(user);
			players.add(player);
			extension.clientInstantiatePlayer(player);
			return true;
		}
		else {
			player.resurrect();
			extension.clientInstantiatePlayer(player);
			return false;
		}
	}

	// Trying to move player. If the new transform is not valid, returns null
	public Transform movePlayer(User u, Transform newTransform) {
		CombatPlayer player = getPlayer(u);

		if (player.isDead()) {
			return player.getTransform();
		}

		if (isValidNewTransform(player, newTransform)) {
			player.getTransform().load(newTransform);

			checkItem(player, newTransform);

			return newTransform;
		}

		return null;
	}

	// Check the player intersection with item - to pick it up
	private void checkItem(CombatPlayer player, Transform newTransform) {
		for (Object itemObj : items.toArray()) {
			Item item = (Item) itemObj;
			if (item.isClose(newTransform)) {
				try {
					useItem(player, item);
				}
				catch (Throwable e) {
					extension.trace("Exception using item " + e.getMessage());
				}
				return;
			}
		}
	}

	// Applying the item effect and removing the item from World
	private void useItem(CombatPlayer player, Item item) {
		if (item.getItemType() == ItemType.Ammo) {
			if (player.hasMaxAmmoInReserve()) {
				return;
			}

			player.addAmmoToReserve(20);
			extension.clientUpdateAmmo(player);
		}
		else if (item.getItemType() == ItemType.HealthPack) {
			if (player.hasMaxHealth()) {
				return;
			}

			player.addHealth(CombatPlayer.maxHealth / 3);
			extension.clientUpdateHealth(player);
		}

		extension.clientRemoveItem(item, player);
		items.remove(item);
	}

	public Transform getTransform(User u) {
		CombatPlayer player = getPlayer(u);
		return player.getTransform();
	}

	private boolean isValidNewTransform(CombatPlayer player,
	                                    Transform newTransform) {

		// Check if the given transform is valid in terms of collisions, speed hacks etc
		// In this example, the server will always accept a new transform from the client

		return true;
	}

	// Gets the player corresponding to the specified SFS user
	private CombatPlayer getPlayer(User u) {
		for (CombatPlayer player : players) {
			if (player.getSfsUser().getId() == u.getId()) {
				return player;
			}
		}

		return null;
	}

	// Process the shot from client
	public void processShot(User fromUser) {
		CombatPlayer player = getPlayer(fromUser);
		if (player.isDead()) {
			return;
		}
		if (player.getWeapon().getAmmoCount() <= 0) {
			return;
		}
		if (!player.getWeapon().isReadyToFire()) {
			return;
		}

		player.getWeapon().shoot();

		extension.clientUpdateAmmo(player);
		extension.clientEnemyShotFired(player);

		// Determine the intersection of the shot line with any of the other players to check if we hit or missed
		for (CombatPlayer pl : players) {
			if (pl != player) {
				boolean res = checkHit(player, pl);
				if (res) {
					playerHit(player, pl);
					return;
				}

			}
		}

		// if we are here - we missed
	}

	// Performing reload
	public void processReload(User fromUser) {
		CombatPlayer player = getPlayer(fromUser);
		if (player.isDead()) {
			return;
		}
		if (player.getAmmoReserve() == 0) {
			return;
		}
		if (!player.getWeapon().canReload()) {
			return;
		}

		player.reload();
		extension.clientReloaded(player);
		extension.clientUpdateAmmo(player);
	}

	// Checking if the player hits enemy using simple line intersection and 
	// the known players position and rotation angles
	private boolean checkHit(CombatPlayer player, CombatPlayer enemy) {
		if (enemy.isDead()) {
			return false;
		}

		// First of all checking the line intersection with enemy in top projection

		double radius = enemy.getCollider().getRadius();
		double height = enemy.getCollider().getHeight();
		double myAngle = player.getTransform().getRoty();
		double vertAngle = player.getTransform().getRotx();

		// Calculating an angle relatively to X axis anti-clockwise
		double normalAngle = normAngle(360 + 90 - myAngle);

		//Calculating the angle of the line between player and enemy center point
		double difx = enemy.getX() - player.getX();
		double difz = enemy.getZ() - player.getZ();

		double ang = 0;
		if (difx == 0) {
			ang = 90;
		}
		else {
			ang = Math.toDegrees(Math.atan(Math.abs(difz / difx)));
		}

		// Modifying angle depending on the quarter
		if (difx <= 0) {
			if (difz <= 0) {
				ang += 180;
			}
			else {
				ang = 180 - ang;
			}
		}
		else {
			if (difz <= 0) {
				ang = 360 - ang;
			}
		}
		ang = normAngle(ang);

		// Calculating min angle to hit
		double angDif = Math.abs(ang - normalAngle);
		double d = Math.sqrt(difx * difx + difz * difz);
		double maxDif = Math.toDegrees(Math.atan(radius / d));

		if (angDif > maxDif) {
			return false;
		}

		// Now calculating the shot in the side projection

		// Correction value to fit the model visually (as the collider may not totally fit the model height on client)
		final double heightCorrection = 0.3;

		if (vertAngle > 90) {
			vertAngle = 360 - vertAngle;
		}
		else {
			vertAngle = -vertAngle;
		}

		double h = d * Math.tan(Math.toRadians(vertAngle));
		double dif = enemy.getTransform().getY() - player.getTransform().getY() - h + heightCorrection;
		if (dif < 0 || dif > height) {
			return false;
		}

		return true;
	}

	private double normAngle(double a) {
		if (a >= 360) {
			return a - 360;
		}
		return a;
	}

	// Applying the hit to the player.
	// Processing the health and death
	private void playerHit(CombatPlayer fromPlayer, CombatPlayer pl) {
		pl.removeHealth(20);

		if (pl.isDead()) {
			fromPlayer.addKillToScore();  // Adding frag to the player if he killed the enemy
			extension.updatePlayerScore(fromPlayer);
			extension.clientKillPlayer(pl, fromPlayer);
		}
		else {
			// Updating the health of the hit enemy
			extension.clientUpdateHealth(pl);
		}
	}

	// When user lefts the room or disconnects - removing him from the players list 
	public void userLeft(User user) {
		CombatPlayer player = this.getPlayer(user);
		if (player == null) {
			return;
		}
		players.remove(player);
	}

}
