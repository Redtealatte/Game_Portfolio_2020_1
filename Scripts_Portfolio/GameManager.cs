using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //스테이지 변환
    SceneController sceneController;
    public int stageNum;
    //사운드
    SoundManager soundManager;
    //퀘스트
    public QuestManager questManager;
    //다이얼로그
    public TalkManager talkManager;
    public Animator talkPanel;
    public TypingEffect talk;
    public GameObject scanObject;
    public bool isAction;
    int talkIndex = 1;
    //메뉴바
    public GameObject menuSet;
    public Text questText;
    //도움말.
    public GameObject helpPanel;
    //상단바
    public Image[] hpInner;
    int filledHp;
    //hp관련 변수들.
    public int hp;
    public int hpState;
    bool hpChanging;
    [SerializeField]
    private float lerpSpeed;
    //세이브 위치.
    public Vector2 savePos;
    //플레이어
    public GameObject player;
    public PlayerMove playerMove;

    public Text coinText;
    int totalMoney;

    GameObject page1;
    GameObject page2;

    Vector3[] stageStartPos;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        helpPanel = GameObject.Find("Canvas").transform.Find("Help").gameObject;
        sceneController = GameObject.Find("SceneManager").GetComponent<SceneController>();
        page1 = GameObject.Find("Canvas").transform.Find("Help").transform.Find("BackPanel").transform.Find("Page1").gameObject;
        page2 = GameObject.Find("Canvas").transform.Find("Help").transform.Find("BackPanel").transform.Find("Page2").gameObject;

        stageStartPos = new Vector3[3];
        stageStartPos[0] = new Vector3(-42.0f, -0.25f, 0);
        stageStartPos[1] = new Vector3(-48.0f, -0.25f, 0);
        stageStartPos[2] = new Vector3(-44.0f, -0.25f, 0);
        savePos = Vector2.zero;

        if (SceneManager.GetActiveScene().name == "GameScene" && !PlayerPrefs.HasKey("isReset"))
        {
            GameLoad();
            Debug.Log("Test");
        }
            
        if (PlayerPrefs.HasKey("isReset"))
        {
            GameResetLoad();
        }
            
        questText.text = questManager.CheckQuest();
        Debug.Log(questManager.CheckQuest());
    }

    public void Action(GameObject scanObj)
    {
        if (scanObj != null)
            scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);
        talkPanel.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";
        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 1;
            questText.text = questManager.CheckQuest(id);
            return;
        }

        if (isNpc)
            talk.SetMsg(talkData);
        else
            talk.SetMsg(talkData);

        isAction = true;
        talkIndex++;
    }

    public void MakeMoney(int moneyType)
    {
        if (moneyType == 0)
            totalMoney += Random.Range(1, 6);
        else if (moneyType == 1)
            totalMoney += Random.Range(6, 11);
        else if (moneyType == 2)
            totalMoney += Random.Range(11, 16);


        coinText.text = ": " + totalMoney;
    }

    void Update()
    {
        if (filledHp != hp)
        {
            if (filledHp > hp && filledHp > 0)
                HPDown();
            else if (filledHp < hp && filledHp <= 3)
            {
                HPUp();
            }

        }
        //메뉴바. 
        //타이틀화면에서는 esc를 눌러도 메뉴바가 사라지지 않음. 게임스테이지(스테이지 1번)부터는 esc를 누르면 메뉴바가 사라짐.
        if (Input.GetButtonDown("Cancel") && (!menuSet.activeSelf && stageNum == 0 || stageNum >= 1))
        {
            if (helpPanel.activeSelf)
            {
                GameHelpClose();
            }
            else
            {
                PlayClickSound();
                if (menuSet.activeSelf)
                    menuSet.SetActive(false);
                else
                    menuSet.SetActive(true);
            }
        }

        //R키를 눌러서 재시작 했을때 처리.
        if (Input.GetKeyDown(KeyCode.R) && !menuSet.activeSelf && !helpPanel.activeSelf 
            && stageNum >= 1 && stageNum <= 3 && !sceneController.GetActiveStandByScene() && hp > 0)
        {
            soundManager.soundChange = true;
            sceneController.ReadyFilledScene(1);
            GameResetSave();
            Invoke("LoadGameScene", 3.0f);
        }
    }

    //체력감소.
    void HPDown()
    {
        hpInner[filledHp - 1].fillAmount = Mathf.Lerp(hpInner[filledHp - 1].fillAmount, 0.0f, Time.deltaTime * lerpSpeed);
        if (hpInner[filledHp - 1].fillAmount < 0.1f)
        {
            hpInner[filledHp - 1].fillAmount = 0.0f;
            filledHp--;
            hpChanging = false;//체력변경이 종료되었으므로 false로 바꿔서 다시 체력감소(증가)가 가능한 상태로 만들어줌.
        }
    }

    //체력증가.
    void HPUp()
    {
        hpInner[filledHp].fillAmount = Mathf.Lerp(hpInner[filledHp].fillAmount, 1.0f, Time.deltaTime * lerpSpeed);
        if (hpInner[filledHp].fillAmount > 0.9f)
        {
            hpInner[filledHp].fillAmount = 1.0f;
            filledHp++;
            hpChanging = false;
        }
    }

    //체력의 변경이 있을때 호출하는 함수.
    public void HPChange(bool hpDown)
    {
        if (hpDown && !hpChanging)//hpDown이 true일때 체력감소를 진행.
        {
            hp--;
            hpChanging = true;//동시에 두명의 적에게 공격 당해서 피가 2 이상 빠지는걸 막기위해 hp가 변화 중 이라는걸 인지하기 위해 true로 변경.
        }
        if (!hpDown && !hpChanging)//hpDown이 false일때 체력증가를 진행.
        {
            hp++;
            if (hp > 3)
            {
                hp = 3;
                totalMoney += 30;
                coinText.text = ": " + totalMoney;
                return;
            }
            hpChanging = true;
        }
        if (hp == 0)//플레이어 사망.
            playerMove.OnDie();
    }

    //UI의 체력바 모션.
    public void HPInitialize(int hp)
    {
        this.hp = filledHp = hp;
        for (int i = 0; i < this.hp; i++)
            hpInner[i].fillAmount = 1.0f;
    }

    public void SetSavePoint(Vector2 pos)
    {
        savePos = pos;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (hp > 1)
            {
                soundManager.SoundEffectPlay("Die");
                coll.attachedRigidbody.velocity = Vector2.zero;
                coll.transform.position = savePos;
            }
            HPChange(true);
        }
    }

    //계속하기.
    public void GameContinue()
    {
        menuSet.SetActive(false);
        soundManager.SoundEffectPlay("Click");
    }

    //저장하기
    public void GameSave()
    {
        if(savePos == Vector2.zero)
            savePos = stageStartPos[stageNum - 1];

        PlayerPrefs.SetFloat("PlayerX", savePos.x);
        PlayerPrefs.SetFloat("PlayerY", savePos.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.SetInt("Hp", hp);
        PlayerPrefs.SetInt("StageNumber", stageNum);
        PlayerPrefs.Save();        

        PlayClickSound();
        menuSet.SetActive(false);
    }

    //불러오기
    public void GameLoad()
    {
        sceneController.ReadyFilledScene(-1);
        //저장된게 없으면 종료.
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            player.transform.position = stageStartPos[0];//1스테이지 시작점.
            questManager.questId = 10;
            questManager.questActionIndex = 0;
            stageNum = 1;
            soundManager.SetStageNumber(stageNum);
            return;
        }

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");
        stageNum = PlayerPrefs.GetInt("StageNumber");
        HPInitialize(PlayerPrefs.GetInt("Hp"));
        if (x == 0 && y == 0)
            player.transform.position = stageStartPos[stageNum - 1];
        else
            player.transform.position = new Vector3(x, y, 0);
        sceneController.ChangeStage(stageNum);
        
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
        soundManager.SetStageNumber(stageNum);

    }

    void PlayClickSound()
    {
        soundManager.VolumeControl(0.3f);
        soundManager.SoundEffectPlay("Click");
    }

    //도움말 열기.
    public void GameHelpOpen()
    {
        if (stageNum == 0)
            menuSet.SetActive(false);
        helpPanel.SetActive(true);
        page1.SetActive(true);
        page2.SetActive(false);
        PlayClickSound();
    }

    //도움말 닫기.
    public void GameHelpClose()
    {
        PlayClickSound();
        if (page1.activeSelf)
        {
            page1.SetActive(false);
            page2.SetActive(true);
            return;
        }
        menuSet.SetActive(true);
        helpPanel.SetActive(false);        
    }
    
    //새 게임. 기존의 저장된 게임을 삭제.
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        menuSet.SetActive(false);
        soundManager.soundChange = true;
        sceneController.ReadyFilledScene(1);
        Invoke("LoadGameScene", 3.0f);
    }

    //불러오기. 저장된 게임을 불러오기.
    public void GameContinuing()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            menuSet.SetActive(false);
            soundManager.soundChange = true;
            sceneController.ReadyFilledScene(1);
            Invoke("LoadGameScene", 3.0f);
        }
    }

    //종료하기
    public void GameExit()
    {
        PlayClickSound();
        //에디터에서는 실행되지 않음.
        Application.Quit();
    }

    //각 스테이지에 따라 캐릭터의 위치를 변경.
    public void ChangeNextStage()
    {
        stageNum++;
        if (stageNum == 2)//2스테이지 진입.
        {
            GameObject enemy1 = GameObject.Find("Grid").transform.Find("Stage1").transform.Find("Enemy").gameObject;
            //player.transform.position = new Vector3(-39.5f, 25.5f, 0);
            player.transform.position = stageStartPos[1];
            Destroy(enemy1);
            sceneController.ChangeStage(stageNum);
            savePos = Vector3.zero;
        }
        else if (stageNum == 3)//3스테이지 진입
        {
            GameObject enemy2 = GameObject.Find("Grid").transform.Find("Stage2").transform.Find("Enemy").gameObject;
            //player.transform.position = new Vector3(11.0f, 8.0f, 0);
            player.transform.position = stageStartPos[2];
            Destroy(enemy2);
            sceneController.ChangeStage(stageNum);
            savePos = Vector3.zero;
        }
        else//게임클리어시
        {
            GameObject enemy3 = GameObject.Find("Grid").transform.Find("Stage3").transform.Find("Enemy").gameObject;
            Destroy(enemy3);

            sceneController.ChangeStage(stageNum);
            GameObject endingCredit = GameObject.Find("Canvas").transform.Find("End").gameObject;

            endingCredit.SetActive(true);
            Text totalCoin = GameObject.Find("Canvas").transform.Find("End").transform.Find("TotalCoin").GetComponent<Text>();
            totalCoin.text = "획득 동전 : " + totalMoney;
        }
        soundManager.SetStageNumber(stageNum);
    }

    //플레이어 사망 시 화면 출력 및 데이터 삭제.
    public void GameFailed()
    {
        sceneController.ReadyFilledScene(1);
        PlayerPrefs.DeleteAll();
        Invoke("GameFailedText", 3.0f);
    }

    //플레이어 사망 시 게임 실패 화면 출력.
    void GameFailedText()
    {
        GameObject failedtext = GameObject.Find("Canvas").transform.Find("StandByScene").transform.Find("Failed").gameObject;
        failedtext.SetActive(true);
        Invoke("LoadStartScene", 3.0f);
    }

    //Invoke로 시간차를 이용해서 실행해야 되는 함수.
    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    void GameResetSave()
    {
        PlayerPrefs.SetInt("isReset", 1);

        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.SetInt("Hp", hp);
        PlayerPrefs.SetInt("StageNumber", stageNum);
        PlayerPrefs.Save();
    }

    void GameResetLoad()
    {
        sceneController.ReadyFilledScene(-1);

        PlayerPrefs.DeleteKey("isReset");

        stageNum = PlayerPrefs.GetInt("StageNumber");
        HPInitialize(PlayerPrefs.GetInt("Hp"));
        if(stageNum >= 2)
        {
            GameObject enemy1 = GameObject.Find("Grid").transform.Find("Stage1").transform.Find("Enemy").gameObject;
            Destroy(enemy1);
            if(stageNum == 3)
            {
                GameObject enemy2 = GameObject.Find("Grid").transform.Find("Stage2").transform.Find("Enemy").gameObject;
                Destroy(enemy2);
            }
        }

        player.transform.position = stageStartPos[stageNum - 1];
        sceneController.ChangeStage(stageNum);
        questManager.questId = PlayerPrefs.GetInt("QuestId");
        questManager.questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");
        questManager.ControlObject();
        soundManager.SetStageNumber(stageNum);
    }
}
