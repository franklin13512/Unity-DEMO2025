using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 方案二：背景板常驻不销毁，进入不同场景时只替换背景的贴图/颜色。
/// 顺带处理：换场景后把 P1P / P2P（脚部 IK 目标）拉回玩家附近，避免被扯到旧场景坐标。
/// </summary>
public class SceneBackGroundTheme : MonoBehaviour
{
    [System.Serializable]
    public class Theme
    {
        [Tooltip("场景名（Build Settings 里的名字，不含 .unity）")]
        public string SceneName;

        [Header("远景 Sky")]
        public Sprite SkySprite;
        public Color SkyColor = Color.white;

        [Header("前层视差 Front")]
        public Sprite FrontSprite;
        public Color FrontColor = Color.white;

        [Header("后层视差 Back")]
        public Sprite BackSprite;
        public Color BackColor = Color.white;
    }

    [Tooltip("CameraMove（留空会自动在本物体上找）")]
    public CameraMove TheCamera;

    [Tooltip("每个场景对应的一组背景设置；找不到对应场景时保持原样")]
    public Theme[] Themes;

    [Header("脚部 IK 目标同步")]
    [Tooltip("换场景后把 P1P / P2P 拉回玩家附近")]
    public bool SyncFootTargets = true;
    [Tooltip("两个脚部目标相对玩家的左右偏移")]
    public float FootTargetSpacing = 0.12f;

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void Start()
    {
        ApplyTheme(SceneManager.GetActiveScene().name);
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyTheme(scene.name);

        if (SyncFootTargets)
            SyncFootTargetsToPlayer();
    }

    /// <summary>按场景名应用背景主题；没有匹配项时不做任何改动。</summary>
    public void ApplyTheme(string sceneName)
    {
        if (TheCamera == null)
            TheCamera = GetComponent<CameraMove>();
        if (TheCamera == null || Themes == null)
            return;

        Theme theme = null;
        for (int i = 0; i < Themes.Length; i++)
        {
            if (Themes[i] != null && Themes[i].SceneName == sceneName)
            {
                theme = Themes[i];
                break;
            }
        }

        if (theme == null)
            return;

        SetRenderer(TheCamera.DeepBackGround, theme.SkySprite, theme.SkyColor);

        SetRenderer(TheCamera.Front1, theme.FrontSprite, theme.FrontColor);
        SetRenderer(TheCamera.Front2, theme.FrontSprite, theme.FrontColor);
        SetRenderer(TheCamera.Front3, theme.FrontSprite, theme.FrontColor);
        SetRenderer(TheCamera.CurrentFront, theme.FrontSprite, theme.FrontColor);

        SetRenderer(TheCamera.Back1, theme.BackSprite, theme.BackColor);
        SetRenderer(TheCamera.Back2, theme.BackSprite, theme.BackColor);
        SetRenderer(TheCamera.Back3, theme.BackSprite, theme.BackColor);
        SetRenderer(TheCamera.CurrentBack, theme.BackSprite, theme.BackColor);
    }

    void SetRenderer(SpriteRenderer target, Sprite sprite, Color color)
    {
        if (target == null)
            return;

        if (sprite != null)
            target.sprite = sprite;

        target.color = color;
    }

    /// <summary>把常驻的 P1P / P2P 拉回当前玩家附近。</summary>
    public void SyncFootTargetsToPlayer()
    {
        Player thePlayer = FindObjectOfType<Player>();
        if (thePlayer == null || thePlayer.BodyBox == null)
            return;

        Vector3 center = thePlayer.BodyBox.bounds.center;

        Vector3 left = center + new Vector3(-FootTargetSpacing, 0f, 0f);
        Vector3 right = center + new Vector3(FootTargetSpacing, 0f, 0f);
        left.z = 0f;
        right.z = 0f;

        Transform p1 = FindDeepByName("P1P");
        if (p1 != null) p1.position = left;

        Transform p2 = FindDeepByName("P2P");
        if (p2 != null) p2.position = right;
    }

    /// <summary>P1P/P2P 是常驻物体，用 GameObject.Find 有时受激活状态影响，这里做个兜底扫描。</summary>
    Transform FindDeepByName(string targetName)
    {
        GameObject go = GameObject.Find(targetName);
        if (go != null)
            return go.transform;

        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name == targetName)
                return root.transform;

            Transform found = FindInChildren(root.transform, targetName);
            if (found != null)
                return found;
        }

        return null;
    }

    Transform FindInChildren(Transform parent, string targetName)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == targetName)
                return child;

            Transform deeper = FindInChildren(child, targetName);
            if (deeper != null)
                return deeper;
        }

        return null;
    }

    [ContextMenu("立即应用当前场景主题")]
    public void ApplyCurrentSceneTheme()
    {
        ApplyTheme(SceneManager.GetActiveScene().name);
    }
}
