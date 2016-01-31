using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {
	const float RAINHEIGHT = 10;
	const int NUMRAINDROPS = 100;
	const float DAMAGETHRESHOLD = .25f;
	public WitchManager witches;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Witch witch in witches.Witches) {
			if (witch.State == WitchState.Offscreen) {
				continue;
			}

			Bounds witchBounds = witch.GetComponent<BoxCollider2D>().bounds;
			int numCollided = 0;
			float total = 0;
			for(float x = witchBounds.min.x; x <= witchBounds.max.x; x += witchBounds.extents.x/NUMRAINDROPS) {
				total++;
				RaycastHit2D hit = Physics2D.Raycast (new Vector2 (x, RAINHEIGHT), Vector2.down);
				if(hit.collider != null && hit.collider.gameObject == witch.gameObject) {
					numCollided++;
				}
			}

			float damage = numCollided / total;

			if (damage > DAMAGETHRESHOLD) {
				witch.State = WitchState.Damaged;
			}
		}
	}
}
