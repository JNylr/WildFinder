using UnityEngine;
using Unity.Netcode;

namespace Gear.Network
{
    /// <summary>
    /// Simple UI for starting network sessions.
    /// Draws buttons to start Host, Server, or Client.
    /// </summary>
    public class NetworkUI : MonoBehaviour
    {
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 180));
            
            // Make buttons bigger and more visible
            GUI.skin.button.fontSize = 18;
            GUI.skin.button.padding = new RectOffset(15, 15, 15, 15);
            GUI.skin.label.fontSize = 16;

            // Show start buttons only when we are not already connected
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                if (GUILayout.Button("Start Host", GUILayout.Height(50)))
                    NetworkManager.Singleton.StartHost();

                if (GUILayout.Button("Start Server", GUILayout.Height(50)))
                    NetworkManager.Singleton.StartServer();

                if (GUILayout.Button("Start Client", GUILayout.Height(50)))
                    NetworkManager.Singleton.StartClient();
            }
            else
            {
                GUILayout.Label($"Mode: {(NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client")}");
                
                if (GUILayout.Button("Shutdown", GUILayout.Height(50)))
                    NetworkManager.Singleton.Shutdown();
            }

            GUILayout.EndArea();
        }
    }
}
