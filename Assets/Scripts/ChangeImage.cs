using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChangeImage : MonoBehaviour
{
    public Sprite[] _myOtherSprites;
    public int spriteNumber = 3;

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
            spriteNumber = Random.Range(0, spriteNumber);
            _images[i].sprite = _myOtherSprites[spriteNumber];
        }
        yield return new WaitForSeconds(2);
    }
}