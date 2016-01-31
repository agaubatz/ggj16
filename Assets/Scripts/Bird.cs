using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	public bool toDestroy = false;
	public bool isGood = true;
	private float timeOnScreen = 0f;
	private float destination = 0f;
	private float startY = 0f;

	// Use this for initialization
	void Start () {
		startY = transform.position.y;
		if (transform.position.y <= 4f) {
			destination = 4f;
		} else {
			destination = -4f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeOnScreen += Time.deltaTime;
		float progress = 0;
		if (timeOnScreen <= 2f) {
			progress = (float)Easing.SineEaseOut (timeOnScreen, 0, 1, 2f);
		} else {
			progress = 1f-(float)Easing.SineEaseIn(timeOnScreen-2f, 0, 1, 2f);
		}
		transform.localPosition = new Vector3(transform.localPosition.x - 5f*Time.deltaTime, startY + progress*destination, transform.localPosition.z);
	}

	public void MakeEvil() {
		isGood = false;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.color = new Color(1f, 0f, 0f, 1f);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.GetComponentInParent<Umbrella>() != null) {
			Umbrella u = coll.GetComponentInParent<Umbrella>();
			u.CollideWithBird(this);
			toDestroy = true;
		}
	}
}
