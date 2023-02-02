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
    private bool UpCollide;
    private bool DownCollide;
    private bool LeftCollide;
    private bool RightCollide;

    // Start is called before the first frame update
    void Start()
    {
        canExtract = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpExtract()
    {

    }

    void DownExtract()
    {

    }

    void LeftExtract()
    {

    }

    void RightExtract()
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

            canExtract = false;

            StartCoroutine(ExtractionProcess(collision.transform.parent.gameObject));
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Extract"))
            canExtract = true;
    }

    //物体缓慢下降的过程
    IEnumerator ExtractionProcess(GameObject go)
    {
        while(go.transform.localScale.y > scaleTemp.y)
        {
            differenceTemp = go.transform.localScale.y -  Time.deltaTime;
            go.transform.localScale = new Vector3(scaleTemp.x, differenceTemp, scaleTemp.z);
            go.transform.position = new Vector3(positionTemp.x, go.transform.position.y - Time.deltaTime / 2, positionTemp.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
