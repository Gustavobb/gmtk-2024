using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCounter : MonoBehaviour
{
    [SerializeField] private float _liveImageWidth = 20;
    [SerializeField] private RectTransform plusRect, minusRect;
    [SerializeField] private int plusCount, minusCount;
        
    private void Start()
    {
        // plusRect.sizeDelta = new Vector2(0, plusRect.sizeDelta.y);
        // minusRect.sizeDelta = new Vector2(0, minusRect.sizeDelta.y);
        BuildUI();
    }

    private void BuildUI()
    {
        AdjustPlusWidth(plusCount);
        AdjustMinusWidth(minusCount);
    }

    public void UsePlusBullet()
    {
        if (plusCount > 0)
        {
            plusCount--;
            AdjustPlusWidth(plusCount);
        }
    }

    public void UseMinusBullet()
    {
        if (minusCount > 0)
        {
            minusCount--;
            AdjustMinusWidth(minusCount);
        }
    }

    public void AddPlusBullets(int numberOfBullets)
    {
        plusCount += numberOfBullets;
        AdjustPlusWidth(plusCount);
    }

    public void AddMinusBullets(int numberOfBullets)
    {
        minusCount += numberOfBullets;
        AdjustMinusWidth(minusCount);
    }
    public bool HasPlusBullet()
    {
        return plusCount > 0;
    }

    public bool HasMinusBullet()
    {
        return minusCount > 0;
    }

    public void AdjustPlusWidth(int numberOfBullets){
        plusRect.sizeDelta = new Vector2(_liveImageWidth * numberOfBullets, plusRect.sizeDelta.y);
    }

    public void AdjustMinusWidth(int numberOfBullets){
        minusRect.sizeDelta = new Vector2(_liveImageWidth * numberOfBullets, minusRect.sizeDelta.y);
    }
}
