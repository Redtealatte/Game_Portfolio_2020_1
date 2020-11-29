using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour {

    List<Dictionary<string, object>> data;
    
    // Use this for initialization
    void Start () {
        data = CSVReader.Read("dialogue");//Resources폴더의 dialogue파일을 읽어들여서 List로 된 Dictionary에 키(string)와 값으로 저장.
    }
	
    //원하는 데이터를 id와 index를 입력해 추출.
	public string GetTalk(int id, int index)
    {
        if(id % 100 != 0)
            //퀘스트 시 나오는 대화 + 아이템 관련 대화.
            for (var i = 0; i < data.Count; i++)
                if ((int)data[i]["id"] == id && (int)data[i]["index"] == index)
                    return (string)data[i]["text"];//text데이터 반환.

        //기본 대화.
        for (var i = 0; i < data.Count; i++)
            if ((int)data[i]["id"] == id - id % 100 && (int)data[i]["index"] == index)
                return (string)data[i]["text"];//text데이터 반환.

        return null;//List내에 id와 index값에 해당하는 데이터를 찾을 수 없으면 null 반환.
    }
}
