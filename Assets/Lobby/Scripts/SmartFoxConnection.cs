using UnityEngine;
using Sfs2X;

// Statics for holding the connection to the SFS server end
// Can then be queried from the entire game to get the connection

public class SmartFoxConnection : MonoBehaviour
{
	private static SmartFoxConnection mInstance; 
	private static SmartFox smartFox;
	public static SmartFox Connection {
		get {
            if (mInstance == null) {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            return smartFox;
        }
      set {
            if (mInstance == null) {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
            }
            smartFox = value;
        } 
	}

	public static bool IsInitialized {
		get { 
			return (smartFox != null); 
		}
	}
	
	// Handle disconnection automagically
	// ** Important for Windows users - can cause crashes otherwise
    void OnApplicationQuit() { 
        if (smartFox.IsConnected) {
            smartFox.Disconnect();
        }
    } 
}