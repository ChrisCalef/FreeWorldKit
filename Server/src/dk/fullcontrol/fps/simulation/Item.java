package dk.fullcontrol.fps.simulation;

import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;

// Item class describes an item on the scene

// In this example project it represents either ammo crate or health pack
public class Item {
	public static final double touchDistance = 3; // How close must the player approach to the item to take it

	private int id;  // The unique id of the item
	private Transform transform; // The position and rotation of the item in the world
	private ItemType itemType; // The type of the item (ammo or healthpack)

	public Item(int id, Transform transform, ItemType itemType) {
		this.id = id;
		this.transform = transform;
		this.itemType = itemType;
	}

	public ItemType getItemType() {
		return itemType;
	}

	// Put the item info to JSON object to send it to client
	public void toSFSObject(ISFSObject data) {
		ISFSObject itemData = new SFSObject();

		itemData.putInt("id", id);
		itemData.putUtfString("type", itemType.toString());
		this.transform.toSFSObject(itemData);
		data.putSFSObject("item", itemData);
	}

	public boolean isClose(Transform newTransform) {
		double d = newTransform.distanceTo(this.transform);
		return (d <= touchDistance);
	}

}
