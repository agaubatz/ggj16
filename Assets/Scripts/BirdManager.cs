using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour {
	public static BirdManager instance;
	public GameObject BirdPrefab;
	private List<Bird> birds = new List<Bird>();
	private const float summonEvery = 1f;
	private float lastSummon = 0f;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lastSummon += Time.deltaTime;
		if (lastSummon >= summonEvery) {
			lastSummon = 0f;
			SummonBird();
		}
	}

	void SummonBird() {
		float x = FollowCamera.instance.JustOffscreen.x + BirdPrefab.GetComponent<SpriteRenderer> ().bounds.extents.x;
		float y = RandomFromDistribution.RandomNormalDistribution (3f, 0.5f);
		Vector3 summonPosition = new Vector3(x, y, BirdPrefab.transform.position.z);
		Bird b = ((GameObject)Instantiate (BirdPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
		birds.Add(b);
	}
}
