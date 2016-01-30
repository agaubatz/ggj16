using UnityEngine;
using System.Collections;

public class Witch : MonoBehaviour {
	private float timeSinceUpdate = 0f;
	private const float UPDATEEVERY = 2f;
	private const float MAXDISTFROMCAMERA = 6f;
	private const float SPEEDSTDEV = 0.5f;
	private float speed = FollowCamera.CAMERASPEED;
	private Vector3 velocity = Vector3.zero;
	private const float DAMP = 0.05f;

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
			do {
				speed = RandomFromDistribution.RandomNormalDistribution (FollowCamera.CAMERASPEED, SPEEDSTDEV);
				destX = transform.localPosition.x + speed * UPDATEEVERY;
			} while (Mathf.Abs(destX - cameraX) > MAXDISTFROMCAMERA);
		}
		//transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(transform.localPosition.x + speed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z), ref velocity, DAMP);
		transform.localPosition = new Vector3(transform.localPosition.x + speed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
	}
}
