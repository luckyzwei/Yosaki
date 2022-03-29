using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DamTextType
{
    Normal, Critical, Green, Red, Penetration, SuperCritical
}

public class DamageText : PoolItem
{
    [SerializeField]
    private TextMeshPro damageText;

    [SerializeField]
    private float disableTime = 10f;

    private readonly string DisableFuncName = "DisableObject";

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color criticalColor;

    [SerializeField]
    private Color superCriticalColor;

    [SerializeField]
    private Color redColor;

    [SerializeField]
    private Color greenColor;

    [SerializeField]
    private Color ignoreDefenseColor;

    //[SerializeField]
    //private TMP_FontAsset normalfont;

    //[SerializeField]
    //private TMP_FontAsset ciriticalfont;

    //[SerializeField]
    //private GameObject criticalIcon;

    //[SerializeField]
    //private VertexGradient greenGradient;
    //[SerializeField]
    //private VertexGradient whiteGradient;
    //[SerializeField]
    //private VertexGradient redGradient;

    [SerializeField]
    private Animator animator;

    //   private readonly string Format = "N1";

    public void Initialize(double damage, DamTextType type = DamTextType.Normal)
    {
        SetColor(type);


        damage *= DamageBalance.GetRandomDamageRange();
        //SetColor(type);

        //criticalIcon.SetActive(isCritical);

        damageText.SetText(Utils.ConvertBigNum(damage));

        Invoke(DisableFuncName, disableTime);
    }


    private void SetColor(DamTextType type)
    {
        switch (type)
        {
            case DamTextType.Normal:
                damageText.color = normalColor;
                break;
            case DamTextType.Green:
                damageText.color = greenColor;
                break;
            case DamTextType.Red:
                damageText.color = redColor;
                break;
            case DamTextType.Critical:
                damageText.color = criticalColor;
                break;
            case DamTextType.SuperCritical:
                damageText.color = superCriticalColor;
                break;
        }
    }

    private void DisableObject()
    {
        this.gameObject.SetActive(false);
    }
}
