using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {
	public Witch witch;
	const float RAINHEIGHT = 10;
	const int NUMRAINDROPS = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Bounds witchBounds = witch.GetComponent<SpriteRenderer>().bounds;
		int numCollided = 0;
		float total = 0;
		for(float x = witchBounds.min.x; x <= witchBounds.max.x; x += witchBounds.extents.x/NUMRAINDROPS) {
			total++;
			RaycastHit2D hit = Physics2D.Raycast (new Vector2 (x, RAINHEIGHT), Vector2.down);
			if(hit.collider != null && hit.collider.gameObject == witch.gameObject) {
				numCollided++;
			}
		}

		if (numCollided > 0) {
			Debug.Log(numCollided/total);
		}
	}
}
