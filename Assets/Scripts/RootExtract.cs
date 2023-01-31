using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootExtract : MonoBehaviour
{
    private Vector3 scaleTemp;
    private Vector3 positionTemp;

    private bool canExtract;

    // Start is called before the first frame update
    void Start()
    {
        canExtract = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Extractable") && canExtract)
        {
            scaleTemp = collision.transform.localScale;
            positionTemp = collision.transform.position;
            collision.transform.localScale = new Vector3(scaleTemp.x, scaleTemp.y/2, scaleTemp.z);
            collision.transform.position = new Vector3(positionTemp.x, positionTemp.y - scaleTemp.y / 4, positionTemp.z);
            canExtract = false;
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Extractable"))
            canExtract = true;
    }
}
