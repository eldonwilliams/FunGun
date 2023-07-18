using Unity.Netcode;
using UnityEngine;

public class FollowLocal : NetworkBehaviour
{
    GameObject localPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !IsClient) {
            gameObject.GetComponent<FollowLocal>().enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!IsSpawned) return;
        if (localPlayer == null) {
            NetworkManager networkManager = NetworkManager.Singleton;
            NetworkObject localClient = networkManager.SpawnManager.GetLocalPlayerObject();
            if (localClient == null) return;
            localPlayer = localClient.gameObject;
            return;
        }
        transform.position = new Vector3(0, 6f, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(localPlayer.transform.position - transform.position), Time.deltaTime * 3f);
    }
}
