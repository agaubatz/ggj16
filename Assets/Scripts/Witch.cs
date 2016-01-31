using UnityEngine;
using System.Collections;

public enum WitchState {
	Spellcasting, Walking
}

public class Witch : MonoBehaviour {
	public ScoreManager score;
	private const float UPDATEEVERY = 2f;
	private const float MAXDISTFROMCAMERA = 5.5f;
	private const float SPEEDSTDEV = 0.5f;
	private const float DAMP = 1f;
	private float timeSinceUpdate = UPDATEEVERY; //So it updates frame 1
	private float speed = FollowCamera.CAMERASPEED;
	private float oldSpeed = FollowCamera.CAMERASPEED;
	private WitchState state = WitchState.Walking;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceUpdate += Time.deltaTime;
		if (timeSinceUpdate >= UPDATEEVERY) {
			timeSinceUpdate = 0;
			UpdateState();
		}

		float mySpeed = speed;
		if (timeSinceUpdate < DAMP) {
			mySpeed = oldSpeed + (speed-oldSpeed)*(float)Easing.QuadEaseInOut(timeSinceUpdate, 0, 1, DAMP);
		}
		if (speed > 1) {
			animator.speed = Mathf.Abs (mySpeed);	
		} else {
			animator.speed = 1;
		}

		transform.localPosition = new Vector3(transform.localPosition.x + mySpeed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
	}

	void UpdateState() {
		if (state == WitchState.Spellcasting) { //You successfully cast the spell!
			score.score += 5;
		}

		float rand = Random.value;
		float cameraXdest = Camera.main.transform.position.x + UPDATEEVERY * FollowCamera.CAMERASPEED;
		if (rand < 0.2f && cameraXdest - transform.localPosition.x < MAXDISTFROMCAMERA) { //20% of idle spellcasting, unless it's going to put us off screen
			Debug.Log("Spellcasting");
			state = WitchState.Spellcasting;
			speed = 0;
			animator.SetFloat("speed", 0);
		} else {
			Debug.Log("Walking");
			state = WitchState.Walking;
			float destX = transform.localPosition.x;
			float cameraX = Camera.main.transform.position.x + UPDATEEVERY * FollowCamera.CAMERASPEED;
			oldSpeed = speed;
			int rerollLimit = 10;
			do {
				speed = RandomFromDistribution.RandomNormalDistribution (FollowCamera.CAMERASPEED*1.2f, SPEEDSTDEV);
				destX = transform.localPosition.x + speed * UPDATEEVERY;
				rerollLimit--;
			} while (Mathf.Abs(destX - cameraX) > MAXDISTFROMCAMERA && rerollLimit > 0);
			animator.SetFloat ("speed", speed);
		}
	}
}
