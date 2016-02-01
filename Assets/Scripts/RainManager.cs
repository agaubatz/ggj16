using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {
	const float RAINHEIGHT = 10;
	const int NUMRAINDROPS = 100;
	const float DAMAGETHRESHOLD = .25f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (TextManager.instance.GameOver) {
			return;
		}

		foreach (Witch witch in WitchManager.instance.Witches) {
			if (witch.State == WitchState.Offscreen || witch.State == WitchState.Melted || witch.CannotHit) {
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
			damage = 0;

			if (damage > DAMAGETHRESHOLD) {
				witch.DealDamage (damage * Time.deltaTime);
			} else {
				witch.CoveredByUmbrella();
			}
		}
	}
}
