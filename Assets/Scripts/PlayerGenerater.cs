using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerater : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject currentPlayer;
    [SerializeField] private Vector3 playerPosition;
    public float lastFrameDirection;
    public float lastPowerNumber;
    public float lastChangeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lastFrameDirection);
        if (currentPlayer)
        {
            playerPosition = currentPlayer.transform.position;
        }
    }

    public void generate()
    {
        currentPlayer = Instantiate(playerPrefab, playerPosition - new Vector3(0, 1.45f, 0), Quaternion.identity);
        if(lastFrameDirection < 0)
        {
            currentPlayer.transform.localScale = new Vector3(-currentPlayer.transform.localScale.x, currentPlayer.transform.localScale.y, currentPlayer.transform.localScale.z);
        }
        currentPlayer.GetComponentInChildren<RootExtract>().powNumber = lastPowerNumber;
        currentPlayer.GetComponentInChildren<RootExtract>().changeSpeed = lastChangeSpeed;
        currentPlayer.GetComponent<Animator>().SetTrigger("RotateR");
    }
}
