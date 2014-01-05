package dk.fullcontrol.fps.handlers;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import dk.fullcontrol.fps.simulation.Transform;
import dk.fullcontrol.fps.simulation.World;
import dk.fullcontrol.fps.utils.RoomHelper;
import dk.fullcontrol.fps.utils.UserHelper;

import java.util.Date;
import java.util.List;

public class SendTransformHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
		// The transform we received from player
		Transform receivedTransform = Transform.fromSFSObject(data);

		World world = RoomHelper.getWorld(this);
		// Trying to apply new transform and return it or null if it's rejected
		Transform resultTransform = world.movePlayer(u, receivedTransform);

		if (resultTransform != null) {
			// Server accepted transform - send it to all the clients
			sendTransform(u, resultTransform);
		}
		else {
			// Server rejected transform - send corresponding message to client
			sendRejectedTransform(u);
		}

	}

	// Send the transform to all the clients
	private void sendTransform(User fromUser, Transform resultTransform) {
		ISFSObject data = new SFSObject();

		// Adding server timestamp to transform here
		long time = new Date().getTime();
		resultTransform.setTimeStamp(time);

		resultTransform.toSFSObject(data);
		data.putInt("id", fromUser.getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("transform", data, userList, true); // Use UDP = true
	}

	// Sending rejected transform message to specified user
	private void sendRejectedTransform(User u) {
		ISFSObject data = new SFSObject();
		RoomHelper.getWorld(this).getTransform(u).toSFSObject(data);
		data.putInt("id", u.getId());
		this.send("notransform", data, u, true); // Use UDP = true
	}


}
