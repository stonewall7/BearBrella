using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {
	
	private GameController controller;
	private GameObject player;

	public SpriteRenderer spriteRender;
	public Sprite bomb;
	public Sprite explode;

	public float bombSpeed = 20.0f;
	public float fuseTime = 1.5f;
	public float explosionTime = 2.0f;

	private float bombGravity = 1.0f;

	private bool exploded = false;
	private bool dropped = false;

	public AudioClip tick;
	public AudioClip boom;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Avatar");
		controller = GameObject.Find ("GameManager").GetComponent<GameController> () as GameController;

		rigidbody2D.gravityScale = 0;
		collider2D.enabled = false;
	}

	void Update () {
		if ((player.transform.position.y < (transform.position.y + 4) && !dropped)) {
			dropped = true;
			rigidbody2D.gravityScale = bombGravity;
			sendFlying();
		}
		if(controller.currState == GameController.STATES.PAUSE){
			Time.timeScale = 0;
		}else{
			Time.timeScale = 1;
		}
	}

	void sendFlying(){
		audio.PlayOneShot (tick);
		rigidbody2D.AddForce (Vector3.left * bombSpeed * Random.Range(10,30));
		Invoke ("Explode", fuseTime);
	}

	void Explode() {
		audio.PlayOneShot (boom);
		rigidbody2D.gravityScale = 0;
		rigidbody2D.velocity = new Vector2(0,0);
		spriteRender.sprite = explode;
		collider2D.enabled = true;
		exploded = true;
		Invoke ("Disappear", explosionTime);
	}

	void Disappear(){
		spriteRender.enabled = false;
		collider2D.enabled = false;
		enabled = false;
	}

	void setCollider() {
		if (!exploded) {
			collider2D.enabled = false;
		}
		else {
			collider2D.enabled = true;
		}
	}
}
