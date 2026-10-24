using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIconManager : MonoBehaviour
{
    [Tooltip("预制体列表（原来从 Player 继承）")]
    public PrefabList ThePrefabList;

    public StatusManager TheStatusManager;
    public TakeDamage[] TheTakeDamage;
    public GameObject TheStatusPanel;

    private bool WhetherCollapse;
    private bool WhetherInternalInjures;
    private bool WhetherBleeding;
    private bool WhetherBurn;
    private bool WhetherScabbed;
    private bool WhetherGunShot;
    private bool WhetherCut;
    private bool WhetherBruis;
    private bool WhetherExciting;
    private bool WhetherListless;
    private bool WhetherGasPoisoning;
    private bool WhetherScratch;
    private bool WhetherBiteInjury;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TheStatusManager != null)
        {
            CollapseFunc();
            InteralInjuresFunc();
            BleedingFunc();
            BurnFunc();
            ScabbedFunc();
            GunShotFunc();
            TheCutFunc();
            TheBruiseFunc();
            TheExcitementFunc();
            TheListlessFunc();
            TheGasPoisoningFunc();
            TheScratchFunc();
            BiteInjuryFunc();
        }
    }

    public void BiteInjuryFunc()
    {
        if (TheStatusManager.NumberOfBiteInjury > 0 && WhetherBiteInjury == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "BiteInjury")
                {
                    WhetherBiteInjury = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "BiteInjury")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherBiteInjury = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfBiteInjury <= 0 && WhetherBiteInjury == true)//ע�����
        {
            WhetherBiteInjury = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "BiteInjury")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherBiteInjury = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheScratchFunc()
    {
        if (TheStatusManager.NumberOfScratch > 0 && WhetherScratch == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scratch")
                {
                    WhetherScratch = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scratch")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherScratch = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfScratch <= 0 && WhetherScratch == true)//ע�����
        {
            WhetherScratch = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scratch")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherScratch = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheGasPoisoningFunc()
    {
        if (TheStatusManager.IsGasPoisoning == true && WhetherGasPoisoning == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GasPoisoning")
                {
                    WhetherGasPoisoning = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GasPoisoning")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherGasPoisoning = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.IsGasPoisoning == false && WhetherGasPoisoning == true)//ע�����
        {
            WhetherGasPoisoning = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GasPoisoning")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherGasPoisoning = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheListlessFunc()
    {
        if (TheStatusManager.IsListless == true && WhetherListless == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Listless")
                {
                    WhetherListless = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Listless")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherListless = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.IsListless == false && WhetherListless == true)//ע�����
        {
            WhetherListless = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Listless")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherListless = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheExcitementFunc()
    {
        if (TheStatusManager.IsExcitement == true && WhetherExciting == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Excitement")
                {
                    WhetherExciting = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Excitement")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherExciting = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.IsExcitement == false && WhetherExciting == true)//ע�����
        {
            WhetherExciting = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Excitement")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherExciting = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheBruiseFunc()
    {
        if (TheStatusManager.NumberOfBruise > 0 && WhetherBruis == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bruise")
                {
                    WhetherBruis = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bruise")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherBruis = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfBruise <= 0 && WhetherBruis == true)//ע�����
        {
            WhetherBruis = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bruise")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherBruis = false;//ע�����
                    return;
                }
            }
        }
    }
    public void TheCutFunc()
    {
        if (TheStatusManager.NumberOfCut > 0 && WhetherCut == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Cut")
                {
                    WhetherCut = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Cut")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherCut = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfCut <= 0 && WhetherCut == true)//ע�����
        {
            WhetherGunShot = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Cut")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherCut = false;//ע�����
                    return;
                }
            }
        }
    }
    public void GunShotFunc()
    {
        if (TheStatusManager.NumberOfGunShot > 0 && WhetherGunShot == false)//ע�����
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GunShot")
                {
                    WhetherGunShot = true;//ע�����
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GunShot")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherGunShot = true;//ע�����
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfGunShot <= 0 && WhetherGunShot == true)//ע�����
        {
            WhetherGunShot = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "GunShot")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherGunShot = false;//ע�����
                    return;
                }
            }
        }
    }
    public void ScabbedFunc()
    {
        if (TheStatusManager.IsScabbed == true && WhetherScabbed == false)
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scabbed")
                {
                    WhetherScabbed = true;
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scabbed")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherScabbed = true;
                    return;
                }
            }
        }
        else if (TheStatusManager.IsScabbed == false && WhetherScabbed == true)
        {
            WhetherScabbed = false; //ע�����
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Scabbed")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherScabbed = false;
                    return;
                }
            }
        }
    }
    public void BurnFunc()
    {
        if (TheStatusManager.NumberOfBurn > 0 && WhetherBurn == false)
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Burn")
                {
                    WhetherBurn = true;
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Burn")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherBurn = true;
                    return;
                }
            }
        }
        else if (TheStatusManager.NumberOfBurn <= 0 && WhetherBurn == true)
        {
            WhetherBurn = false;
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Burn")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherBurn = false;
                    return;
                }
            }
        }
    }
    public void BleedingFunc()
    {
        if (TheStatusManager.BleedingTime > 0 && WhetherBleeding == false)
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bleeding")
                {
                    WhetherBleeding = true;
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bleeding")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherBleeding = true;
                    return;
                }
            }
        }
        else if (TheStatusManager.BleedingTime <= 0 && WhetherBleeding == true)
        {
            WhetherBleeding = false;

            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Bleeding")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherBleeding = false;
                    return;
                }
            }
        }
    }
    public void CollapseFunc()
    {
        if (TheStatusManager.IsCollapse == true && WhetherCollapse == false)
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Collapse")
                {
                    WhetherCollapse = true;
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Collapse")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherCollapse = true;
                    return;
                }
            }
        }
        else if (TheStatusManager.IsCollapse == false && WhetherCollapse == true)
        {
            WhetherCollapse = false;
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "Collapse")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherCollapse = false;
                    return;
                }
            }
        }
    }
    public void InteralInjuresFunc()
    {
        if (TheStatusManager.IsInternalInjures == true && WhetherInternalInjures == false)
        {
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "InternalInjures")
                {
                    WhetherInternalInjures = true;
                    return;
                }
            }

            for (int i = 0; i < ThePrefabList.StatusIcon.Length; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.StatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "InternalInjures")
                {
                    GameObject TheIcon = Instantiate(ThePrefabList.StatusIcon[i].gameObject, transform.position, Quaternion.identity);
                    TheIcon.transform.SetParent(TheStatusPanel.transform);
                    ThePrefabList.ExistedStatusIcon.Add(TheIcon);
                    WhetherInternalInjures = true;
                    return;
                }
            }
        }
        else if (TheStatusManager.IsInternalInjures == false && WhetherInternalInjures == true)
        {
            WhetherInternalInjures = false;
            for (int i = 0; i < ThePrefabList.ExistedStatusIcon.Count; i++)
            {
                StatusIconInformation TheIconInformation = ThePrefabList.ExistedStatusIcon[i].gameObject.GetComponent<StatusIconInformation>();
                if (TheIconInformation != null && TheIconInformation.StatusCode == "InternalInjures")
                {
                    GameObject DeleteIcon = ThePrefabList.ExistedStatusIcon[i].gameObject;
                    ThePrefabList.ExistedStatusIcon.Remove(ThePrefabList.ExistedStatusIcon[i].gameObject);
                    Destroy(DeleteIcon);
                    WhetherInternalInjures = false;
                    return;
                }
            }
        }
    }
}
