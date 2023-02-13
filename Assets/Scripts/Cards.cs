using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    private bool isGet;
    [SerializeField]private Transform slot;//连接卡槽控制器
    [SerializeField] private float power;//倍率

    private void Update()
    {
        if (isGet)
        {
            slot.GetComponent<SlotController>().createCard(power);
            isGet = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isGet = true;
        }
    }
}
