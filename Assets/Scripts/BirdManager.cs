using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdManager : MonoBehaviour {
	public static BirdManager instance;
	public GameObject BirdPrefab;
	private List<Bird> birds = new List<Bird>();
	private const float summonEvery = 2f;
	private float lastSummon = 2f;

	void Awake() {
		instance = this;
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
			if (b.toDestroy || (FollowCamera.instance.ScreenLeft.x > b.transform.position.x + BirdPrefab.GetComponent<SpriteRenderer> ().bounds.extents.x)) {
				deadBirds.Add (b);
			}
		}
		foreach (Bird b in deadBirds) {
			birds.Remove (b);
			Destroy (b.gameObject);
		}
	}

	void SummonBird() {
		float x = FollowCamera.instance.ScreenRight.x + BirdPrefab.GetComponent<SpriteRenderer> ().bounds.extents.x;
		float y = Random.Range(-2f, 10f);
		Vector3 summonPosition = new Vector3(x, y, BirdPrefab.transform.position.z);
		Bird b = ((GameObject)Instantiate (BirdPrefab, summonPosition, Quaternion.identity)).GetComponent<Bird> ();
		birds.Add(b);
		if (Random.value > 0.5f) {
			b.MakeEvil ();
		}
	}
}
