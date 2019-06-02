using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChangeImage : MonoBehaviour
{
    public Sprite[] _myOtherSprites;
    public int spriteNumber = 3;
    public int spriteChoice;

    private SpriteRenderer[] _images;

    void Start()
    {
        _images = gameObject.GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(Count());
    }
    IEnumerator Count()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            spriteChoice = Random.Range(0, spriteNumber);
            _images[i].sprite = _myOtherSprites[spriteChoice];
        }
        yield return new WaitForSeconds(2);
    }
}