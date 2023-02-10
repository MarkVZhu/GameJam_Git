using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootExtract : MonoBehaviour
{
    private Vector3 scaleTemp;
    private Vector3 positionTemp;
    private string collidionName;

    [SerializeField]private GameObject auxCollider;

    private float differenceTemp;
    public bool canMove;
    public bool canExtract;

    public float powNumber = 2f;
    private float multiplier;

    public float changeSpeed;
    private float changeSpeedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        canExtract = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(changeSpeedMultiplier);
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

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.transform.parent && collision.CompareTag("Extract") && canExtract)
        {
            scaleTemp = collision.transform.parent.localScale;
            positionTemp = collision.transform.parent.position;

            collidionName = collision.gameObject.name;

            switch (collidionName)
            {
                case "Up":
                    if(auxCollider.GetComponent<RootExtractAux>().isAttach)
                    {
                        if(powNumber > 1)
                        {
                            differenceTemp = scaleTemp.y - Mathf.Sqrt(scaleTemp.y);
                            changeSpeedMultiplier = differenceTemp / 10;
                            multiplier = -1.0f;
                            scaleTemp.y = Mathf.Sqrt(scaleTemp.y);
                            positionTemp.y = positionTemp.y - differenceTemp / 2;
                        }
                        else
                        {
                            differenceTemp = Mathf.Pow(scaleTemp.y, 1 / powNumber) - scaleTemp.y;
                            changeSpeedMultiplier = differenceTemp / 10;
                            multiplier = 1.0f;
                            scaleTemp.y = Mathf.Pow(scaleTemp.y, 1/powNumber);
                            positionTemp.y = positionTemp.y - differenceTemp / 2;
                        }
                        positionTemp.y = positionTemp.y + multiplier * differenceTemp / 2;

                        canMove = false;
                        canExtract = false;
                        StartCoroutine(ExtractionProcessU(collision.transform.parent.gameObject, powNumber < 1));
                        
                    }
                    break;
                case "Down":
                    //scaleTemp.y = scaleTemp.y / 2;
                    //positionTemp.y = positionTemp.y + scaleTemp.y / 4;
                    break;
                case "Left":
                    if (auxCollider.GetComponent<RootExtractAux>().isAttach)
                    {
                        if (powNumber > 1)
                        {
                            differenceTemp = scaleTemp.x - Mathf.Sqrt(scaleTemp.x);
                            changeSpeedMultiplier = differenceTemp / 10;
                            scaleTemp.x = Mathf.Sqrt(scaleTemp.x);
                            positionTemp.x = positionTemp.x - differenceTemp / 2;
                        }
                        else
                        {
                            differenceTemp = Mathf.Pow(scaleTemp.x, 1/powNumber) - scaleTemp.x;
                            changeSpeedMultiplier = differenceTemp / 10;
                            scaleTemp.x = Mathf.Pow(scaleTemp.x, 1/powNumber);
                            positionTemp.x = positionTemp.x + differenceTemp / 2;
                        }

                        canMove = false;
                        canExtract = false;
                        StartCoroutine(ExtractionProcessL(collision.transform.parent.gameObject, powNumber < 1));
                        
                    }
                    break;
                case "Right":
                    if (auxCollider.GetComponent<RootExtractAux>().isAttach)
                    {
                        if(powNumber > 1)
                        {
                            differenceTemp = scaleTemp.x - Mathf.Sqrt(scaleTemp.x);
                            changeSpeedMultiplier = differenceTemp / 10;
                            scaleTemp.x = Mathf.Sqrt(scaleTemp.x);
                            positionTemp.x = positionTemp.x - differenceTemp / 2;
                        }
                        else
                        {
                            differenceTemp = Mathf.Pow(scaleTemp.x, 1/powNumber) - scaleTemp.x;
                            changeSpeedMultiplier = differenceTemp / 10;
                            scaleTemp.x = Mathf.Pow(scaleTemp.x, 1 / powNumber);
                            positionTemp.x = positionTemp.x + differenceTemp / 2;
                        }

                        canMove = false;
                        canExtract = false;
                        StartCoroutine(ExtractionProcessR(collision.transform.parent.gameObject, powNumber < 1));
                        
                    }
                    break;
                default:
                    break;
            }

            //StartCoroutine(ExtractionProcessY(collision.transform.parent.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Extract"))
            canExtract = true;
    }

    //物体缓慢下降/上升的过程
    IEnumerator ExtractionProcessU(GameObject go, bool isRaise)
    {
        if(!isRaise)
            while (go.transform.localScale.y > scaleTemp.y)
            {
                differenceTemp = go.transform.localScale.y - Time.deltaTime * changeSpeed* changeSpeedMultiplier;
                go.transform.localScale = new Vector3(scaleTemp.x, differenceTemp, scaleTemp.z);
                go.transform.position = new Vector3(positionTemp.x, go.transform.position.y - Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        else 
        {
            while (go.transform.localScale.y < scaleTemp.y)
            {
                differenceTemp = go.transform.localScale.y + Time.deltaTime * changeSpeed * changeSpeedMultiplier;
                go.transform.localScale = new Vector3(scaleTemp.x, differenceTemp, scaleTemp.z);
                go.transform.position = new Vector3(positionTemp.x, go.transform.position.y + Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        canMove = true;
    }

    //物体缓慢拉长/缩短的过程
    IEnumerator ExtractionProcessL(GameObject go, bool isEnlongate)
    {
        if (!isEnlongate)
        {
            while (go.transform.localScale.x > scaleTemp.x)
            {
                differenceTemp = go.transform.localScale.x - Time.deltaTime * changeSpeed * changeSpeedMultiplier;
                go.transform.localScale = new Vector3(differenceTemp, scaleTemp.y, scaleTemp.z);
                go.transform.position = new Vector3(go.transform.position.x - Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.y, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (go.transform.localScale.x < scaleTemp.x)
            {
                differenceTemp = go.transform.localScale.x + Time.deltaTime * changeSpeed * changeSpeedMultiplier;
                go.transform.localScale = new Vector3(differenceTemp, scaleTemp.y, scaleTemp.z);
                go.transform.position = new Vector3(go.transform.position.x + Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.y, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        canMove = true;
    }

    IEnumerator ExtractionProcessR(GameObject go, bool isEnlongate)
    {
        if (!isEnlongate)
        {
            while (go.transform.localScale.x > scaleTemp.x)
            {
                differenceTemp = go.transform.localScale.x - Time.deltaTime * changeSpeed * changeSpeedMultiplier;
                go.transform.localScale = new Vector3(differenceTemp, scaleTemp.y, scaleTemp.z);
                go.transform.position = new Vector3(go.transform.position.x + Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.y, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (go.transform.localScale.x < scaleTemp.x)
            {
                differenceTemp = go.transform.localScale.x + Time.deltaTime * changeSpeed * changeSpeedMultiplier;
                go.transform.localScale = new Vector3(differenceTemp, scaleTemp.y, scaleTemp.z);
                go.transform.position = new Vector3(go.transform.position.x - Time.deltaTime * changeSpeed * changeSpeedMultiplier / 2, positionTemp.y, positionTemp.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        canMove = true;
    }
}
