using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalControl : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprite;
    public float maxSpeed = 1;
    public float minSpeed = 0.5f;
    public float maxAngleSpeed = 32;
    public float minAngleSpeed = 18f;
    public float speed;
    public float angleSpeed;

    void Start() {
        int x = (int) (((long) (Random.value * 998244353l)) % sprite.Length);
        spriteRenderer.sprite = sprite[x];
        speed = minSpeed + (maxSpeed - minSpeed) * Random.value;
        angleSpeed = minAngleSpeed + (maxAngleSpeed - minAngleSpeed) * Random.value;
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y < -15) {
            transform.position += new Vector3(0, 40, 0);
            Start();
        }
        transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;

        Vector3 angle = transform.rotation.eulerAngles;
        angle.z += angleSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(angle);

    }
}