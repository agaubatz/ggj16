using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour {
	public static BirdManager instance;
	public GameObject GoodPrefab;
	public GameObject BadPrefab;
	public GameObject BadSpawner;
	private List<Bird> birds = new List<Bird>();
	private Dictionary<GameObject, float> badSpawns = new Dictionary<GameObject, float>();
	private float lastSummonBad = 20f;
	private float lastSummonGood = 10f;

	private Bounds goodBounds;
	private Bounds badBounds;

	void Awake() {
		instance = this;

		goodBounds = new Bounds();
		badBounds = new Bounds();
		foreach (var child in GoodPrefab.GetComponentsInChildren<SpriteRenderer>()) {
			goodBounds.Encapsulate (child.bounds);
		}
		foreach (var child in BadPrefab.GetComponentsInChildren<SpriteRenderer>()) {
			badBounds.Encapsulate (child.bounds);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (TextManager.instance.GameOver) {
			return;
		}

		lastSummonBad -= Time.deltaTime;
		if (lastSummonBad <= 0) {
			if (Time.time <= 40f) {
				lastSummonBad = RandomFromDistribution.RandomNormalDistribution(8f, 2f);
			} else if (Time.time <= 60f) {
				lastSummonBad = RandomFromDistribution.RandomNormalDistribution(4f, 1f);
			} else if (Time.time <= 120f) {
				lastSummonBad = RandomFromDistribution.RandomNormalDistribution(2f, 0.5f);
			} else {
				lastSummonBad = RandomFromDistribution.RandomNormalDistribution(1f, 0.25f);
			}

			SummonBad();
		}

		lastSummonGood -= Time.deltaTime;
		if (lastSummonGood <= 0) {
			lastSummonGood = RandomFromDistribution.RandomNormalDistribution(10f, 2f);
			SummonGood ();
		}

		List<Bird> deadBirds = new List<Bird> ();
		foreach (Bird b in birds) {
			if (b.toDestroy || (b.isGood && FollowCamera.instance.ScreenLeft.x > b.transform.position.x + goodBounds.extents.x) || (!b.isGood && FollowCamera.instance.ScreenLeft.x > b.transform.position.x + badBounds.extents.x)) {
				deadBirds.Add (b);
			}
		}
		foreach (Bird b in deadBirds) {
			birds.Remove (b);
			Destroy (b.gameObject);
		}

		List<GameObject> destroyList = new List<GameObject> ();
		foreach (GameObject g in new List<GameObject>(badSpawns.Keys)) {
			badSpawns[g] -= Time.deltaTime;
			if (badSpawns[g] <= 0) {
				destroyList.Add (g);
			}
		}

		foreach (GameObject g in destroyList) {
			badSpawns.Remove(g);
			Destroy (g);
		}
	}

	void SummonGood() {
		float x = FollowCamera.instance.ScreenRight.x + goodBounds.extents.x;
		float y = Random.Range (-2f, 10f);
		Vector3 summonPosition = new Vector3 (x, y, GoodPrefab.transform.position.z);
		Bird b = ((GameObject)Instantiate (GoodPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
		birds.Add (b);
	}

	void SummonBad() {
		float x = FollowCamera.instance.ScreenRight.x + badBounds.extents.x;
		float y = Random.Range (-1f, 9f);
		Vector3 summonPosition = new Vector3 (x + 30f * 1.5f, y, BadPrefab.transform.position.z); //Summon 15f*0.5f back for the animation to finish
		Bird b = ((GameObject)Instantiate (BadPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
		birds.Add (b);
		b.MakeEvil ();
		Vector3 badSpawnSummon = new Vector3 (x, y, BadSpawner.transform.position.z);
		badSpawns.Add ((GameObject)Instantiate (BadSpawner, badSpawnSummon, Quaternion.identity), 1.5f);
	}
}
