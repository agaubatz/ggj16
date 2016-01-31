using UnityEngine;
using System.Collections;

public enum WitchState {
	Offscreen, Spellcasting, Idle, Walking, Lunging, Damaged, Melted
}

public class Witch : MonoBehaviour {
	private const float UPDATEEVERY = 2f;
	private const float DAMP = 1f;
	private float timeSinceUpdate = UPDATEEVERY; //So it updates frame 1
	private float speed = 1f;
	private float oldSpeed = 1f;
	private WitchState state = WitchState.Offscreen;
	public WitchState State {
		set {
			Debug.Log(string.Format("State from {0} to {1}", state, value));
			state = value;
		}
		get {
			return state;
		}
	}

	public float health = 5f;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (State == WitchState.Melted)
			return;

		timeSinceUpdate += Time.deltaTime;
		if (timeSinceUpdate >= UPDATEEVERY || State == WitchState.Damaged) {
			timeSinceUpdate = 0;
			UpdateState();
		}
			
		animator.SetBool ("DoWitchyAction", State == WitchState.Spellcasting);

		float mySpeed = speed;
		if (timeSinceUpdate < DAMP && state == WitchState.Walking) {
			mySpeed = oldSpeed + (speed-oldSpeed)*(float)Easing.QuadEaseOut(timeSinceUpdate, 0, 1, DAMP);
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
			ScoreManager.instance.score += 5;
		}

		float rand = Random.value;
		if (rand < 0.2f) {
			State = WitchState.Spellcasting;
			oldSpeed = speed;
			speed = 0;
		} else if(rand < 0.3f) {
			State = WitchState.Idle;
			oldSpeed = speed;
			speed = 0;
			animator.SetFloat ("speed", 0);
		} else if (rand < 0.9f) {
			State = WitchState.Walking;
			oldSpeed = speed;
			speed = RandomFromDistribution.RandomNormalDistribution (1.2f, .25f);
			animator.SetFloat ("speed", speed);
		} else {
			State = WitchState.Lunging;
			oldSpeed = speed;
			speed = RandomFromDistribution.RandomNormalDistribution (4f, .25f);
			animator.SetFloat ("speed", speed);
		}
	}

	public void DealDamage(float damage) {
		if (state == WitchState.Melted)
			return;
		health -= damage;

		if (health <= 0) {
			state = WitchState.Melted;
			animator.SetBool ("Die", true);
		} else {
			state = WitchState.Damaged;
			animator.SetBool("Hit", true);
		}
	}

	public void CoveredByUmbrella() {
		animator.SetBool("Hit", false);
	}
}
