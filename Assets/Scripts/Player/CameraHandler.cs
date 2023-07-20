using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the camera and makes it follow the local player.
/// Also makes the camera and player rotate with the mouse.
/// </summary>
public class CameraHandler : MonoBehaviour
{
    /// <summary>
    /// What CursorLockMode should be used.
    /// </summary>
    [Tooltip("What CursorLockMode should be used.")]
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;

    [Range(0.5f, 10f)]
    public float mouseSensitivity = 0.5f;

    [Range(10, 90)]
    public int yAngleClamp = 88;

    private GameObject localPlayer;
    private InputAction cameraPanAction;
    private InputAction menuAction;

    void Start()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        networkManager.OnClientStarted += () => {
            Cursor.lockState = cursorLockMode;
            Cursor.visible = false;

            StartCoroutine(WaitForLocalPlayer());
        };

        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        cameraPanAction = playerInput.actions.FindAction("camera_pan");
        
#if UNITY_STANDALONE
        playerInput.actions.FindAction("menu").performed += (_) => {
            if (Cursor.lockState == CursorLockMode.None)
                HideCursor();
            else
                ShowCursor();
        };
#endif
    }

    void HideCursor() {
        Cursor.lockState = cursorLockMode;
        Cursor.visible = false;
    }

    void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator WaitForLocalPlayer() {
        NetworkManager networkManager = NetworkManager.Singleton;

        while (localPlayer == null) {
            localPlayer = networkManager.LocalClient?.PlayerObject?.gameObject;
            yield return new WaitForFixedUpdate();
        }
    }

    // Inspired by https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e
    Vector2 rotation = Vector2.zero;

    void LateUpdate() {
        // make first person camera using mouse delta
        if (localPlayer == null) return;
        transform.position = localPlayer.transform.position + new Vector3(0, 0.5f, 0);
        
        Vector2 mouseDelta = cameraPanAction.ReadValue<Vector2>();
        rotation.x += mouseDelta.x * mouseSensitivity;
        rotation.y += mouseDelta.y * mouseSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yAngleClamp, yAngleClamp);
        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
        localPlayer.transform.forward = VectorMath.NormalizeHorizontalProjection(transform.forward);
    }

#if UNITY_STANDALONE
    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus && NetworkManager.Singleton.IsClient) {
            Cursor.lockState = cursorLockMode;
            Cursor.visible = false;
        }
    }
#endif

}
