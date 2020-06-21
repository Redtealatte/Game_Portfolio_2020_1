using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCreditController : MonoBehaviour {

    GameManager manager;

    GameObject endStory;
    GameObject totalCoin;
    GameObject madeBy;
    GameObject theEnd;    

    RectTransform endStoryPos;
    RectTransform totalCoinPos;
    RectTransform madeByPos;
    RectTransform theEndPos;

    Vector3 creditCenter;
    Vector3 creditEnd;

    int creditSpeed;

    int creditSequence;    

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        endStory = GameObject.Find("Canvas").transform.Find("End").transform.Find("EndStory").gameObject;
        totalCoin = GameObject.Find("Canvas").transform.Find("End").transform.Find("TotalCoin").gameObject;
        madeBy = GameObject.Find("Canvas").transform.Find("End").transform.Find("MadeBy").gameObject;
        theEnd = GameObject.Find("Canvas").transform.Find("End").transform.Find("TheEnd").gameObject;

        endStoryPos = endStory.transform as RectTransform;
        totalCoinPos = totalCoin.transform as RectTransform;
        madeByPos = madeBy.transform as RectTransform;
        theEndPos = theEnd.transform as RectTransform;

        creditCenter = new Vector3(0.0f, 0.0f, 0.0f);
        creditEnd = new Vector3(0.0f, 400.0f, 0.0f);

        creditSequence = 0;
        creditSpeed = 200;
    }
	
	// Update is called once per frame
	void Update () {
        EndingCredit();
    }

    public void Sequence()
    {
        if (!endStory.activeSelf && !totalCoin.activeSelf && !madeBy.activeSelf && !theEnd.activeSelf)
        {
            endStory.SetActive(true);
            creditSequence = 1;
        }
        else if (endStory.activeSelf && !totalCoin.activeSelf && !madeBy.activeSelf && !theEnd.activeSelf)
        {
            totalCoin.SetActive(true);
            creditSequence = 2;
        }
        else if (!endStory.activeSelf && totalCoin.activeSelf && !madeBy.activeSelf && !theEnd.activeSelf)
        {
            madeBy.SetActive(true);
            creditSequence = 3;
        }
        else if (!endStory.activeSelf && !totalCoin.activeSelf && madeBy.activeSelf && !theEnd.activeSelf)
        {
            theEnd.SetActive(true);
            creditSequence = 4;
        }
        //게임 끝. 타이틀화면 출력.
        else if (!endStory.activeSelf && !totalCoin.activeSelf && !madeBy.activeSelf && theEnd.activeSelf)
            manager.LoadStartScene();

        //현재 순서에서 이전 크레딧이 다올라가지 않은 상태로 클릭된다면 이전 크레딧의 위치를 종료점으로 바꾸고 스킵.
        if (creditSequence == 2 && endStoryPos.localPosition != creditCenter && endStoryPos.localPosition != creditEnd)
        {
            endStoryPos.localPosition = creditEnd;
            endStory.SetActive(false);
        }

        if (creditSequence == 3 && totalCoinPos.localPosition != creditCenter && totalCoinPos.localPosition != creditEnd)
        {
            totalCoinPos.localPosition = creditEnd;
            totalCoin.SetActive(false);
        }

        if (creditSequence == 4 && madeByPos.localPosition != creditCenter && madeByPos.localPosition != creditEnd)
        {
            madeByPos.localPosition = creditEnd;
            madeBy.SetActive(false);
        }
    }

    void EndingCredit()
    {
        //천천히 엔딩크레딧이 올라가고 중간중간 멈춰서 확인.
        if(creditSequence == 1)//스토리 확인.
        {            
            endStoryPos.localPosition = Vector3.MoveTowards(endStoryPos.localPosition, creditCenter, Time.deltaTime * creditSpeed);
        }
        else if (creditSequence == 2)//스토리는 사라지고 획득한 동전 개수 확인.
        {
            endStoryPos.localPosition = Vector3.MoveTowards(endStoryPos.localPosition, creditEnd, Time.deltaTime * creditSpeed);
            totalCoinPos.localPosition = Vector3.MoveTowards(totalCoinPos.localPosition, creditCenter, Time.deltaTime * creditSpeed);
            if (endStoryPos.localPosition == creditEnd && endStory.activeSelf)
                endStory.SetActive(false);
        }
        else if(creditSequence == 3)//획득 동전이 사라지고 만든이 확인.
        {
            totalCoinPos.localPosition = Vector3.MoveTowards(totalCoinPos.localPosition, creditEnd, Time.deltaTime * creditSpeed);
            madeByPos.localPosition = Vector3.MoveTowards(madeByPos.localPosition, creditCenter, Time.deltaTime * creditSpeed);
            if (totalCoinPos.localPosition == creditEnd && totalCoin.activeSelf)
                totalCoin.SetActive(false);
        }
        else if (creditSequence == 4)//만든이 확인하고 The End
        {
            madeByPos.localPosition = Vector3.MoveTowards(madeByPos.localPosition, creditEnd, Time.deltaTime * creditSpeed);
            theEndPos.localPosition = Vector3.MoveTowards(theEndPos.localPosition, creditCenter, Time.deltaTime * creditSpeed);
            if (madeByPos.localPosition == creditEnd && madeBy.activeSelf)
                madeBy.SetActive(false);
        }
    }
}
