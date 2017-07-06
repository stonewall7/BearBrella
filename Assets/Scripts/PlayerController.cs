using UnityEngine;
using System.Collections;

/**
 * Responsible the movement of the Avatar 
 * Responsible for animation of the Avatar
 * 
 **/ 

public class PlayerController : MonoBehaviour {

	private GameController controller;

	private GameObject bear;
	private GameObject umbrella;

	public Sprite normalBear;
	public Sprite bearDamage;

	public float MoveSpeed = 10.0F;
	private float currentMoveSpeed;

	public float maxUmbrella = 1.0f;
	public float umbrellaTime;

	public float maxCooldown = 3.0f;
	public float cooldownTime;

	public float dmgInvinc = 2f;
	private float dmgTimer;
	private float lastFlash;

	public bool moveEnabled;

	public AudioClip umbrellaOpen;
	public AudioClip noSound;
	public AudioClip hurtSound;

	private bool playedUmbrellaOpen = false;
	private bool playedNoSound = false;


	void Start() {

		bear = GameObject.Find("Bear");
		umbrella = GameObject.Find ("Umbrella");
		controller = GameObject.Find ("GameManager").GetComponent<GameController> () as GameController;

		umbrella.SetActive (false);
		umbrellaTime = maxUmbrella;

		currentMoveSpeed = MoveSpeed;

		cooldownTime = 0;
		dmgTimer = 0;
		lastFlash = Time.time;

		moveEnabled = true;
	}
	
	void Update () {

		if (!moveEnabled) {
			return;
		}

		flashing ();
		cooldown ();
		umbrellaDrop ();
		move ();
	}

	void flashing(){
		if (dmgTimer > 0) {
			dmgTimer -= Time.deltaTime;
			
			if(Time.time - lastFlash > .1){
				bear.GetComponent<SpriteRenderer>().enabled = !bear.GetComponent<SpriteRenderer>().enabled;
				lastFlash = Time.time;
			}
		}else{
			dmgTimer = 0;
			bear.GetComponent<SpriteRenderer>().enabled = true;
			bear.GetComponent<SpriteRenderer> ().sprite = normalBear;
		}
	}
	
	void cooldown(){
		if(cooldownTime > 0){
			cooldownTime -= Time.deltaTime;
			if(cooldownTime <= 0){
				cooldownTime = 0;
				umbrellaTime = maxUmbrella;
			}
		}
	}

	void umbrellaDrop(){
		float inputV = Input.GetAxis ("Vertical");

		if (cooldownTime <= 0) {
			if (inputV > 0) {
				
				umbrella.SetActive (true);
				controller.lowSpeed ();
				if (!playedUmbrellaOpen){
					audio.PlayOneShot(umbrellaOpen);
				}
				playedUmbrellaOpen = true;
				umbrellaTime -= Time.deltaTime;

				if (umbrellaTime <= 0) {
					playedUmbrellaOpen = false;
					umbrellaTime = 0;
					cooldownTime = maxCooldown;
					umbrella.SetActive (false);
				}
			} else {
				playedUmbrellaOpen = false;
				umbrella.SetActive (false);
				umbrellaTime = Mathf.Min (umbrellaTime + Time.deltaTime / 2, maxUmbrella);
			}
		} else {
			if (inputV > 0) {
				if (!playedNoSound){
					audio.PlayOneShot(noSound);
				}
				playedNoSound = true;
				Invoke("toggleIt", 1.5f);
			}
		}
		
		if (inputV < 0) {
			controller.highSpeed();
		}
		
		if(inputV == 0){
			controller.resetSpeed();
		}
	}

	void move(){
		float inputH = Input.GetAxis ("Horizontal");
		
		if (rigidbody2D.position.x < -3.3f) {
			rigidbody2D.position = new Vector2(-3.3f, rigidbody2D.position.y);
		} else if (rigidbody2D.position.x > 5.8f) {
			rigidbody2D.position = new Vector2(5.8f, rigidbody2D.position.y);
		}
		
		if (inputH > 0 && rigidbody2D.position.x != 5.8f) 
		{
			rigidbody2D.MovePosition (rigidbody2D.position + Vector2.right * currentMoveSpeed * Time.deltaTime);
		}
		if (inputH < 0 && rigidbody2D.position.x != -3.3f) 
		{
			rigidbody2D.MovePosition (rigidbody2D.position - Vector2.right * currentMoveSpeed * Time.deltaTime);
		}
	}

	void damaged(){
		dmgTimer = dmgInvinc;
		bear.GetComponent<SpriteRenderer> ().sprite = bearDamage;
		controller.lives--;
		audio.PlayOneShot(hurtSound);
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Spikes") {
			if(dmgTimer <= 0){
				damaged();
			}
			Debug.Log ("Spike collide");
		}
		if (col.gameObject.tag == "Bomb") {
			if(dmgTimer <= 0){
				damaged();
			}
			Debug.Log ("Bomb collide");
		}
		if (col.gameObject.tag == "life_up") {
			Debug.Log ("Moar lives");
			col.gameObject.renderer.enabled = false;
			col.gameObject.collider2D.enabled = false;
			GameObject.Find ("GameManager").SendMessage("addLife");
			GameObject.Find ("SoundFX").SendMessage("playPickup");
		}
		if (col.gameObject.tag == "Rocket") {
			if(dmgTimer <= 0){
				damaged();
			}
			Debug.Log ("Rocket collide");
		}
	}

	void toggleIt(){
		playedNoSound = false;
	}
}
