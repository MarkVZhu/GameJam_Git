using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptControl : MonoBehaviour
{
    public GameObject textClose;
    public GameObject textOpen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(textClose)
                textClose.SetActive(false);
            if(textOpen)
                textOpen.SetActive(true);
        }
    }
}
