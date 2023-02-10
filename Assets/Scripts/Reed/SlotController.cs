using Fexla;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{

    public Transform player;
    public Transform currentCard;
    public Transform clickCard;
    private Vector3 slotPosition;
    public List<Transform> cards;
    [SerializeField]private bool inserting;
    [SerializeField] private Vector3 cardsPileStartPosition;
    [SerializeField]private List<Vector3> cardsPilePosition;
    [SerializeField]private float offsetX;
    public bool isMoving;
    public float initPower = 2;
    [SerializeField] private float cardMoveSpeed;
    private int clickCardIndex;

    public Transform cardPerfeb;
    void Start()
    {
        slotPosition = transform.position;
        Transform newCard = GameObject.Instantiate(cardPerfeb);
        newCard.transform.GetComponent<CardDrag>().power = initPower;
        newCard.transform.SetParent(transform.parent);
        currentCard = newCard; //Fix
        currentCard.position = slotPosition;
        cardsPilePosition.Add(cardsPileStartPosition);
    }

    void Update()
    {
        if (isMoving)
        {
            clickCard.transform.position = Vector3.MoveTowards(clickCard.transform.position, 
                slotPosition, 
                cardMoveSpeed * Time.deltaTime);

            if (cardsPilePosition.Count - 2 >= 0)
            {
                currentCard.transform.position = Vector3.MoveTowards(currentCard.transform.position,
                    transform.position + cardsPilePosition[cardsPilePosition.Count - 2],
                    cardMoveSpeed * Time.deltaTime);
            }

            for (int i = clickCardIndex; i < cards.Count-1; i++)
            {
                cards[i].transform.position = Vector3.MoveTowards(
                    cards[i].transform.position,
                    transform.position + cardsPilePosition[i],
                    cardMoveSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(currentCard.transform.position, transform.position + cardsPilePosition[cardsPilePosition.Count - 2]) <= 0.01f)
            {
                isMoving = false;
                currentCard = clickCard;
                player.GetComponent<PlayerStateControl>().power = 1/currentCard.GetComponent<CardDrag>().power;
                clickCard = null;
            }
        }
    }

    public void StopDraggingCard()
    {
        if (!inserting)
        {
            clickCard.transform.position = new Vector3(0, 0, 0);
        }
        else 
        {
            clickCard.transform.position = slotPosition;
            currentCard = clickCard;
        }
    }

/*    public void TestClick()
    {
        Debug.Log("succeed");
        Transform testNewCard = GameObject.Instantiate(testCard);
        AddCard(testNewCard);
    }*/

    public void AddCard(Transform card)
    {
        cards.Add(card);
        card.transform.position = transform.position + cardsPilePosition[cardsPilePosition.Count - 1]; //Fix
        card.transform.SetParent(transform.parent);
        //�ڿ�Ƭλ�������м������һ�ſ���λ������
        cardsPilePosition.Add(new Vector3(cardsPilePosition[cardsPilePosition.Count - 1].x + offsetX, cardsPilePosition[cardsPilePosition.Count - 1].y));
    }

    public Transform createCard(float power)
    {
        Transform newCard = GameObject.Instantiate(cardPerfeb);
        newCard.transform.GetComponent<CardDrag>().power = power;
        AddCard(newCard);//Fix
        return newCard;
    }

    public void ClickCard() 
    {
        if (!isMoving && !clickCard.Equals(currentCard))
        {
            isMoving = true;
            clickCardIndex = cards.IndexOf(clickCard);
            cards.RemoveAt(clickCardIndex);
            cards.Add(currentCard);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(cardsPileStartPosition.x, cardsPileStartPosition.y, 0), cardsPileStartPosition.z);
    }
}

