using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public Witch witch;
	public Umbrella umbrella;
	//public const float dampTime = 0.15f;
	//private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x + 1f*Time.deltaTime, transform.position.y, transform.position.z);
	}
}
