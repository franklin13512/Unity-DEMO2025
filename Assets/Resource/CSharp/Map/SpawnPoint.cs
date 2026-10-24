using UnityEngine;

/// <summary>
/// 出生点标记：放在场景里，ScenePortal 通过 SpawnId 指定要出生在哪个点。
/// 场景里没有匹配项时，SceneLoader 会退回到第一个 SpawnPoint。
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("出生点标识，例如 Default / GateA / GateB")]
    public string SpawnId = "Default";

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 1f, 0.4f, 0.85f);
        Gizmos.DrawWireSphere(transform.position, 0.35f);
        Gizmos.DrawLine(transform.position + Vector3.left * 0.35f, transform.position + Vector3.right * 0.35f);
        Gizmos.DrawLine(transform.position + Vector3.up * 0.35f, transform.position + Vector3.down * 0.35f);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, "Spawn: " + SpawnId);
#endif
    }
}
