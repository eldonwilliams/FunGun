using Unity.Netcode;
using UnityEngine;

public class MovementPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            Move();
        }
    }

    public void Move() {
        NetworkManager networkManager = NetworkManager.Singleton;
        if (networkManager.IsServer) {
            Vector3 randomPos = GetRandomPosition();
            transform.position = randomPos;
            Position.Value = randomPos;
        } else {
            MoveRequestServerRPC();
        }
    }

    void Start() {
        Position.OnValueChanged += (Vector3 _, Vector3 newPosition) => {
            goalPosition = newPosition;
        };
    }

    private Vector3 goalPosition;

    void Update() {
        if (IsServer) return;
        transform.position = Vector3.Lerp(transform.position, goalPosition, Time.deltaTime * 6f);
    }

    [ServerRpc]
    void MoveRequestServerRPC() {
        Move();
    }

    static Vector3 GetRandomPosition() {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }
}
