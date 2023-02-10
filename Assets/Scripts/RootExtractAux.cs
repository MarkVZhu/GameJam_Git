using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootExtractAux : MonoBehaviour
{
    public bool isAttach;

    void Start()
    {
        isAttach = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isAttach);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Extract"))
        {
            isAttach = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Extract"))
        {
            isAttach = false;
        }
    }
}
