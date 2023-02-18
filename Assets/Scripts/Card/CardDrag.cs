using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour
{
    public Text powerText;
    public float power;
    public static float currentRate = 2;
    [SerializeField] private Transform slot;

    bool isDrag;//�Ƿ�Ϊ�϶�״̬
    [SerializeField]public bool isBack;//���ƿ�Ƭ�ص�ԭλ��
    [SerializeField] public bool isBackSlot;//���ƿ�Ƭ�ص�����λ��

    // Start is called before the first frame update
    void Start()
    {
        if(power > 1)
            powerText.text = ((int)power).ToString();
        else
            powerText.text = "1/" + ((int)(1 /power)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickCard()
    {
        if (!slot.GetComponent<SlotController>().isMoving)
        {
            isDrag = true;
            slot.GetComponent<SlotController>().clickCard = this.transform;
        }
    }
}
