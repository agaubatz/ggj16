using UnityEngine;
using System.Collections;

public enum WitchColor
{
	Hat,
	Robe,
	Skin,
	Hair
}

public class WitchColoredPiece : MonoBehaviour {
	public WitchColor color;

	private Witch witch;

	// Use this for initialization
	void Start () {
		witch = GetComponentInParent<Witch>();

		GetComponent<SpriteRenderer>().color = witch.colors[color];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
