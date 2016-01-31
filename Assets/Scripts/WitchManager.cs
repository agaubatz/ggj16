using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchManager : MonoBehaviour {
	public FollowCamera FollowCam;
	public GameObject WitchPrefab;
	private Bounds WitchBounds;
	private List<Witch> witches = new List<Witch>();
	private const float summonEvery = 5f;
	private float lastSummon = 0f;

	public List<Witch> Witches {
		get{
			return witches;
		}
	}

	// Use this for initialization
	void Start () {
		Witch w = Instantiate (WitchPrefab).GetComponent<Witch> ();
		witches.Add(w);
		foreach (var child in w.GetComponentsInChildren<Renderer>()) {
			WitchBounds.Encapsulate (child.bounds);
			}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Witch w in witches) {
			var dist = (w.transform.position - Camera.main.transform.position).z;
			float leftBorder = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, dist)).x;
			float rightBorder = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, dist)).x;
			if (w.transform.position.x + WitchBounds.extents.x < leftBorder || w.transform.position.x + WitchBounds.extents.x > rightBorder) {
				w.State = WitchState.Offscreen;
			}
		}


		lastSummon += Time.deltaTime;
		if (lastSummon >= summonEvery) {
			lastSummon = 0f;
			Vector3 summonPosition = WitchPrefab.transform.position;
			summonPosition.x = FollowCam.JustOffscreen.x + WitchBounds.extents.x;
			summonPosition.y = -3.14f;
			witches.Add(((GameObject)Instantiate(WitchPrefab, summonPosition, Quaternion.identity)).GetComponent<Witch>());
		}
	}
}
