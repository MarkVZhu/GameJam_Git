using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };//用于控制房间生成方向的枚举类型
    public Direction direction;

    [Header("房间信息")]
    public GameObject roomPrefab;//房间预制体 
    public int roomNumber;//房间数量
    public Color startColor, endColor;//起始房间颜色
    public GameObject endRoom;
    public Text text;
    public int stepToStart;//距离初始房间的"步数"


    [Header("位置控制")]
    public Transform genetorPoint;//生成位置
    public float xOffest;//水平偏移量
    public float yOffest;//竖直偏移量
    public LayerMask roomLayer;

    public List<Room> rooms = new List<Room>();//存储房间的列表

    public WallType wallType;

    private void Start()
    {
        for (int i = 0; i < roomNumber; i++)//摁两下Tab自动生成
        {
            rooms.Add(Instantiate(roomPrefab, genetorPoint.position, Quaternion.identity).GetComponent<Room>());
            //在指定生成位置生成相应预制体的房间(先生成初始房间，之后的房间在生成时位置会根据下方函数变换)，旋转属性一致不变

            rooms[i].UpdataRoom();

            //改变Point的位置
            ChangePointPosition();
        }
        //改变起始和终点房间的颜色
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        //rooms[roomNumber - 1].GetComponent<SpriteRenderer>().color = endColor;
        //终点房间应当距离起始房间较远，如上方式确定的最后生成的房间不一定具有合适的距离

        endRoom = rooms[0].gameObject;

        foreach (var room in rooms)
        {
            //Debug.Log(room.stepToStart + " > " + endRoom.GetComponent<Room>().stepToStart);
            if (room.stepToStart > endRoom.GetComponent<Room>().stepToStart)
            {
                endRoom = room.gameObject;
            }
            //sqrMagnitude用于比较两个Vector3的大小(相比magnitude精度有损失)
            //通过比较房间中心点的向量大小选择最远房间(? 似乎存在问题)

            SetupRoom(room, room.transform.position);
        }

        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    public void ChangePointPosition()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);//随机赋予变换方向变量direction上下左右中的某个数值
                                                      //Random.Range包含左边界值，不包含右边界值
                                                      //int不能隐式转换为enum

            switch (direction)//通过switch完成房间生成点的位置变换
            {
                case Direction.up:
                    genetorPoint.position += new Vector3(0, yOffest, 0);
                    break;
                case Direction.down:
                    genetorPoint.position += new Vector3(0, -yOffest, 0);
                    break;
                case Direction.left:
                    genetorPoint.position += new Vector3(-xOffest, 0, 0);
                    break;
                case Direction.right:
                    genetorPoint.position += new Vector3(xOffest, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(genetorPoint.position, 0.2f, roomLayer));
        //为房间的预制体添加碰撞体和刚体(静态，以免受重力掉落)，并添加一个Layer"Room"，
        //在生成房间时检测生成点的位置是否已经存在其他房间。如果存在，则继续随机，直到找到适宜位置
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        //某一方向的门是否有效取决于OverLapCircle函数所检测的该方向是否存在房间
        //其中，newRoom为当前房间本身，roomPosition为当前房间的位置
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffest, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffest, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffest, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffest, 0, 0), 0.2f, roomLayer);

        //获取对应的墙壁
        switch (newRoom.GetDoornumber())
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.Wall_U, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.Wall_D, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.Wall_L, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.Wall_R, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.Wall_UD, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.Wall_UL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.Wall_UR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.Wall_DL, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.Wall_DR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.Wall_LR, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.Wall_UDL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.Wall_UDR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.Wall_ULR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.Wall_DLR, roomPosition, Quaternion.identity);
                break;
            case 4:
                Instantiate(wallType.Wall_All, roomPosition, Quaternion.identity);
                break;
        }
    }
 }

    [System.Serializable]
    public class WallType
    {
        public GameObject Wall_L, Wall_R, Wall_U, Wall_D,
                          Wall_UL, Wall_UR, Wall_UD, Wall_DL, Wall_DR, Wall_LR,
                          Wall_ULR, Wall_DLR, Wall_UDL, Wall_UDR,
                          Wall_All;
    }


