using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// Moves to the clicked position.
/// </summary>
[DisallowMultipleComponent]
public class PlayerMovement : NetworkBehaviour
{
    /// <summary>
    /// The target to move. By default, the current GameObject.
    /// </summary>
    [Tooltip("The target to move.")]
    public GameObject Target;

    /// <summary>
    /// Speed of the movement.
    /// </summary>
    public float Speed = 10f;

    new private Camera camera;
    private InputAction moveAction;

    private void Start()
    {
        Target = Target == null ? gameObject : Target;
        camera = Camera.main;
        Target.transform.position = new Vector3(0, 1, 0);
        moveAction = FindObjectOfType<PlayerInput>().actions.FindAction("move");
    }

    private void Update()
    {
        if (IsOwner) {
            Target.transform.position += VectorMath.NormalizeHorizontalProjection(moveAction.ReadValue<Vector2>().x * camera.transform.right + moveAction.ReadValue<Vector2>().y * camera.transform.forward) * Time.deltaTime * Speed;
        }
    }
}
