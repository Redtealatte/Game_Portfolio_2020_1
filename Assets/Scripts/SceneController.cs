using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    GameObject standByScene;
    Image standByImage;
    float fillSpeed;
    int standByState;

    GameObject stage1;
    GameObject stage2;
    GameObject stage3;

    // Use this for initialization
    void Start () {
        standByScene = GameObject.Find("Canvas").transform.Find("StandByScene").gameObject;
        standByImage = GameObject.Find("Canvas").transform.Find("StandByScene").GetComponent<Image>();
        stage1 = GameObject.Find("Grid").transform.Find("Stage1").gameObject;
        stage2 = GameObject.Find("Grid").transform.Find("Stage2").gameObject;
        stage3 = GameObject.Find("Grid").transform.Find("Stage3").gameObject;
        fillSpeed = 1.0f;
        standByState = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (standByState == 1)
            FilledScene();
        else if (standByState == -1)
            UnFilledScene();
	}

    void UnFilledScene()
    {
        standByImage.fillAmount -= Time.deltaTime * fillSpeed;
        if (standByImage.fillAmount < 0.001f)
        {
            standByImage.fillAmount = 0.0f;
            standByState = 0;
            standByScene.SetActive(false);
        }
    }

    void FilledScene()
    {
        standByImage.fillAmount += Time.deltaTime * fillSpeed;
        if (standByImage.fillAmount > 0.999f)
        {
            standByImage.fillAmount = 1.0f;
            standByState = 0;
        }
    }

    public void ReadyFilledScene(int state)
    {        
        if(state == 1)
        {
            standByImage.fillAmount = 0.0f;
            standByState = state;
            standByImage.fillClockwise = true;
        }
        else if(state == -1)
        {
            standByImage.fillAmount = 1.0f;
            standByState = state;
            standByImage.fillClockwise = false;
        }
        standByScene.SetActive(true);
    }

    public void ChangeStage(int currentStage)
    {
        if(currentStage == 2)
        {
            stage1.SetActive(false);
            stage2.SetActive(true);            
        }
        else if(currentStage == 3)
        {
            stage2.SetActive(false);
            stage3.SetActive(true);
        }
        ReadyFilledScene(-1);
    }

    public bool GetActiveStandByScene()
    {
        return standByScene.activeSelf;
    }
    
}
