package dk.fullcontrol.fps.handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import java.util.Date;

//This request is used to send the server time to clients
public class GetTimeHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
		ISFSObject res = new SFSObject();
		Date date = new Date();
		res.putLong("t", date.getTime());
		this.send("time", res, u);
	}

}
