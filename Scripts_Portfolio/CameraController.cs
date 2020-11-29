using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    int cameraSpeed;
    Vector3 startPos;
    Vector3 endPos;

	// Use this for initialization
	void Start () {
        cameraSpeed = 5;
        startPos = new Vector3(-56.5f, 24.0f, -1.0f);
        endPos = new Vector3(0.5f, 24.0f, -1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        CameraMove();
    }

    void CameraMove()
    {
        if (transform.position.x >= endPos.x)
            transform.position = startPos;
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * cameraSpeed);
    }
}
