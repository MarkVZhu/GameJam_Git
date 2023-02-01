using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootExtract : MonoBehaviour
{
    private Vector3 scaleTemp;
    private Vector3 positionTemp;
    private string collidionName;
    private float differenceTemp;

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
        if (collision.CompareTag("Extract") && canExtract)
        {
            scaleTemp = collision.transform.parent.localScale;
            positionTemp = collision.transform.parent.position;


            collidionName = collision.gameObject.name;
            //Debug.Log(collision.gameObject.name);
            switch (collidionName)
            {
                case "Up":
                    differenceTemp = scaleTemp.y - Mathf.Sqrt(scaleTemp.y);
                    scaleTemp.y = Mathf.Sqrt(scaleTemp.y);
                    Debug.Log(scaleTemp.y/4);
                    positionTemp.y = positionTemp.y - differenceTemp/2;
                    break;
                case "Down":
                    //scaleTemp.y = scaleTemp.y / 2;
                    //positionTemp.y = positionTemp.y + scaleTemp.y / 4;
                    break;
                case "Left":
                    break;
                case "Right":
                    break;
                default:
                    break;
            }

            collision.transform.parent.transform.localScale = new Vector3(scaleTemp.x, scaleTemp.y, scaleTemp.z);
            collision.transform.parent.transform.position = new Vector3(positionTemp.x, positionTemp.y, positionTemp.z);
            canExtract = false;
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Extract"))
            canExtract = true;
    }
}
