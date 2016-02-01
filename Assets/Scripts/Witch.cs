using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WitchState {
	Offscreen, Spellcasting, Idle, Walking, Lunging, Damaged, Melted
}

public class Witch : MonoBehaviour {
	public GameObject ShinyPrefab;
	public GameObject TeleportTo;
	public GameObject TeleportAway;
	public AudioClip TeleportInSound;
	public AudioClip TeleportOutSound;
	public AudioClip ScreamSound;
	private GameObject myShiny = null;
	private GameObject myTeleportTo = null;
	private GameObject myTeleportAway = null;
	private float updateIn = 0f;
	private float timeSince = 0f;
	private float speed = 1f;
	private float vulnerableCountdown = 2f;
	private bool startInvulnerable = true;
	private bool wasDamaged = false;
	private WitchState state = WitchState.Offscreen;

	public Dictionary<WitchColor, Color> colors = new Dictionary<WitchColor, Color>();

	public WitchState State {
		set {
			//Debug.Log(string.Format("State from {0} to {1}", state, value));
			state = value;
		}
		get {
			return state;
		}
	}

	public GameObject MyShiny {
		get {
			return myShiny;
		}
	}

	public bool CannotHit = false;

	private float health = 2f;

	private Animator animator;

	private static Color[] SkinOptions = new Color[]{ColorBible.AndroidGreen, ColorBible.DollarBill, ColorBible.GreenRyb, ColorBible.JuneBud, ColorBible.Olivine, ColorBible.Pistachio};
	private static Color[] HatOptions = new Color[]{ColorBible.AzureWebAzureMist, ColorBible.CanaryYellow, ColorBible.Capri, ColorBible.Daffodil, ColorBible.DeepMagenta, ColorBible.ElectricViolet, ColorBible.HotMagenta, ColorBible.LawnGreen, ColorBible.NeonCarrot, ColorBible.OliveDrab7, ColorBible.Ruby, ColorBible.Russet, ColorBible.VividTangerine, ColorBible.UpMaroon};
	private static Color[] HairOptions = new Color[]{ColorBible.Wenge, ColorBible.MediumTaupe, ColorBible.Charcoal, ColorBible.Black, ColorBible.SmokyBlack, ColorBible.Bistre, ColorBible.Onyx, ColorBible.Heliotrope, ColorBible.Jade};

