using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Aims the holder of this script at the camera forward.
/// </summary>
public class AimHeldObject : NetworkBehaviour
{
    public float aimSpeed = 30f;

    private Quaternion targetRotation;

    private void Start()
    {
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (IsOwner)
        {
            // Rotate towards the camera forward
            targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        }

        // Rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
    }
}