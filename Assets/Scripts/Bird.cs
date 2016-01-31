using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(transform.localPosition.x - 5f*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
	}
}
