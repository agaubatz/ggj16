using UnityEngine;
using System.Collections;

public class Umbrella : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position =  new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0);
	}
}
