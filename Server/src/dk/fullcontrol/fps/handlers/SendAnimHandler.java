package dk.fullcontrol.fps.handlers;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import dk.fullcontrol.fps.utils.RoomHelper;
import dk.fullcontrol.fps.utils.UserHelper;

public class SendAnimHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
		ISFSObject res = new SFSObject();
		res.putUtfString("msg", data.getUtfString("msg"));
		res.putInt("layer", data.getInt("layer"));
		res.putInt("id", u.getId());
		Room currentRoom = RoomHelper.getCurrentRoom(this);
		this.send("anim", res, UserHelper.getRecipientsList(currentRoom, u));
	}


}