	void Awake() {
		colors [WitchColor.Skin] = SkinOptions [Random.Range (0, SkinOptions.Length)];
		colors[WitchColor.Hat] = HatOptions [Random.Range (0, HatOptions.Length)];
		colors[WitchColor.Hair] = HairOptions [Random.Range (0, HairOptions.Length)];
		colors[WitchColor.Robe] = HatOptions [Random.Range (0, HatOptions.Length)];

	}

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
		myTeleportTo = (GameObject)Instantiate (TeleportTo, transform.localPosition, Quaternion.identity);
		CannotHit = true;
		AudioSource.PlayClipAtPoint (TeleportInSound, transform.localPosition);
	}
	
	// Update is called once per frame
	void Update () {
		if (State == WitchState.Melted)
			return;

		updateIn -= Time.deltaTime;
		timeSince += Time.deltaTime;
		if (updateIn <= 0 || State == WitchState.Damaged) {
			updateIn = UpdateState ();
			timeSince = 0;
		}

		if (startInvulnerable) {
			vulnerableCountdown -= Time.deltaTime;
			if (vulnerableCountdown <= 0) {
				CannotHit = false;
				startInvulnerable = false;
			}
		}

		if(updateIn <= 0.5f && (State == WitchState.Lunging || State == WitchState.Spellcasting)) { //End the animation!
			animator.SetBool ("DoWitchyAction", false);
		}

		float walkSpeed = speed;
		if (timeSince <= 0.5f && State == WitchState.Walking) { //Ease into walking speed
			walkSpeed = speed * (float)Easing.QuadEaseInOut (timeSince, 0, 1, 0.5f);
		}
		if (timeSince >= 0.5f && timeSince <= 1f && State == WitchState.Lunging) { //Speed up lunge
			walkSpeed = 8f * (float)Easing.CubicEaseIn (timeSince-0.5f, 0, 1, 0.5f);
		} else if (timeSince > 1f && State == WitchState.Lunging) {
			walkSpeed = 8f;
		}
		if (updateIn <= 0.5f && State == WitchState.Lunging) { //Slow down at the end of the lunge
			walkSpeed = 8f * (float)Easing.CubicEaseOut (updateIn, 1, 0, 0.5f);
		}

		if (updateIn <= 1.5f && updateIn + Time.deltaTime > 1.5f && State == WitchState.Spellcasting) { //Start Teleport
			CannotHit = true;
			foreach (var child in GetComponentsInChildren<Renderer>()) {
				child.enabled = false;
			}
			myTeleportTo = (GameObject)Instantiate (TeleportTo, new Vector3 (transform.localPosition.x - 5f, transform.localPosition.y, transform.localPosition.z), Quaternion.identity);
			myTeleportAway = (GameObject)Instantiate (TeleportAway, transform.localPosition, Quaternion.identity);
			AudioSource.PlayClipAtPoint (TeleportOutSound, transform.localPosition);
		} else if (updateIn <= 0.5f && updateIn + Time.deltaTime > 0.5f && State == WitchState.Spellcasting) { //End teleport
			transform.localPosition = new Vector3 (transform.localPosition.x - 5f, transform.localPosition.y, transform.localPosition.z);
			AudioSource.PlayClipAtPoint (TeleportInSound, transform.localPosition);
			foreach (var child in GetComponentsInChildren<Renderer>()) {
				child.enabled = true;
			}
			CannotHit = false;
		}

		transform.localPosition = new Vector3(transform.localPosition.x + walkSpeed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);

		if (myShiny != null && State != WitchState.Lunging) {
			Destroy (myShiny, 2f);
			myShiny.GetComponent<Animator>().SetTrigger("Break");
			myShiny = null;
		}

		if (myTeleportTo != null && State != WitchState.Spellcasting) {
			Destroy (myTeleportTo, 5f);
			myTeleportTo = null;
			Destroy (myTeleportAway, 5f);
			myTeleportAway = null;
		}
	}

	float UpdateState() {
		if (state == WitchState.Spellcasting) { //You successfully cast the spell!
			ScoreManager.instance.score += 5;
		} else if (state == WitchState.Lunging) { //You successfully got the shiny!
			ScoreManager.instance.score += 10;
		}

		animator.SetBool ("Walk", false);
		animator.SetBool ("DoWitchyAction", false);
		animator.SetBool ("Idle", false);

		float rand = Random.value;
		if (rand < 0.25f || ((startInvulnerable || state == WitchState.Spellcasting || state == WitchState.Lunging || state == WitchState.Damaged) && rand < 0.5f)) {
			//Walking
			State = WitchState.Walking;
			//oldSpeed = speed;
			speed = RandomFromDistribution.RandomNormalDistribution (2f, 0.5f);
			animator.SetBool ("Walk", true);
			return 2f; //2 Seconds
		} else if (rand < 0.5f || startInvulnerable || state == WitchState.Spellcasting || state == WitchState.Lunging || state == WitchState.Damaged) {
			//Idle
			State = WitchState.Idle;
			speed = 0;
			animator.SetBool ("Idle", true);
			return 1f; //1 Second
		} else if (rand < 0.75f) {
			//Lunging
			State = WitchState.Lunging;
			Vector3 shinySummon = transform.localPosition;
			shinySummon = new Vector3 (shinySummon.x + 6f, shinySummon.y - 1f, shinySummon.z);
			if (myShiny != null) {
				Destroy (myShiny);
			}
			myShiny = (GameObject)Instantiate (ShinyPrefab, shinySummon, Quaternion.identity);
			speed = 0;
			animator.SetBool ("DoWitchyAction", true);
			animator.SetFloat ("WitchyAction", 1);
			return 2f; //2 Seconds
		} else {
			//Spellcasting
			State = WitchState.Spellcasting;
			speed = 0;
			animator.SetBool ("DoWitchyAction", true);
			animator.SetFloat ("WitchyAction", 0);
			return 3.5f; //3.5 Seconds
		}
	}

	public void DealDamage(float damage) {
		if (state == WitchState.Melted)
			return;
		health -= damage;
		speed = 0;

		switch(state) {
		case WitchState.Idle:
			animator.SetBool ("Idle", false);
			break;
		case WitchState.Walking:
			animator.SetBool ("Walk", false);
			break;
		case WitchState.Lunging:
		case WitchState.Spellcasting:
			animator.SetBool ("DoWitchyAction", false);
			break;
		}

		if (health <= 0) {
			state = WitchState.Melted;
			animator.SetBool ("Die", true);
		} else {
			if (!wasDamaged) {
				wasDamaged = true;
				AudioSource.PlayClipAtPoint (ScreamSound, transform.localPosition);
			}
			state = WitchState.Damaged;
			animator.SetBool("Hit", true);
		}


	}

	public void CoveredByUmbrella() {
		animator.SetBool("Hit", false);

		wasDamaged = false;
	}
}
