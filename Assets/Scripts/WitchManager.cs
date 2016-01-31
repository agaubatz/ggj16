using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchManager : MonoBehaviour {
	public static WitchManager instance;
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

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Witch w = SummonWitch(-4);
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
			SummonWitch(FollowCamera.instance.JustOffscreen.x + WitchBounds.extents.x);
		}
	}

	Witch SummonWitch(float xPos) {
		Vector3 summonPosition = WitchPrefab.transform.position;
		summonPosition.x = xPos;
		summonPosition.y = -3.14f;
		Witch w = ((GameObject)Instantiate (WitchPrefab, summonPosition, Quaternion.identity)).GetComponent<Witch> ();
		witches.Add(w);
		return w;
	}

	public void KillWitch(Witch witch) {
		witches.Remove(witch);
	}
}
