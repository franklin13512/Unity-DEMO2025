using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WatchMouse : MonoBehaviour
{
    public GameObject Player;
    public GameObject TargetItem;
    public Vector2 TheNormalScale;
    public BoxCollider2D BC;
    public DetachLimb[] Limbs;

    public Vector3 TheMouseCorodinate;
    public Vector2 TargetCorodinate;

    public Vector3 Distance;

    public bool IsTurned;
    public bool HandIsUp;
    public bool HandIsUp2;

    public float TheRadius;
    public GameObject UIPrefab;
    public GameObject FatherContent;
    public PickUp ThePickup;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        BC = TargetItem.gameObject.GetComponent<BoxCollider2D>();
        TheNormalScale = Player.transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        TheMouseCorodinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        TargetCorodinate = BC.bounds.center;

        if ((TheMouseCorodinate.x - BC.bounds.center.x >= 0) && IsTurned)
        {
            IsTurned = false;

            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].Detaching();
            }

            GameObject NewPlayer = new GameObject("Player00");

            NewPlayer.transform.position = new Vector2(TargetCorodinate.x, 0);

            Player.transform.position = NewPlayer.transform.position;
            Destroy(NewPlayer);

            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].GetParents(Player);
            }

            Player.transform.localScale = new Vector2(TheNormalScale.x, TheNormalScale.y);
        }
        else if (TheMouseCorodinate.x - BC.bounds.center.x < 0 && !IsTurned)
        {
            IsTurned = true;

            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].Detaching();
            }

            GameObject NewPlayer = new GameObject("Player00");

            NewPlayer.transform.position = new Vector2(TargetCorodinate.x, 0);

            Player.transform.position = NewPlayer.transform.position;
            Destroy(NewPlayer);

            for (int i = 0; i < Limbs.Length; i++)
            {
                Limbs[i].GetParents(Player);
            }

            Player.transform.localScale = new Vector2(-TheNormalScale.x, TheNormalScale.y);
        }
    }

    public void SearchObjs()
    {
        foreach (Transform child in FatherContent.transform)
        {
            Destroy(child.gameObject);
        }

        Collider2D[] TheItems = Physics2D.OverlapCircleAll(BC.bounds.center, TheRadius);

        foreach (Collider2D col in TheItems)
        {
            Item TheDetectedItem = col.gameObject.GetComponent<Item>();
            if (TheDetectedItem != null && !TheDetectedItem.IsEquiped)
            {
                GameObject TheUI = Instantiate(UIPrefab, FatherContent.transform);
                Roll TheRoll = TheUI.gameObject.GetComponent<Roll>();
                if (TheRoll != null)
                {
                    byte[] imageData = File.ReadAllBytes(TheDetectedItem.TheBasicData.TheSprite_Path);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);

                    TheRoll.TheUIImage.sprite = ConvertWholeTextureToSprite(texture);
                    TheRoll.TheItemText.text = TheDetectedItem.TheBasicData.UniqueCode;
                    TheRoll.TheRelatedItem = TheDetectedItem;

                    Button TheButton = TheRoll.gameObject.GetComponent<Button>();
                    if (TheButton != null)
                    {
                        TheButton.onClick.AddListener(() => ThePickup.PickUpBaseItem_UI(TheRoll.TheRelatedItem.TheBasicData, TheRoll.TheRelatedItem.gameObject));
                        TheButton.onClick.AddListener(SearchObjs);
                    }
                }
            }
        }
    }

    public Sprite ConvertWholeTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height), // 整张图区域
            new Vector2(0.5f, 0.5f)                        // 锚点居中
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, TheRadius);
    }
}
