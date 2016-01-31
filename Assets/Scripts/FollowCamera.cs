using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public static FollowCamera instance;
	private const float dampTime = 1f;
	private Vector3 velocity = Vector3.zero;

	public Vector3 ScreenRight {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 10));
		}
	}

	public Vector3 ScreenLeft {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 10));
		}
	}

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Camera.main.orthographicSize = 8f;
	}
	
	// Update is called once per frame
	void Update () {
		Bounds AllWitchBounds = new Bounds();
		bool notSet = true;
		foreach (Witch w in WitchManager.instance.Witches) {
			foreach (var child in w.GetComponentsInChildren<SpriteRenderer>()) {
				if (notSet) {
					notSet = false;
					AllWitchBounds = child.bounds;
				}
				AllWitchBounds.Encapsulate (child.bounds);
			}
			if (w.MyShiny != null) {
				AllWitchBounds.Encapsulate (w.MyShiny.GetComponentInChildren<SpriteRenderer>().bounds);
			}
		}
		Vector3 center = AllWitchBounds.center;
		Vector3 target = new Vector3 (center.x, WitchManager.summonY, transform.position.z);

		/*
		float orthographicSize = Camera.main.orthographicSize;
		Vector3 topRight = new Vector3(AllWitchBounds.max.x, AllWitchBounds.max.y, 0f);
		Vector3 topRightAsViewport = Camera.main.WorldToViewportPoint(topRight);

		if (topRightAsViewport.x >= topRightAsViewport.y) {
			orthographicSize = Mathf.Abs (AllWitchBounds.size.x) / Camera.main.aspect / 2f;
		} else {
			orthographicSize = Mathf.Abs (AllWitchBounds.size.y) / 2f;
		}

		Camera.main.orthographicSize = Mathf.Clamp(Mathf.Lerp(Camera.main.orthographicSize, orthographicSize, Time.deltaTime * 20f), 8f, Mathf.Infinity);
		*/

		Vector3 point = Camera.main.WorldToViewportPoint(target);
		Vector3 delta = target - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.2f, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}
}
