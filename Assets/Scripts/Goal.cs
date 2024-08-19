using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openedSprite;
    [SerializeField] private GameObject portal;

    private void Start()
    {
        portal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            spriteRenderer.sprite = openedSprite;
            portal.SetActive(true);
            StartCoroutine(WaitAndLoadNextLevel());
        }
    }

    private IEnumerator WaitAndLoadNextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.LoadNextLevel();
    }
}
