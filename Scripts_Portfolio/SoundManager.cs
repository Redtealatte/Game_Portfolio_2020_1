using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    Dictionary<string, AudioClip> soundList;
    AudioSource audioSource;
    AudioSource backgroundAudio;
    int stageNum;
    float maxVolume;
    float volumeDiv;
    int playingCount;
    int playingMaxCount;
    bool resetCount;

    public bool soundChange;

	// Use this for initialization
	void Awake () {
        audioSource = GetComponent<AudioSource>();
        backgroundAudio = gameObject.AddComponent<AudioSource>();
        soundList = new Dictionary<string, AudioClip>();
        GenerateSound();
        SetMaxVolume(0.4f);
        playingMaxCount = 3;
    }

    void Update()
    {
        if (soundChange)
            VolumeDown();
        else if (!soundChange)
            SoundBody();        
    }
	
	void GenerateSound()
    {
        soundList.Add("Apple", Resources.Load("Sounds/DM-CGS-45") as AudioClip);
        soundList.Add("Die", Resources.Load("Sounds/Die_1") as AudioClip);
        soundList.Add("Coin", Resources.Load("Sounds/Item3") as AudioClip);
        soundList.Add("Key", Resources.Load("Sounds/DM-CGS-26") as AudioClip);
        soundList.Add("Box", Resources.Load("Sounds/DM-CGS-32") as AudioClip);
        soundList.Add("EnemyDie", Resources.Load("Sounds/Jump3") as AudioClip);
        soundList.Add("Click", Resources.Load("Sounds/Click") as AudioClip);
        soundList.Add("Flag", Resources.Load("Sounds/DM-CGS-15") as AudioClip);
        soundList.Add("Happy", Resources.Load("Sounds/Background/H_Full") as AudioClip);
        soundList.Add("Peaceful", Resources.Load("Sounds/Background/P_Full") as AudioClip);
        soundList.Add("Thinker", Resources.Load("Sounds/Background/T_Full") as AudioClip);
        soundList.Add("Excited", Resources.Load("Sounds/Background/E_Full") as AudioClip);
    }

    //입력받은 soundname을 가지고 SoundList에서 해당 키값에 맞는 AudioClip을 AudioSource로 출력.
    public void SoundEffectPlay(string soundname)
    {
        audioSource.PlayOneShot(soundList[soundname]);
    }

    public void VolumeControl(float volumesize)
    {
        audioSource.volume = volumesize;
    }

    //배경음 설정.
    public void SetBackgroundNumber(int stageNum)
    {
        this.stageNum = stageNum;
        soundChange = true;
    }

    void SoundBody()
    {
        //배경음이 실행된지 않았고 카운트가 0일 때, 볼륨은 0
        if (!backgroundAudio.isPlaying && playingCount == 0)
            backgroundAudio.volume = 0.0f;
        //배경음의 볼륨이 0부터 maxVolume까지 5초동안 커짐. 첫번째 실행일때만 적용.
        else if (backgroundAudio.isPlaying && backgroundAudio.volume <= maxVolume && playingCount == 1)
            backgroundAudio.volume += Time.deltaTime * volumeDiv;

        //배경음 반복부분.
        if (!backgroundAudio.isPlaying && playingCount < playingMaxCount)
        {
            BackgroundSoundPlay();
            playingCount++;
            resetCount = false;
        }
        //배경음이 5초보다 적게 남았고 마지막 반복일때, 볼륨이 점차 작아짐.
        if (backgroundAudio.clip.length - backgroundAudio.time < 5.0f && playingCount == playingMaxCount)
            backgroundAudio.volume -= Time.deltaTime * volumeDiv;

        //재생이 종료되고 X초뒤에 카운트 리셋.
        if (!backgroundAudio.isPlaying && playingCount == playingMaxCount && !resetCount)
        {
            Invoke("CountReset", 3.0f);
            resetCount = true;
        }
            
    }
    
    //스테이지 번호에 따라 배경음악을 다르게 실행시킴.
    void BackgroundSoundPlay()
    {
        string soundName;
        if (stageNum == 0 || stageNum == 4)
            soundName = "Excited";
        else if (stageNum == 1)
            soundName = "Happy";
        else if (stageNum == 2)
            soundName = "Thinker";        
        else
            soundName = "Peaceful";
        backgroundAudio.clip = soundList[soundName];
        backgroundAudio.Play();
    }

    //최대 볼륨 설정.
    public void SetMaxVolume(float maxVolume)
    {
        this.maxVolume = maxVolume;
        volumeDiv = maxVolume / 5.0f;
    }

    //카운트 초기화.
    void CountReset()
    {
        playingCount = 0;
    }

    //스테이지 변경시 노래를 바꾸기위해 볼륨을 천천히 줄여 0이 되고 현재 노래를 멈춘다.
    void VolumeDown()
    {
        backgroundAudio.volume -= Time.deltaTime * volumeDiv;
        if (backgroundAudio.volume <=0.0f)
        {
            backgroundAudio.Stop();
            playingCount = 0;
            soundChange = false;
        }            
    }

    //스테이지 넘버를 설정.
    public void SetStageNumber(int stageNum)
    {
        this.stageNum = stageNum;
    }
}
