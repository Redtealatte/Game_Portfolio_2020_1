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

    //로딩 화면에서 게임 화면으로 변경.
    //UI의 standByImage의 fillAmount가 0.001f보다 작다면 더이상 반복할 수 없게 standByState를 0으로 변경하고 로딩화면을 숨김.
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

    //게임 화면에서 로딩 화면으로 변경.
    //UI의 standByImage의 fillAmount가 0.999f보다 크다면 더이상 반복할 수 없게 standByState를 0으로 변경.
    void FilledScene()
    {
        standByImage.fillAmount += Time.deltaTime * fillSpeed;
        if (standByImage.fillAmount > 0.999f)
        {
            standByImage.fillAmount = 1.0f;
            standByState = 0;
        }
    }

    //게임 - 로딩 전환시에 시계방향으로 채워지고 지워지도록 보이게 설정.
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

    //현재 스테이지의 데이터가 바뀌면 이전 스테이지의 타일맵을 숨기고 현재 스테이지의 타일맵을 활성화.
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
