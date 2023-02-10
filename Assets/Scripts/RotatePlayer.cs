using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    private PlayerControlH ch;
    private PlayerControlV cv;
    [SerializeField] private bool isRoating;
    private float playerSpeed;
    private float playerJumpForce;
    private Animator animHV;
    private bool isClockwise;

    public bool moveAfterReverseRoate;

    public Transform PivoteTransform;
    public float rotateSpeed;

    private GameObject playerGenerater;

    // Start is called before the first frame update
    void Start()
    {
        ch = GetComponent<PlayerControlH>();
        cv = GetComponent<PlayerControlV>();
        animHV = GetComponent<Animator>();
        playerGenerater = GameObject.Find("GeneratePlayer");
        moveAfterReverseRoate = false;
        isRoating = false;
        isClockwise = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRoating)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        if (isClockwise)
        {
            animHV.SetTrigger("Rotate");
            isClockwise = false;
        }
        else
        {
            GameObject gn = GameObject.Find("GeneratePlayer");
            gn.GetComponent<PlayerGenerater>().lastFrameDirection = transform.lossyScale.y;
            gn.GetComponent<PlayerGenerater>().lastPowerNumber = GetComponentInChildren<RootExtract>().powNumber;
            gn.GetComponent<PlayerGenerater>().lastChangeSpeed = GetComponentInChildren<RootExtract>().changeSpeed;
            Debug.Log(gn.GetComponent<PlayerGenerater>().lastFrameDirection);
            gn.GetComponent<PlayerGenerater>().generate();
            Destroy(this.gameObject);
        }
    }

    public void StartRotating()
    {
        isRoating = true;
    }

    public void EndRotate()
    {
        transform.RotateAround(PivoteTransform.position, Vector3.back, 90f);
        if (playerGenerater.GetComponent<PlayerGenerater>().currentPlayer.transform.lossyScale.x < 0)
        {
            moveAfterReverseRoate = true;
            transform.localScale = new Vector3(-transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            transform.position += new Vector3(1.3f, 1.5f, 0);
        }
        alterScript();
        isRoating = false;
    }

    public void JustEndRotate()
    {
        isRoating = false;
    }

    void alterScript()
    {
        if (ch.isActiveAndEnabled && !cv.isActiveAndEnabled)
        {
            ch.enabled = false;
            cv.enabled = true;
        }
        else
        {
            ch.enabled = true;
            cv.enabled = false;
        }
    }

    public bool GetRotating()
    {
        return isRoating;
    }
}