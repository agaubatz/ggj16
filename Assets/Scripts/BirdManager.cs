using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour {
	public static BirdManager instance;
	public GameObject GoodPrefab;
	public GameObject BadPrefab;
	private List<Bird> birds = new List<Bird>();
	private const float summonEvery = 2f;
	private float lastSummon = 2f;

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

		lastSummon -= Time.deltaTime;
		if (lastSummon <= 0) {
			lastSummon = RandomFromDistribution.RandomNormalDistribution(summonEvery, 0.5f);
			SummonBird();
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
	}

	void SummonBird() {
		if (Random.value > 0.5f) {
			float x = FollowCamera.instance.ScreenRight.x + goodBounds.extents.x;
			float y = Random.Range (-2f, 10f);
			Vector3 summonPosition = new Vector3 (x, y, GoodPrefab.transform.position.z);
			Bird b = ((GameObject)Instantiate (GoodPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
			birds.Add (b);
		} else {
			float x = FollowCamera.instance.ScreenRight.x + badBounds.extents.x;
			float y = Random.Range (-2f, 10f);
			Vector3 summonPosition = new Vector3 (x, y, BadPrefab.transform.position.z);
			Bird b = ((GameObject)Instantiate (BadPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
			birds.Add (b);
			b.MakeEvil ();
		}
	}
}
