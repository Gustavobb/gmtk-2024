using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCounter : MonoBehaviour
{
    [SerializeField] private float _liveImageWidth = 20;
    [SerializeField] private RectTransform plusRect, minusRect;

    public void AdjustPlusWidth(int numberOfBullets){
        plusRect.sizeDelta = new Vector2(_liveImageWidth * numberOfBullets, plusRect.sizeDelta.y);
    }

    public void AdjustMinusWidth(int numberOfBullets){
        minusRect.sizeDelta = new Vector2(_liveImageWidth * numberOfBullets, minusRect.sizeDelta.y);
    }
}
