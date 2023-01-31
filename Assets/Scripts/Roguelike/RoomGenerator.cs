using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };//���ڿ��Ʒ������ɷ����ö������
    public Direction direction;

    [Header("������Ϣ")]
    public GameObject roomPrefab;//����Ԥ���� 
    public int roomNumber;//��������
    public Color startColor, endColor;//��ʼ������ɫ
    public GameObject endRoom;
    public Text text;
    public int stepToStart;//�����ʼ�����"����"


    [Header("λ�ÿ���")]
    public Transform genetorPoint;//����λ��
    public float xOffest;//ˮƽƫ����
    public float yOffest;//��ֱƫ����
    public LayerMask roomLayer;

    public List<Room> rooms = new List<Room>();//�洢������б�

    public WallType wallType;

    private void Start()
    {
        for (int i = 0; i < roomNumber; i++)//������Tab�Զ�����
        {
            rooms.Add(Instantiate(roomPrefab, genetorPoint.position, Quaternion.identity).GetComponent<Room>());
            //��ָ������λ��������ӦԤ����ķ���(�����ɳ�ʼ���䣬֮��ķ���������ʱλ�û�����·������任)����ת����һ�²���

            rooms[i].UpdataRoom();

            //�ı�Point��λ��
            ChangePointPosition();
        }
        //�ı���ʼ���յ㷿�����ɫ
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        //rooms[roomNumber - 1].GetComponent<SpriteRenderer>().color = endColor;
        //�յ㷿��Ӧ��������ʼ�����Զ�����Ϸ�ʽȷ����������ɵķ��䲻һ�����к��ʵľ���

        endRoom = rooms[0].gameObject;

        foreach (var room in rooms)
        {
            //Debug.Log(room.stepToStart + " > " + endRoom.GetComponent<Room>().stepToStart);
            if (room.stepToStart > endRoom.GetComponent<Room>().stepToStart)
            {
                endRoom = room.gameObject;
            }
            //sqrMagnitude���ڱȽ�����Vector3�Ĵ�С(���magnitude��������ʧ)
            //ͨ���ȽϷ������ĵ��������Сѡ����Զ����(? �ƺ���������)

            SetupRoom(room, room.transform.position);
        }

        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }

    public void ChangePointPosition()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);//�������任�������direction���������е�ĳ����ֵ
                                                      //Random.Range������߽�ֵ���������ұ߽�ֵ
                                                      //int������ʽת��Ϊenum

            switch (direction)//ͨ��switch��ɷ������ɵ��λ�ñ任
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
        //Ϊ�����Ԥ���������ײ��͸���(��̬����������������)�������һ��Layer"Room"��
        //�����ɷ���ʱ������ɵ��λ���Ƿ��Ѿ������������䡣������ڣ�����������ֱ���ҵ�����λ��
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        //ĳһ��������Ƿ���Чȡ����OverLapCircle���������ĸ÷����Ƿ���ڷ���
        //���У�newRoomΪ��ǰ���䱾��roomPositionΪ��ǰ�����λ��
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffest, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffest, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffest, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffest, 0, 0), 0.2f, roomLayer);

        //��ȡ��Ӧ��ǽ��
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


