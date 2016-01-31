using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	public bool toDestroy = false;
	public bool isGood = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(transform.localPosition.x - 5f*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
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
