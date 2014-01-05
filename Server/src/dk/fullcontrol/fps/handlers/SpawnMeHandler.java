package dk.fullcontrol.fps.handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import dk.fullcontrol.fps.FpsExtension;
import dk.fullcontrol.fps.simulation.CombatPlayer;
import dk.fullcontrol.fps.simulation.World;
import dk.fullcontrol.fps.utils.RoomHelper;

public class SpawnMeHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
		World world = RoomHelper.getWorld(this);
		boolean newPlayer = world.addOrRespawnPlayer(u);

		if (newPlayer) {
			// Send this player data about all the other players
			sendOtherPlayersInfo(u);
		}

		// Instantiating more items together with spawning player
		world.spawnItems();
	}

	// Send the data for all the other players to the newly joined client
	private void sendOtherPlayersInfo(User targetUser) {
		World world = RoomHelper.getWorld(this);
		for (CombatPlayer player : world.getPlayers()) {
			if (player.isDead()) {
				continue;
			}

			if (player.getSfsUser().getId() != targetUser.getId()) {
				FpsExtension ext = (FpsExtension) this.getParentExtension();
				ext.clientInstantiatePlayer(player, targetUser);
			}
		}
	}

}
