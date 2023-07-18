using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Moves to the clicked position.
/// </summary>
[DisallowMultipleComponent]
public class ClickTeleport : NetworkBehaviour
{
    /// <summary>
    /// The target to move. By default, the current GameObject.
    /// </summary>
    [Tooltip("The target to move.")]
    public GameObject m_Target;

    private Camera m_Camera;

    private void Start()
    {
        m_Target = m_Target == null ? gameObject : m_Target;
        m_Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsOwner)
        {
            var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                m_Target.transform.position = hit.point;
            }
        }
    }
}
