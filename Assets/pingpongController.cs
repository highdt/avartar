using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pingpongController : MonoBehaviour {

    public float m_speed;             //Floating point variable to store the player's movement speed.

    // shooting control
    private bool m_isShooting;
    private Vector3 m_shootStartPos;
    private Vector3 m_shootEndPos;

    // shape control
    private float m_scaleSpeed;
    private float m_maxScaling;
    private float m_minScaling;
    private bool m_isScaling;

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        m_speed = 100.0f;
        m_isShooting = false;

        m_scaleSpeed = 0.001f;
        m_maxScaling = 0.7f;
        m_minScaling = 0.35f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_isShooting)
            {
                m_shootStartPos = transform.position;
                m_isShooting = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (m_isShooting)
            {
                m_shootEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Shoot();
            }
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Debug.Log(movement);
        
        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        //rb2d.AddForce(movement * speed);
    }

    void OnMouseDrag()
    {
        Vector3 scale = transform.localScale;
        Vector3 magnitude = new Vector3(m_scaleSpeed, m_scaleSpeed, 0);
        scale += magnitude;
        transform.localScale = scale;
    }

    void Shoot()
    {
        Vector3 shootDir = m_shootEndPos - m_shootStartPos;
        shootDir.Normalize();
        shootDir = shootDir * m_speed;

        Debug.Log("shoot dir:" + shootDir);

        rb2d.AddForce(shootDir);

        m_isShooting = false;
    }
}