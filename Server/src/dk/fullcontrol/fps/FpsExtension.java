package dk.fullcontrol.fps;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.SFSExtension;
import dk.fullcontrol.fps.handlers.*;
import dk.fullcontrol.fps.simulation.CombatPlayer;
import dk.fullcontrol.fps.simulation.Item;
import dk.fullcontrol.fps.simulation.Weapon;
import dk.fullcontrol.fps.simulation.World;
import dk.fullcontrol.fps.utils.RoomHelper;
import dk.fullcontrol.fps.utils.UserHelper;

import java.util.List;


// The extension main class. Used to handle requests from clients and send messages back to them
// All communication is done with json objects
//
// Requests that can be send from clients:
// - sendTransform
// - sendAnim
// - spawnMe
// - getTime
// - shot
// - reload
//
// Responses send from the extention to clients:
// - time
// - anim
// - spawnPlayer
// - transform
// - notransform
// - killed
// - health
// - score
// - ammo
// - spawnItem
// - removeItem
// - enemyShotFired
// - reloaded
//

public class FpsExtension extends SFSExtension {

	private World world; // Reference to World simulation model

	public World getWorld() {
		return world;
	}

	@Override
	public void init() {
		world = new World(this);  // Creating the world model

		// Subscribing the request handlers
		addRequestHandler("sendTransform", SendTransformHandler.class);
		addRequestHandler("sendAnim", SendAnimHandler.class);
		addRequestHandler("spawnMe", SpawnMeHandler.class);
		addRequestHandler("getTime", GetTimeHandler.class);
		addRequestHandler("shot", ShotHandler.class);
		addRequestHandler("reload", ReloadHandler.class);

		addEventHandler(SFSEventType.USER_DISCONNECT, OnUserGoneHandler.class);
		addEventHandler(SFSEventType.USER_LEAVE_ROOM, OnUserGoneHandler.class);
		addEventHandler(SFSEventType.USER_LOGOUT, OnUserGoneHandler.class);

		trace("FPS extension initialized");
	}

	@Override
	public void destroy() {
		world = null;
		super.destroy();
	}

	// Send message to client when a player is killed
	public void clientKillPlayer(CombatPlayer pl, CombatPlayer killerPl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("killerId", killerPl.getSfsUser().getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("killed", data, userList);
	}

	// Send message to clients when the health value of a player is updated
	public void clientUpdateHealth(CombatPlayer pl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("health", pl.getHealth());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("health", data, userList);
	}

	// Send message to clients when the score value of a player is updated
	public void updatePlayerScore(CombatPlayer pl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("score", pl.getScore());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("score", data, userList);
	}

	// When new item is spawned - send message to all the clients
	public void clientInstantiateItem(Item item) {
		ISFSObject data = new SFSObject();
		item.toSFSObject(data);

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("spawnItem", data, userList);
	}

	// When someone picked an item up, send message to all the clients, so they will remove the item from scene 
	public void clientRemoveItem(Item item, CombatPlayer player) {
		ISFSObject data = new SFSObject();
		item.toSFSObject(data);
		data.putInt("playerId", player.getSfsUser().getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("removeItem", data, userList);
	}

	// When someone has made a shot, send message to all the clients to inform about it
	public void clientEnemyShotFired(CombatPlayer pl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("enemyShotFired", data, userList);
	}

	// When someone has reloaded the weapon, send message to all the clients to inform about it.
	public void clientReloaded(CombatPlayer player) {
		ISFSObject data = new SFSObject();
		data.putInt("id", player.getSfsUser().getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("reloaded", data, userList);
	}

	// Send instantiate new player message to all the clients
	public void clientInstantiatePlayer(CombatPlayer player) {
		clientInstantiatePlayer(player, null);
		clientUpdateAmmo(player);
	}

	//Send the player instantiation message to all the clients or to a specified user only
	public void clientInstantiatePlayer(CombatPlayer player, User targetUser) {
		ISFSObject data = new SFSObject();

		player.toSFSObject(data);
		Room currentRoom = RoomHelper.getCurrentRoom(this);
		if (targetUser == null) {
			// Sending to all the users
			List<User> userList = UserHelper.getRecipientsList(currentRoom);
			this.send("spawnPlayer", data, userList);
		}
		else {
			// Sending to the specified user
			this.send("spawnPlayer", data, targetUser);
		}
	}

	// Send message to clients when the ammo value of a player is updated
	public void clientUpdateAmmo(CombatPlayer player) {
		ISFSObject data = new SFSObject();
		data.putInt("id", player.getSfsUser().getId());
		data.putInt("ammo", player.getWeapon().getAmmoCount());
		data.putInt("maxAmmo", Weapon.maxAmmo);
		data.putInt("unloadedAmmo", player.getAmmoReserve());

		this.send("ammo", data, player.getSfsUser());
	}

}
