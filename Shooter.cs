using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    // gameplay
    public MainGame.Army m_myArmy = MainGame.Army.Army_None;

    // shooting control
    private bool m_isShooting;
    private float m_shootingStrength;
    private Vector3 m_shootStartPos;
    private Vector3 m_shootEndPos;
    private float m_shootingCooldown;
    private float m_lastShoointgTime;

    // shape control
    private float m_scaleSpeed;
    private float m_maxScaling;
    private float m_minScaling;

    // body control
    private Vector2 m_currentForce;

    private Rigidbody2D m_body;
    private SpriteRenderer m_render;
    private float m_speed;

    private Transform m_myTransform;

    // Use this for initialization
    void Start () {
        m_body = GetComponent<Rigidbody2D>();
        m_render = GetComponent<SpriteRenderer>();
        m_myTransform = m_body.transform;

        // shooting control
        m_shootingStrength = 1.0f;
        m_speed = 1.0f;
        m_isShooting = false;
        m_shootingCooldown = 3.0f;
        m_lastShoointgTime = -m_shootingCooldown;

        m_scaleSpeed = 0.01f;
        m_maxScaling = 0.7f;
        m_minScaling = 0.35f;
    }

    public void PostStart()
    {

    }
	
	// Update is called once per frame
	void Update () {

        if (m_myArmy == MainGame.Army.Army_None)
        {
            return;
        }

        UpdateUI();

        if (Input.GetMouseButtonDown(0))
        {
            if (!m_isShooting)
            {
                PrepareShoot();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (m_isShooting)
            {
                DoShoot();
            }
        }

    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (m_myArmy == MainGame.Army.Army_None)
        {
            return;
        }

        // handle scaling
        if (m_isShooting && m_myTransform.localScale.x < m_maxScaling)
        {
            ScaleTo(m_maxScaling);
        }
        else if (!m_isShooting && m_myTransform.localScale.x > m_minScaling)
        {
            ScaleTo(m_minScaling);
        }


        float moveVertical = Input.GetAxis("Vertical");
        if (!moveVertical.Equals(0))
        {   
            if (m_currentForce.y > 1.5f && moveVertical > 0)
            {
                moveVertical = 0.0f;
            }

            if (m_currentForce.y < -1.5f && moveVertical < 0)
            {
                moveVertical = 0.0f;
            }

            m_currentForce += (new Vector2(0, moveVertical)) * m_speed;
            m_body.AddForce(m_currentForce);
        }

        // reistrict obj inside border
        if (m_myTransform.position.y < -4.8f)
        {
            m_myTransform.position = new Vector3(m_myTransform.position.x, -4.8f, m_myTransform.position.z);
        }
        else if (m_myTransform.position.y > 5.0f)
        {
            m_myTransform.position = new Vector3(m_myTransform.position.x, 5.0f, m_myTransform.position.z);
        }
    }

    void UpdateUI()
    {
        if (IsInShootCooldown())
        {
            m_render.color = Color.red;
        }
        else
        {
            m_render.color = Color.green;
        }   
    }

    void PrepareShoot()
    {
        // check cooldown
        if (Time.time < (m_lastShoointgTime + m_shootingCooldown))
        {
            return;
        }

        m_shootStartPos = m_myTransform.position;
        m_isShooting = true;
    }

    bool IsInShootCooldown()
    {
        return (Time.time < (m_lastShoointgTime + m_shootingCooldown));
    }

    void DoShoot()
    {
        m_shootEndPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

        Vector3 shootingDir = m_shootEndPos - m_shootStartPos;
        shootingDir.Normalize();
        shootingDir = shootingDir * m_shootingStrength * Time.maximumDeltaTime;

        //Debug.Log("shoot dir:" + shootingDir);

        // create bullet
        GameObject bulletObj = GameObject.Find("bullet");
        GameObject b1 = Instantiate(bulletObj, m_myTransform.position, Quaternion.identity);
        // set position & scale
        b1.transform.position = GetFiringPosition(b1.transform.position, m_myTransform.localScale.x);
        b1.transform.localScale = new Vector3(m_myTransform.localScale.x, m_myTransform.localScale.y, m_myTransform.localScale.z);
        // set script
        Bullet b1cs = b1.GetComponent<Bullet>();
        b1cs.m_velocity = shootingDir;

        m_isShooting = false;

        // entering shooting cooldown
        m_lastShoointgTime = Time.time;
    }

    private Vector3 GetFiringPosition(Vector3 parentPos, float parentScale)
    {
        Vector3 firingPointOffset = new Vector3(parentScale * 0.5f / 0.35f, 0.0f, 0.0f);

        if (m_myArmy == MainGame.Army.Army_Red)
        {
            return parentPos + firingPointOffset;
        }
        else if (m_myArmy == MainGame.Army.Army_Blue)
        {
            return parentPos - firingPointOffset;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void ScaleTo(float scaleTarget)
    {
        Vector3 currScale = m_myTransform.localScale;
        Vector3 magnitude = new Vector3(m_scaleSpeed, m_scaleSpeed, 0);

        if (currScale.x > scaleTarget)
        {
            currScale -= magnitude;
        }
        else if (currScale.x < scaleTarget)
        {
            currScale += magnitude;
        }

        m_myTransform.localScale = currScale;
    }
}
