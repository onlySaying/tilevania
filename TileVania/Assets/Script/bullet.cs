using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    Rigidbody2D rb2d;
    [SerializeField] float buleltSpeed = 10f;
    PlayerMove player;
    float xSpeed;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMove>();
        xSpeed = player.transform.localScale.x * buleltSpeed;
    }

    void Update()
    {
        rb2d .velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
