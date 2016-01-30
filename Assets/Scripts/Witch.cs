﻿using UnityEngine;
using System.Collections;

public class Witch : MonoBehaviour {
	private const float UPDATEEVERY = 2f;
	private const float MAXDISTFROMCAMERA = 5.5f;
	private const float SPEEDSTDEV = 0.5f;
	private float timeSinceUpdate = UPDATEEVERY; //So it updates frame 1
	private float speed = FollowCamera.CAMERASPEED;
	private float oldSpeed = FollowCamera.CAMERASPEED;
	private const float DAMP = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceUpdate += Time.deltaTime;
		if (timeSinceUpdate >= UPDATEEVERY) {
			timeSinceUpdate = 0;
			float destX = transform.localPosition.x;
			float cameraX = Camera.main.transform.position.x + UPDATEEVERY * FollowCamera.CAMERASPEED;
			oldSpeed = speed;
			int rerollLimit = 10;
			do {
				speed = RandomFromDistribution.RandomNormalDistribution (FollowCamera.CAMERASPEED, SPEEDSTDEV);
				destX = transform.localPosition.x + speed * UPDATEEVERY;
				rerollLimit--;
			} while (Mathf.Abs(destX - cameraX) > MAXDISTFROMCAMERA && rerollLimit > 0);
		}

		float mySpeed = speed;
		if (timeSinceUpdate < DAMP) {
			mySpeed = oldSpeed + (speed-oldSpeed)*(float)Easing.QuadEaseInOut(timeSinceUpdate, 0, 1, DAMP);
		}
		transform.localPosition = new Vector3(transform.localPosition.x + mySpeed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
	}
}
