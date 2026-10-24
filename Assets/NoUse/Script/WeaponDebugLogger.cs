using UnityEngine;

/// <summary>
/// 临时诊断脚本：定期把“切换武器”相关状态打到 Console。
/// 定位完问题后可以直接删掉这个组件。
/// </summary>
public class WeaponDebugLogger : MonoBehaviour
{
    [Tooltip("打印间隔（秒）")]
    public float Interval = 1f;

    [Tooltip("是否启用")]
    public bool Enable = true;

    float timer;

    void Update()
    {
        if (!Enable)
            return;

        timer += Time.deltaTime;
        if (timer < Interval)
            return;

        timer = 0f;
        Dump();
    }

    [ContextMenu("立即打印一次")]
    public void Dump()
    {
        string r = "=== [" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "] 武器状态 ===\n";

        Player player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.Log(r + "  找不到 Player");
            return;
        }

        InventoryManager im = player.GetComponent<InventoryManager>();
        UnableallPanel unable = FindObjectOfType<UnableallPanel>(true);

        if (unable == null)
            r += "  UnableallPanel: 找不到\n";
        else
            r += "  UnableallPanel go.active=" + unable.gameObject.activeInHierarchy
               + " enabled=" + unable.enabled
               + " selectedIndex=" + unable.ItemSlotSelectedIndex
               + " ParentPanel_MagazinePanel=" + (unable.ParentPanel_MagazinePanel != null ? "OK" : "NULL")
               + "\n";

        if (im == null || im.AllInventory_BaseItem == null)
        {
            r += "  InventoryManager: 找不到或列表为空\n";
        }
        else
        {
            r += "  背包槽位 (" + im.AllInventory_BaseItem.Count + "):\n";
            for (int i = 0; i < im.AllInventory_BaseItem.Count; i++)
            {
                var slot = im.AllInventory_BaseItem[i];
                if (slot == null || slot.TheThingsInSlot == null)
                {
                    r += "    [" + i + "] 空槽\n";
                    continue;
                }
                var t = slot.TheThingsInSlot;
                r += "    [" + i + "] count=" + t.Count
                   + " type=" + (t.TheBaseItem != null ? t.TheBaseItem.Type.ToString() : "null")
                   + " handle=" + (t.TheBaseItem != null ? t.TheBaseItem.HandleStyle.ToString() : "-")
                   + " TheObj=" + (t.TheObj != null ? t.TheObj.name : "NULL")
                   + "\n";
            }
        }

        HealthManager hm = player.GetComponent<HealthManager>();
        r += "  ThingInHand=" + (hm != null && hm.ThingInHand != null ? hm.ThingInHand.name : "NULL") + "\n";

        GetBigGun bg = FindObjectOfType<GetBigGun>(true);
        GetSmallGun sg = FindObjectOfType<GetSmallGun>(true);
        GetMeleeWeapon mw = FindObjectOfType<GetMeleeWeapon>(true);

        if (bg != null) r += "  GetBigGun: Equipeed=" + bg.Equipeed + " player=" + (bg.ThePlayer != null ? "OK" : "NULL") + "\n";
        if (sg != null) r += "  GetSmallGun: Equipeed=" + sg.Equipeed + " player=" + (sg.ThePlayer != null ? "OK" : "NULL") + "\n";
        if (mw != null) r += "  GetMeleeWeapon: Equipeed=" + mw.Equipeed + " player=" + (mw.ThePlayer != null ? "OK" : "NULL") + "\n";

        WatchMouse wm = FindObjectOfType<WatchMouse>(true);
        if (wm != null)
            r += "  Watch: IsTurned=" + wm.IsTurned + " HandIsUp=" + wm.HandIsUp + " HandIsUp2=" + wm.HandIsUp2 + "\n";

        Debug.Log(r);
    }
}
