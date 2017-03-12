using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {

    public enum Army
    {
        Army_None = 0,
        Army_Red,
        Army_Blue,
    }

    // score board
    private int m_currRedScore;
    private int m_currBlueScore;
    private Text m_scoreUi;

    // Use this for initialization
    void Start () {
        // init shooters
        CreateShooter(Army.Army_Red);
        CreateShooter(Army.Army_Blue);

        m_scoreUi = GameObject.Find("Canvas/Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnBulletHit(GameObject bullet, GameObject target)
    {
        // update score
        Shooter shooter = target.GetComponent<Shooter>();
        if (shooter != null)
        {
            if (shooter.m_myArmy == Army.Army_Red)
            {
                m_currRedScore++;
            }
            else if (shooter.m_myArmy == Army.Army_Blue)
            {
                m_currBlueScore++;
            }

            m_scoreUi.text = (m_currRedScore.ToString() + "   -   " + m_currBlueScore.ToString());
        }
    }

    private void CreateShooter(Army army)
    {
        GameObject cameraObj = GameObject.Find("MainCamera");
        MainCameraController controller = cameraObj.GetComponent<MainCameraController>();

        GameObject shooter = GameObject.Find("Shooter");

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        string name = "";
        if (army == Army.Army_Red)
        {
            position = controller.WorldSpaceToUnitSpace(new Vector3(0.0f, 7.0f, 0.0f));
            rotation = new Quaternion(0, 0, 0, 35);

            name = "red";
        }
        else if (army == Army.Army_Blue)
        {
            position = controller.WorldSpaceToUnitSpace(new Vector3(0.0f, -7.0f, 0.0f));
            rotation = new Quaternion(0, 0, 0, 155);

            name = "blue";
        }

        GameObject shooterObj = Instantiate(shooter, position, rotation);

        shooterObj.name = name;
        Shooter shooterScript = shooterObj.GetComponent<Shooter>();
        shooterScript.m_myArmy = army;
        shooterScript.PostStart();
    }
}
