using UnityEngine;
using System.Collections;

public class rocket_script : MonoBehaviour {

	public Sprite blast;
	public Sprite explode;
	private GameController controller;
	private GameObject player;
	public float bombSpeed = 200;
	public AudioClip tick;
	public AudioClip boom;
	public SpriteRenderer spriteRender;
	public int offset_before_shooting = 50;
	private bool shot = false;
	private float xpos;
	bool locked = false;
	bool exploded = false;
	bool prepped = false;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Avatar");
		controller = GameObject.Find ("GameManager").GetComponent<GameController> () as GameController;
		this.collider2D.enabled = false;
		rigidbody2D.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.transform.position.y < (transform.position.y + offset_before_shooting))) {
			prepForFire();
			//sendFlying();
		}
		if(controller.currState == GameController.STATES.PAUSE){
			Time.timeScale = 0;
		}else{
			Time.timeScale = 1;
		}
	
	}
	void prepForFire()
	{
		if (!locked) {
						xpos = player.transform.position.x;
				} else {
						xpos = xpos;
				}
		Vector2 location = new Vector2(xpos, transform.position.y);
		if (!prepped)
		{
			GameObject.Find ("Reticles").SendMessage ("SetRet", location);
		}
		//prepped = true;
		Invoke ("Lock", 0.5f);

	}
	void sendFlying(){
			if (!shot) {
					audio.PlayOneShot (tick);
				}
			shot = true;
			if (!exploded)
			{
				rigidbody2D.AddForce (Vector3.left * bombSpeed);
			}
			spriteRender.sprite = blast;
			if (transform.position.x <= xpos + 0.6) {
				exploded = true;
				spriteRender.sprite = explode;
				rigidbody2D.velocity = new Vector2(0,0);
				this.collider2D.enabled = true;
		}

	}
	void Lock()
	{
		GameObject.Find ("Reticles").SendMessage ("FreeRet");
		locked = true;
		Invoke ("sendFlying", 0.2f);
		//Vector2 location = new Vector2(xpos, transform.position.y);
		//GameObject.Find ("Reticles").SendMessage ("SetRet", location);
	}
}
