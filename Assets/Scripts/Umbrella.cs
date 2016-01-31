using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UmbrellaState {
	Normal, Large, Disabled
}

public class Umbrella : MonoBehaviour {
	public GameObject subs;

	public List<GameObject> hurtObjects;
	public List<GameObject> nonHurtObjects;

	private float powerUpDuration = 0f;
	private Vector3 oldScale = Vector3.zero;
	private UmbrellaState state = UmbrellaState.Normal;

	// Use this for initialization
	void Start () {
		oldScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (TextManager.instance.GameOver) {
			return;
		}

		if (state != UmbrellaState.Normal && powerUpDuration > 0) {
			powerUpDuration -= Time.deltaTime;
			if (powerUpDuration < 0) {
				if (state == UmbrellaState.Disabled) {
					foreach (GameObject go in hurtObjects)
					{
						go.SetActive(false);
					}
					foreach (GameObject go in nonHurtObjects)
					{
						go.SetActive(true);
					}
				} else if(state == UmbrellaState.Large) {
					subs.SetActive(false);
				}
			}
		}
		Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		transform.position =  new Vector3(mouseScreenPosition.x, Mathf.Clamp(mouseScreenPosition.y, -2f, 8f), 0);
	}

	public void CollideWithBird(Bird b) {
		if (b.isGood) {
			state = UmbrellaState.Large;
			powerUpDuration = 4f;
			subs.SetActive(true);
		} else {
			powerUpDuration = 2f;
			state = UmbrellaState.Disabled;

			foreach (GameObject go in hurtObjects)
			{
				go.SetActive(true);
			}
			foreach (GameObject go in nonHurtObjects)
			{
				go.SetActive(false);
			}
		}
	}
}
