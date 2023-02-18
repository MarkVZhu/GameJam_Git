using Part;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private List<Transform>  negativeOnes;
    [SerializeField] private Sprite switchImage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Sprite lastImage = transform.GetComponent<SpriteRenderer>().sprite;
            transform.GetComponent<SpriteRenderer>().sprite = switchImage;
            switchImage = lastImage;

            foreach (Transform t in negativeOnes)
            { 
                t.GetComponent<Box>().NegativeFlip();
            }
        }
    }
}
