using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public Witch witch;
	public Umbrella umbrella;
	public const float CAMERASPEED = 1f;

	public Vector3 JustOffscreen {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 10));
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + CAMERASPEED*Time.deltaTime, transform.position.y, transform.position.z);
	}
}
