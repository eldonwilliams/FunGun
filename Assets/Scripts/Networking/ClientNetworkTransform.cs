using Unity.Netcode.Components;
using UnityEngine;

/// <summary>
/// Client authoritative network transform.
/// </summary>
[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
