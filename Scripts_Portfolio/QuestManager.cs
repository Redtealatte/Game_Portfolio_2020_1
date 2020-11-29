using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;
    public GameObject[] questObject;

	// Use this for initialization
	void Awake () {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
	}
	
	//퀘스트 데이터 생성.
	void GenerateData () {
        questList.Add(10, new QuestData("엄마에게 가자.", new int[] { 1000 }));
        questList.Add(20, new QuestData("꽃의 행방을 찾자.", new int[] { 3000, 5000 }));
        questList.Add(30, new QuestData("할머니께 꽃을 전달해 드리자.", new int[] { 5000, 2000 }));
        questList.Add(40, new QuestData("Quest All Clear!", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    public string CheckQuest(int id)
    {        
        //퀘스트 대화의 다음 NPC.
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        ControlObject();

        //대화 완료 및 다음 퀘스트.
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        //퀘스트 이름 반환.
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject()
    {
        switch (questId) {
            case 20:
                if (questActionIndex == 1)
                    questObject[0].SetActive(true);
                break;
            case 30:
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }        
    }
}
