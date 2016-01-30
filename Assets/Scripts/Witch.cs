using UnityEngine;
using System.Collections;

public class Witch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector2(transform.localPosition.x + 1f*Time.deltaTime, transform.localPosition.y);
	}
}
