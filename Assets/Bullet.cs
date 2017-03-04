using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 m_velocity;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        transform.Translate(m_velocity);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger hit " + collision.gameObject.name);

        GameObject gameObj = GameObject.Find("MainGame");
        MainGame mainGame = gameObj.GetComponent<MainGame>();
        mainGame.OnBulletHit(this.gameObject, collision.gameObject);

        Destroy(this.gameObject);
    }
}