using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;

    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(0).gameObject;//ͨ��������ȡǽ�ڻ�ͼ���ڵĶ���
        //Debug.Log(mapSprite.name);
        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
        }
    }
}
