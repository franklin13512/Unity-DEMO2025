using UnityEngine;

/// <summary>
/// 场景传送门：玩家在触发器内按 Interact 键 → 加载指定场景，并在目标场景的指定出生点复活。
/// 兼容两种判定：2D 触发器，以及“距离兜底”（防止层碰撞矩阵把触发器挡掉）。
/// </summary>
public class ScenePortal : MonoBehaviour
{
    [Header("目标")]
    [Tooltip("要加载的场景名（必须已加入 Build Settings，不含 .unity）")]
    public string SceneName = "";

    [Tooltip("目标场景里的出生点 SpawnId；留空则用第一个 SpawnPoint")]
    public string SpawnId = "Default";

    [Header("交互")]
    [Tooltip("玩家所在 Tag")]
    public string PlayerTag = "Player";

    [Tooltip("直接检测的按键（默认 F）")]
    public KeyCode InteractKey = KeyCode.F;

    [Tooltip("同时尝试 Input Manager 里的 Interact 按钮")]
    public bool AlsoUseInteractButton = true;

    [Tooltip("是否提示（在 Console 打印一次）")]
    public bool LogOnUse = true;

    [Header("兜底判定")]
    [Tooltip("触发器不生效时，用距离判定代替（推荐开启）")]
    public bool UseDistanceFallback = true;

    [Tooltip("距离判定半径")]
    public float InteractRadius = 1.2f;

    bool playerInside;
    bool interactButtonChecked;
    bool hasInteractButton;

    void Reset()
    {
        // 加上组件时自动把碰撞体设为触发器
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
            playerInside = false;
    }

    void OnDisable()
    {
        playerInside = false;
    }

    void Update()
    {
        if (SceneLoader.Instance == null)
            return;

        if (SceneLoader.Instance.IsLoading)
            return;

        if (!playerInside && !IsPlayerNearby())
            return;

        if (!InteractPressed())
            return;

        if (LogOnUse)
            Debug.Log("[ScenePortal] 进入场景 '" + SceneName + "'，出生点 '" + SpawnId + "'");

        SceneLoader.Instance.LoadScene(SceneName, SpawnId);
    }

    bool IsPlayer(Collider2D other)
    {
        if (other == null)
            return false;

        if (!string.IsNullOrEmpty(PlayerTag) && other.CompareTag(PlayerTag))
            return true;

        // 有些子碰撞体可能没打 Tag，向上找一下
        Player found = other.GetComponentInParent<Player>();
        return found != null;
    }

    bool IsPlayerNearby()
    {
        if (!UseDistanceFallback)
            return false;

        Player thePlayer = FindObjectOfType<Player>();
        if (thePlayer == null)
            return false;

        Vector2 center = thePlayer.transform.position;

        if (thePlayer.BodyBox != null)
            center = thePlayer.BodyBox.bounds.center;

        return Vector2.Distance(center, transform.position) <= InteractRadius;
    }

    bool InteractPressed()
    {
        if (InteractKey != KeyCode.None && Input.GetKeyDown(InteractKey))
            return true;

        if (!AlsoUseInteractButton)
            return false;

        if (!interactButtonChecked)
        {
            interactButtonChecked = true;
            try
            {
                Input.GetButton("Interact");
                hasInteractButton = true;
            }
            catch
            {
                hasInteractButton = false;
            }
        }

        if (!hasInteractButton)
            return false;

        try
        {
            return Input.GetButtonDown("Interact");
        }
        catch
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, InteractRadius);
    }
}
