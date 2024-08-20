using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public enum ButtonType
    {
        Grower,
        Shrinker
    }

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private ScalableObject scalable;
    [SerializeField] private Animation animator;
    private bool stopWatchCanPress = true;
    private float stopWatchTime = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp") || !stopWatchCanPress) return;
        Press(scalable);
    }

    private void Press(ScalableObject scalable)
    {
        switch (buttonType)
        {
            case ButtonType.Grower:
                scalable.ScaleUpQueue();
                break;
            case ButtonType.Shrinker:
                scalable.ScaleDownQueue();
                break;
        }

        animator.Play();
        StartCoroutine(StopWatch());
        SoundManager.instance.Play("Button");
    }

    private IEnumerator StopWatch()
    {
        stopWatchCanPress = false;
        yield return new WaitForSeconds(stopWatchTime);
        stopWatchCanPress = true;
    }
}
