using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorRight, doorLeft, doorUp, doorDown;

    public bool roomRight, roomLeft, roomUp, roomDown;//决定门是否可用; 这组布尔变量的判断在RoomGenerator中进行

    public int stepToStart;
    public Text text;

    private int doorNumber;

    private void Start()
    {
        doorRight.SetActive(roomRight);
        doorLeft.SetActive(roomLeft);
        doorUp.SetActive(roomUp);
        doorDown.SetActive(roomDown);
    }

    public void UpdataRoom()//在Generator的SetupRoom中调用
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / 18) + Mathf.Abs(transform.position.y / 10));
        //通过相应坐标除以偏移量来获得"步数"

        text.text = stepToStart.ToString();
    }

    public int GetDoornumber()
    {
        if (roomRight) doorNumber++;
        if (roomLeft) doorNumber++;
        if (roomUp) doorNumber++;
        if (roomDown) doorNumber++;
        return doorNumber;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Change Target");
            CameraControl.instance.ChangeTarget(transform);
        }
    }

}
