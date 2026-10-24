using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 全局场景加载器（挂在常驻物体上，例如 ImportantResource）。
/// 负责：异步加载场景 → 等新场景初始化完 → 把玩家移动到指定出生点。
/// 不再锁身体、不再硬等时间。
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Tooltip("加载完成后是否把玩家传送到出生点")]
    public bool TeleportPlayerToSpawn = true;

    [Tooltip("找不到对应 SpawnId 时，是否退回到场景里的第一个 SpawnPoint")]
    public bool FallbackToFirstSpawnPoint = true;

    public bool IsLoading { get; private set; }
    public string PendingSpawnId { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    /// <summary>加载场景，并在加载完成后把玩家放到 spawnId 对应的出生点。</summary>
    public void LoadScene(string sceneName, string spawnId)
    {
        if (IsLoading)
            return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneLoader] sceneName 为空，已忽略。");
            return;
        }

        StartCoroutine(LoadRoutine(sceneName, spawnId));
    }

    IEnumerator LoadRoutine(string sceneName, string spawnId)
    {
        IsLoading = true;
        PendingSpawnId = spawnId;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        if (op == null)
        {
            Debug.LogError("[SceneLoader] 无法加载场景：" + sceneName + "（检查 Build Settings 里是否包含）");
            IsLoading = false;
            PendingSpawnId = null;
            yield break;
        }

        while (!op.isDone)
            yield return null;

        // 让新场景里的 Awake / Start / OnEnable 先跑完
        yield return null;

        if (TeleportPlayerToSpawn)
            TeleportPlayer(spawnId);

        // 背包里的物品模板（TheObj）是场景根物体，换场景时会被销毁，
        // 这里重新让 InventoryManager 通过 Addressables 把它们补回来。
        RepairInventory();

        yield return null;

        IsLoading = false;
        PendingSpawnId = null;
    }

    /// <summary>换场景后重建背包里丢失的物品模板对象（TheObj）。</summary>
    public void RepairInventory()
    {
        Player thePlayer = FindObjectOfType<Player>();
        if (thePlayer == null)
            return;

        InventoryManager im = thePlayer.GetComponent<InventoryManager>();
        if (im == null)
            return;

        im.MoniterTheList();
        Debug.Log("[SceneLoader] 已请求 InventoryManager 重建物品模板。");
    }

    /// <summary>把玩家移动到匹配的出生点。</summary>
    public void TeleportPlayer(string spawnId)
    {
        Player thePlayer = FindObjectOfType<Player>();
        if (thePlayer == null)
        {
            Debug.LogWarning("[SceneLoader] 新场景里找不到 Player。");
            return;
        }

        SpawnPoint target = FindSpawnPoint(spawnId);
        if (target == null)
        {
            // 没有出生点就保持原位，至少把速度和旋转清干净
            ResetPlayerPhysics(thePlayer, thePlayer.transform.position);
            Debug.LogWarning("[SceneLoader] 场景里没有可用的 SpawnPoint，玩家保持原位。");
            return;
        }

        ResetPlayerPhysics(thePlayer, target.transform.position);

        Debug.Log("[SceneLoader] 玩家已出生在 '" + target.gameObject.name + "' (SpawnId=" + target.SpawnId + ")");
    }

    SpawnPoint FindSpawnPoint(string spawnId)
    {
        SpawnPoint[] all = FindObjectsOfType<SpawnPoint>(true);
        if (all == null || all.Length == 0)
            return null;

        if (!string.IsNullOrEmpty(spawnId))
        {
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null && all[i].SpawnId == spawnId)
                    return all[i];
            }
        }

        if (FallbackToFirstSpawnPoint)
        {
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null)
                    return all[i];
            }
        }

        return null;
    }

    void ResetPlayerPhysics(Player thePlayer, Vector3 position)
    {
        position.z = thePlayer.transform.position.z;

        thePlayer.transform.position = position;

        Rigidbody2D body = null;

        if (thePlayer.BodyBox != null)
            body = thePlayer.BodyBox.attachedRigidbody;

        if (body == null)
            body = thePlayer.GetComponentInChildren<Rigidbody2D>();

        if (body != null)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.position = new Vector2(position.x, position.y);
            body.velocity = Vector2.zero;
            body.angularVelocity = 0f;
            body.rotation = 0f;
            body.freezeRotation = true;
        }

        // 常驻的脚部 IK 目标跟着一起挪，避免下一帧被扯回旧坐标
        SceneBackGroundTheme theme = FindObjectOfType<SceneBackGroundTheme>();
        if (theme != null)
            theme.SyncFootTargetsToPlayer();
    }
}
