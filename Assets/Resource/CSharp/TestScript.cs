using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public SpriteRenderer HelmetSprite;

    public SpriteRenderer BodySprite;
    public SpriteRenderer[] UpperArmSprite;
    public SpriteRenderer[] LowerArmSprite;

    public SpriteRenderer HipSprite;
    public SpriteRenderer[] LapSprite;
    public SpriteRenderer[] LegSprite;

    public SpriteRenderer[] ShoesSprite;

    public SpriteRenderer ArmorSprite;

    [Space]
    public SpriteRenderer HelmetSprite_F;

    public SpriteRenderer BodySprite_F;
    public SpriteRenderer[] UpperArmSprite_F;
    public SpriteRenderer[] LowerArmSprite_F;

    public SpriteRenderer HipSprite_F;
    public SpriteRenderer[] LapSprite_F;
    public SpriteRenderer[] LegSprite_F;

    public SpriteRenderer[] ShoesSprite_F;

    public SpriteRenderer ArmorSprite_F;

    [Space]
    public SpriteRenderer HelmetSprite_Orignal;

    public SpriteRenderer BodySprite_Orignal;
    public SpriteRenderer[] UpperArmSprite_Orignal;
    public SpriteRenderer[] LowerArmSprite_Orignal;

    public SpriteRenderer HipSprite_Orignal;
    public SpriteRenderer[] LapSprite_Orignal;
    public SpriteRenderer[] LegSprite_Orignal;

    public SpriteRenderer[] ShoesSprite_Orignal;

    public SpriteRenderer ArmorSprite_Orignal;

    [Space]
    public bool IsFemale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter()
    {
        if (IsFemale == true)
        {
            HelmetSprite_Orignal = HelmetSprite;

            BodySprite_Orignal = BodySprite;
            for (int i = 0; i < 2; i++)
            {
                UpperArmSprite_Orignal[i] = UpperArmSprite[i];
            }
            for (int i = 0; i < 2; i++)
            {
                LowerArmSprite_Orignal[i] = LowerArmSprite[i];
            }

            HipSprite_Orignal = HipSprite;
            for (int i = 0; i < 2; i++)
            {
                LapSprite_Orignal[i] = LapSprite[i];
            }
            for (int i = 0; i < 2; i++)
            {
                LegSprite_Orignal[i] = LapSprite[i];
            }

            for (int i = 0; i < 2; i++)
            {
                ShoesSprite_Orignal[i] = ShoesSprite[i];
            }
        }else if (IsFemale == false)
        {
            HelmetSprite_Orignal = HelmetSprite_F;

            BodySprite_Orignal = BodySprite_F;
            for (int i = 0; i < 2; i++)
            {
                UpperArmSprite_Orignal[i] = UpperArmSprite_F[i];
            }
            for (int i = 0; i < 2; i++)
            {
                LowerArmSprite_Orignal[i] = LowerArmSprite_F[i];
            }

            HipSprite_Orignal = HipSprite_F;
            for (int i = 0; i < 2; i++)
            {
                LapSprite_Orignal[i] = LapSprite_F[i];
            }
            for (int i = 0; i < 2; i++)
            {
                LegSprite_Orignal[i] = LapSprite_F[i];
            }

            for (int i = 0; i < 2; i++)
            {
                ShoesSprite_Orignal[i] = ShoesSprite_F[i];
            }
        }
    }
}
