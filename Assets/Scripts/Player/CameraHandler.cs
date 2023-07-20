using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This script is attached to the camera and makes it follow the local player.
/// Also makes the camera and player rotate with the mouse.
/// </summary>
public class CameraHandler : NetworkBehaviour
{
    /// <summary>
    /// What CursorLockMode should be used.
    /// </summary>
    [Tooltip("What CursorLockMode should be used.")]
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;

    private GameObject localPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !IsClient) {
            gameObject.GetComponent<CameraHandler>().enabled = false;
            return;
        }

        Cursor.lockState = cursorLockMode;
        Cursor.visible = false;

        NetworkManager networkManager = NetworkManager.Singleton;
        NetworkObject localClient = networkManager.SpawnManager.GetLocalPlayerObject();
        if (localClient == null) return;
        localPlayer = localClient.gameObject;
    }

    // Inspired by https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
    Vector2 rotation = Vector2.zero;

    void LateUpdate() {
        // make first person camera using mouse delta
        if (localPlayer == null) return;
        transform.position = localPlayer.transform.position;

        rotation.x += Input.GetAxis("Mouse X") * 3f;
        rotation.y += Input.GetAxis("Mouse Y") * 3f;
        rotation.y = Mathf.Clamp(rotation.y, -90, 90);
        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
        localPlayer.transform.forward = VectorMath.NormalizeHorizontalProjection(transform.forward);
    }

}
