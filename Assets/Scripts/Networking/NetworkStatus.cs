using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkStatus : MonoBehaviour
{
    void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        NetworkManager networkManager = NetworkManager.Singleton;
        if (!networkManager.IsServer && !networkManager.IsClient)
            StartButtons();
        else
            StatusLabels();

        GUILayout.EndArea();
    }

    private string ip_addr = "127.0.0.1";

    void StartButtons() {
        NetworkManager networkManager = NetworkManager.Singleton;
        if (GUILayout.Button("Start Server")) networkManager.StartServer();
        if (GUILayout.Button("Start Client")) networkManager.StartClient();
        GUILayout.Label("IP Address:");
        ip_addr = GUILayout.TextField(ip_addr);
        UnityTransport transport = (UnityTransport) networkManager.NetworkConfig.NetworkTransport;
        transport.ConnectionData.Address = ip_addr;
        if (GUILayout.Button("Start Host")) networkManager.StartHost();
    }

    static void StatusLabels() {
        NetworkManager networkManager = NetworkManager.Singleton;
        string mode = networkManager.IsHost ? "Host" : networkManager.IsClient ? "Client" : "Server";

        UnityTransport transport = (UnityTransport) networkManager.NetworkConfig.NetworkTransport;

        // It should be assumed from now on out that UnityTransport is used.
        // GUILayout.Label("Transport: " + transportType.Name);
        GUILayout.Label("IP: " + transport.ConnectionData.Address);
        GUILayout.Label("Mode: " + mode);
    }
}
