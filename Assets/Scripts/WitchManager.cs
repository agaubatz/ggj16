﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchManager : MonoBehaviour {
	public static WitchManager instance;
	public GameObject WitchPrefab;
	private Bounds WitchBounds;
	private List<Witch> witches = new List<Witch>();
	private Dictionary<Witch, float> deadWitches = new Dictionary<Witch, float>();
	private const float summonEvery = 5f;
	private const float destroyAnimation = 3f;
	public const float summonY = -3.14f;
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
		List<Witch> removeFromDead = new List<Witch> ();
		foreach (Witch w in new List<Witch>(deadWitches.Keys)) {
			deadWitches[w] -= Time.deltaTime;
			if (deadWitches [w] <= 0) {
				witches.Remove (w);
				removeFromDead.Add (w);
			}
		}
		foreach (Witch w in removeFromDead) {
			deadWitches.Remove (w);
			Destroy (w);
		}

		foreach (Witch w in witches) {
			if (w.State == WitchState.Melted) {
				if (!deadWitches.ContainsKey (w)) {
					deadWitches.Add (w, destroyAnimation);
				}
				continue;
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
		summonPosition.y = summonY;
		Witch w = ((GameObject)Instantiate (WitchPrefab, summonPosition, Quaternion.identity)).GetComponent<Witch> ();
		witches.Add(w);
		return w;
	}
}
