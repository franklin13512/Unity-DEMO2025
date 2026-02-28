using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D;
using static UnityEngine.Rendering.DebugUI;

public class Structure : MonoBehaviour
{
    public float StructureDurability;
    public Material TheD2DMaterial;
    public bool CanbeScattered;

    private bool HasFractured;

    public bool WasDead;

    public bool IsOpen;

    // Start is called before the first frame update
    void Start()
    {
        D2dDestructibleSprite TheSprite = gameObject.GetComponent<D2dDestructibleSprite>();
        if (TheSprite)
        {

            if (StructureDurability <= 0)
            {
                D2dDestroyer TheDes = gameObject.GetComponent<D2dDestroyer>();
                if (TheDes != null)
                    TheDes.enabled = true;
            }
            else
            {
                BoxCollider2D TheBox = gameObject.AddComponent<BoxCollider2D>();
                TheBox.isTrigger = true;
            }
        }

    }

    public void TakeDamage_ST(float Damage)
    {
        StructureDurability -= Damage;

        if (StructureDurability <= 0 && HasFractured == false && CanbeScattered == true)
        {
            HasFractured = true;

            SpriteRenderer TheSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            TheSpriteRenderer.material = TheD2DMaterial;

            D2dFracturer TheFracturer = gameObject.GetComponent<D2dFracturer>();
            D2dDestroyer TheDes = gameObject.GetComponent<D2dDestroyer>();
            if (TheFracturer != null)
                TheFracturer.TryFracture();
            if (TheDes != null)
                TheDes.enabled = true;
        }
    }

    public virtual void Destruction()
    {
        return;
    }

    public virtual void Interact()
    {
        return;
    }
}
