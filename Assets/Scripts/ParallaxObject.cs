using UnityEngine;
using System.Collections;

public class ParallaxObject : MonoBehaviour {
	public Vector2 speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(speed.x*Time.deltaTime, 0));
	}
}
