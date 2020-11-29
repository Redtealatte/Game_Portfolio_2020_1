using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    SceneController sceneManager;
    GameManager manager;
    SoundManager soundManager;

    QuestManager questManager;

    [SerializeField]
    private int nextQuestId;

	// Use this for initialization
	void Start () {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneController>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
	
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player" && nextQuestId == questManager.questId)
        {
            sceneManager.ReadyFilledScene(1);
            soundManager.soundChange = true;
            Invoke("DelayAction", 3.0f);
        }
    }

    void DelayAction()
    {
        manager.ChangeNextStage();
    }
}
