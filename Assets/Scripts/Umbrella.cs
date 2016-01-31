using UnityEngine;
using System.Collections;

public class Umbrella : MonoBehaviour {
	private float powerUpDuration = 0f;
	private Vector3 oldScale = Vector3.zero;
	private bool inactive = false;

	// Use this for initialization
	void Start () {
		oldScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (powerUpDuration > 0) {
			powerUpDuration -= Time.deltaTime;
			if (powerUpDuration < 0) {
				transform.localScale = oldScale;
			}
		}
		Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position =  new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0);
	}

	public void CollideWithBird(Bird b) {
		if (b.isGood) {
			powerUpDuration = 4f;
			transform.localScale = new Vector3 (2f * oldScale.x, oldScale.y, oldScale.z);
		}
	}
}
