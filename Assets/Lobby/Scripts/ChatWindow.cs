using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Requests;

public class ChatWindow {
	private ArrayList messages = new ArrayList();
	private string newMessage = "";
	private Vector2 chatScrollPosition;
		
	private SmartFox smartFox;
	private GUIStyle chatStyle;
	private GUIStyle userEventStyle;
	private GUIStyle systemStyle;
	
	public ChatWindow() {
		smartFox = SmartFoxConnection.Connection;
		SetStyle();
	}

	private void SetStyle() {
		chatStyle = new GUIStyle();
		chatStyle.normal.textColor = new Color(0, 0, 0);
		chatStyle.wordWrap = true;
		userEventStyle = new GUIStyle();
		userEventStyle.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
		userEventStyle.wordWrap = true;
		systemStyle = new GUIStyle();
		systemStyle.normal.textColor = new Color(1, 0, 0);
		systemStyle.wordWrap = true;
	}

	public void AddSystemMessage(string message) {
		messages.Add(new ChatMessage(ChatMessage.ChatType.SYSTEM, message));
		chatScrollPosition.y = 100000;
	}

	public void AddChatMessage(string message) {
		messages.Add(new ChatMessage(ChatMessage.ChatType.CHAT, message));
		chatScrollPosition.y = 100000;
	}

	public void AddPlayerJoinMessage(string message) {
		messages.Add(new ChatMessage(ChatMessage.ChatType.JOIN, message));
		chatScrollPosition.y = 100000;
	}

	public void AddPlayerLeftMessage(string message) {
		messages.Add(new ChatMessage(ChatMessage.ChatType.LEAVE, message));
		chatScrollPosition.y = 100000;
	}

	public void Draw(float chatPanelPosX, float chatPanelPosY, float chatPanelWidth, float chatPanelHeight) {
		// Chat history panel
		GUILayout.BeginArea(new Rect(chatPanelPosX, chatPanelPosY, chatPanelWidth, chatPanelHeight));
		GUILayout.Box ("Chat", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.BeginVertical();
		GUILayout.BeginArea(new Rect(20, 25, chatPanelWidth-40, chatPanelHeight-80), GUI.skin.customStyles[0]);
		
		chatScrollPosition = GUILayout.BeginScrollView (chatScrollPosition);
		foreach (ChatMessage message in messages) {
			DrawChatMessage(message);
		}
					
		GUILayout.EndScrollView ();
		GUILayout.EndArea();

		// Send chat message text field and button
		GUILayout.BeginArea(new Rect(30, chatPanelHeight - 40, chatPanelWidth-60, 40));
		GUILayout.BeginHorizontal();
		newMessage = GUILayout.TextField(newMessage, 50, GUILayout.Width(chatPanelWidth - 120));

		if (GUILayout.Button("Send")  || (Event.current.type == EventType.keyDown && Event.current.character == '\n')) {
			smartFox.Send(new PublicMessageRequest(newMessage, null, smartFox.LastJoinedRoom));
			newMessage = "";
		}
		GUILayout.EndHorizontal();
					
		GUILayout.EndArea ();		
		GUILayout.EndVertical();
		GUILayout.EndArea ();
	}
	
	private void DrawChatMessage(ChatMessage message) {
		GUILayout.BeginHorizontal();
		GUILayout.Space(5);
		switch (message.GetChatType()) {
			case ChatMessage.ChatType.SYSTEM:
				GUILayout.Label(message.GetMessage(), systemStyle);
				break;
			case ChatMessage.ChatType.CHAT:
				GUILayout.Label(message.GetMessage(), chatStyle);
				break;
			case ChatMessage.ChatType.JOIN:
			case ChatMessage.ChatType.LEAVE:
				GUILayout.Label(message.GetMessage(), userEventStyle);
				break;
			default:
				// Ignore and dont print anything
				break;
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(1);
	}
	
	class ChatMessage {
		public enum ChatType {
			IGNORE = 0,
			SYSTEM,
			CHAT,
			JOIN,
			LEAVE,
		};
		private ChatType type;
		private string message;

		public ChatMessage() {
			type = ChatType.IGNORE;
			message = "";
		}

		public ChatMessage(ChatType type, string message) {
			this.type = type;
			this.message = message;
		}

		public ChatType GetChatType() {
			return type;
		}

		public string GetMessage() {
			return message;
		}
	}
}
