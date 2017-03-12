using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {

    private static float DEFAULT_WORLD_WIDTH = 9.0f;
    private static float DEFAULT_WORLD_HEIGHT = 16.0f;
    private static float DEFAULT_ASPECT = 9.0f / 16.0f;

    public float s_WorldWidth;
    public float s_WorldHeight;
    public float s_worldBoarderLeft;
    public float s_worldBoarderRight;

    private float m_scaler;
    // Use this for initialization
    void Start () {
        Camera cameraObj = GetComponent<Camera>();
        cameraObj.aspect = DEFAULT_ASPECT;
        cameraObj.ResetAspect();

        // calculate size
        float size = Screen.height / 100.0f / 2.0f;
        cameraObj.orthographicSize = size;

        m_scaler = size / DEFAULT_WORLD_WIDTH;

        s_WorldWidth = DEFAULT_WORLD_WIDTH * m_scaler * 2.0f;
        s_WorldHeight = DEFAULT_WORLD_HEIGHT * m_scaler * 2.0f;

        s_worldBoarderLeft = -s_WorldWidth / 2.0f;
        s_worldBoarderRight = s_WorldWidth / 2.0f;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public Vector3 WorldSpaceToUnitSpace(Vector3 ptWorldSpace)
    {
        return ptWorldSpace * m_scaler;
    }
}
