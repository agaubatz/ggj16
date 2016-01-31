using UnityEngine;
using System.Collections;

public enum UmbrellaState {
	Normal, Large, Disabled
}

public class Umbrella : MonoBehaviour {
	private float powerUpDuration = 0f;
	private Vector3 oldScale = Vector3.zero;
	private UmbrellaState state = UmbrellaState.Normal;

	// Use this for initialization
	void Start () {
		oldScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (state != UmbrellaState.Normal && powerUpDuration > 0) {
			powerUpDuration -= Time.deltaTime;
			if (powerUpDuration < 0) {
				if (state == UmbrellaState.Disabled) {
					GetComponentInChildren<Collider2D> ().enabled = true;
					SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer> ();
					renderer.color = new Color(1f, 1f, 1f, 1f);
				} else if(state == UmbrellaState.Large) {
					transform.localScale = oldScale;
				}
			}
		}
		Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position =  new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0);
	}

	public void CollideWithBird(Bird b) {
		if (b.isGood) {
			state = UmbrellaState.Large;
			powerUpDuration = 4f;
			transform.localScale = new Vector3 (2f * oldScale.x, oldScale.y, oldScale.z);
		} else {
			powerUpDuration = 2f;
			state = UmbrellaState.Disabled;
			GetComponentInChildren<Collider2D> ().enabled = false;
			SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer> ();
			renderer.color = new Color(1f, 1f, 1f, 0.5f);
		}
	}
}
