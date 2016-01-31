using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	private List<SpriteRenderer> backgroundPart;

	void Start()
	{
			backgroundPart = new List<SpriteRenderer>();

			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);

				foreach (SpriteRenderer renderer in child.GetComponentsInChildren<SpriteRenderer>())
				{
					backgroundPart.Add(renderer);
				}
			}
	}

	void Update()
	{
		var dist = (transform.position - Camera.main.transform.position).z;
		float leftBorder = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, dist)).x;
		float rightBorder = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, dist)).x;

		backgroundPart = backgroundPart.OrderBy (t => t.transform.position.x).ToList ();
		SpriteRenderer firstChild = backgroundPart.FirstOrDefault ();

		if (firstChild != null) {
			while (firstChild.transform.position.x + firstChild.bounds.extents.x < leftBorder) {
				float newX = rightBorder + 2f * firstChild.bounds.extents.x;
				if (firstChild.sortingLayerName == "Ground") {
					newX = firstChild.transform.position.x + 4f * firstChild.bounds.extents.x;
				}
				firstChild.transform.position = new Vector3 (
					newX,
					firstChild.transform.position.y,
					firstChild.transform.position.z
				);

				// The first part become the last one
				backgroundPart.Remove (firstChild);
				backgroundPart.Add (firstChild);
				firstChild = backgroundPart.FirstOrDefault ();
			}
		}
	}
}