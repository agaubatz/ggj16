using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchManager : MonoBehaviour {
	public GameObject WitchPrefab;
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
		witches.Add(Instantiate(WitchPrefab).GetComponent<Witch>());
	}
	
	// Update is called once per frame
	void Update () {
		lastSummon += Time.deltaTime;
		if (lastSummon >= summonEvery) {
			lastSummon = 0f;
			witches.Add(Instantiate(WitchPrefab).GetComponent<Witch>());
		}
	}
}
