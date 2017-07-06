using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Responsible for changes between Start Screen, Game Screen, and Score screen
 * Pause, as well (assuming we want that)
 * Control changes between audio tracks (and sound effects)
 * Keeping Score
 * High-Bar: Upgrade Store
 *
 **/
public class GameController : MonoBehaviour {

	public enum STATES {SPLASH, GAME, PAUSE, LOSE};
	public STATES currState;

	private GameObject player;

	//Increments as the player falls. Used for final score and to determine difficulty of levels to generate
	public float score = 0; 
	public int lives = 3;
	
	public float normalSpeed = 3f;
	public float umbrellaSpeed = 1.5f;
	public float dropSpeed = 6f;

	private float gameSpeed;

	public GameObject background;
	public GameObject obstacles;

	public GameObject splashBackground;
	public GameObject splashObstacles;

	public GameObject cloudPrefab;
	public GameObject splashClouds;
	public GameObject splashScraper;

	public GameObject liveUpText;
	private GUIText liveUpGuiText;
	public GameObject liveUpBear;
	private SpriteRenderer liveUpBearRenderer;

	public GameObject[] levels;

	public float lifeScore;
	private int oldScoreVal;

	void Start () {
		
		player = GameObject.Find ("Avatar");
		liveUpGuiText = liveUpText.GetComponent<GUIText>();
		liveUpBearRenderer = liveUpBear.GetComponent<SpriteRenderer>();
		initSplash();
	}
	void Update () {

		switch (currState) {
			case STATES.SPLASH:
				splash ();
				break;
			case STATES.GAME:
				game ();
				break;
			case STATES.PAUSE:
				pause ();
				break;
			case STATES.LOSE:
				lose ();
				break;
		}

	}

	/*
	 * Update methods by game state.
	 */

	void splash () {
		List<GameObject> splashBack = new List<GameObject>();
		List<GameObject> splashObs = new List<GameObject>();
		
		
		foreach (Transform kids in splashBackground.transform) { 
			splashBack.Add(kids.gameObject);
		}
		
		foreach (Transform kids in splashObstacles.transform) { 
			splashObs.Add(kids.gameObject);
		}
		
		for (int i = 0; i < splashBack.Count; i++) {
			splashBack[i].transform.position += new Vector3(transform.position.x, transform.position.y + (gameSpeed / 2)*Time.deltaTime,transform.position.z);
			if(splashBack[i].transform.position.y >= 10){
				Destroy(splashBack[i]);
				GameObject ret = Instantiate (splashClouds, new Vector3(0, splashBack[i].transform.position.y - 30, 0), Quaternion.Euler (Vector3.zero)) as GameObject;
				ret.transform.parent = splashBackground.transform;
			}
		}
		
		for (int i = 0; i < splashObs.Count; i++) {
			splashObs[i].transform.position += new Vector3(transform.position.x, transform.position.y + (gameSpeed)*Time.deltaTime,transform.position.z);
			if(splashObs[i].transform.position.y >= 10){
				Destroy(splashObs[i]);
				GameObject ret = Instantiate (splashScraper, new Vector3(0, splashObs[i].transform.position.y - 30, 0), Quaternion.Euler (Vector3.zero)) as GameObject;
				ret.transform.parent = splashObstacles.transform;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			initGame();
		}
	}

	void game () {

		List<GameObject> back = new List<GameObject>();
		List<GameObject> obs = new List<GameObject>();
		
		
		foreach (Transform kids in background.transform) { 
			back.Add(kids.gameObject);
		}

		foreach (Transform kids in obstacles.transform) { 
			obs.Add(kids.gameObject);
		}
		
		for (int i = 0; i < back.Count; i++) {
			back[i].transform.position += new Vector3(transform.position.x, transform.position.y + (gameSpeed / 2)*Time.deltaTime,transform.position.z);
			if(back[i].transform.position.y >= 22){
				Destroy(back[i]);
				GameObject ret = Instantiate (cloudPrefab, new Vector3(3, back[i].transform.position.y - 36, 0), Quaternion.Euler (Vector3.zero)) as GameObject;
				ret.transform.parent = background.transform;
			}
		}
		
		for (int i = 0; i < obs.Count; i++) {
			obs[i].transform.position += new Vector3(transform.position.x, transform.position.y + (gameSpeed)*Time.deltaTime,transform.position.z);
			if(obs[i].transform.position.y >= 22){
				Destroy(obs[i]);
				GameObject ret = Instantiate (levels[Random.Range(0,levels.Length)], new Vector3(3, obs[i].transform.position.y - 36, 0), Quaternion.Euler (Vector3.zero)) as GameObject;
				ret.transform.parent = obstacles.transform;
			}
		}

		score += gameSpeed * Time.deltaTime;
		checkForLives ();
		if(Input.GetKeyDown(KeyCode.Space)){
			initPause();
		}

		if(lives <= 0){
			initLose();
		}

	}

	void pause () {
		if(Input.GetKeyDown(KeyCode.Space)){
			currState = STATES.GAME;
			player.GetComponent<PlayerController> ().moveEnabled = true;
		}
	}

	void lose () {
		if(Input.GetKeyDown(KeyCode.Space)){
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void checkForLives(){
		if (Mathf.FloorToInt(score) % lifeScore == 0  && score != 0) {
			addLife(Mathf.FloorToInt(score)); //add events associated with that
		}
	}

	/*
	 * Initialize methods for each state. 
	 */

	void initSplash() {
		GameObject.Find ("SplashScreen").SetActive(true);

		currState = STATES.SPLASH;
		gameSpeed = normalSpeed;
		player.GetComponent<PlayerController> ().moveEnabled = false;
	}

	void initGame() {
		GameObject.Find ("SplashScreen").SetActive(false);

		currState = STATES.GAME;
		gameSpeed = normalSpeed;

		score = 0;
		lives = 3;
		player.GetComponent<PlayerController> ().moveEnabled = true;
	}

	void initPause() {
		currState = STATES.PAUSE;
		player.GetComponent<PlayerController> ().moveEnabled = false;
	}

	void initLose() {
		currState = STATES.LOSE;
		player.GetComponent<PlayerController> ().moveEnabled = false;
	}

	public void resetSpeed() {
		gameSpeed = normalSpeed;
	}

	public void lowSpeed() {
		gameSpeed = umbrellaSpeed;
	}
	
	public void highSpeed() {
		gameSpeed = dropSpeed;
	}
	public void addLife(int score) {
		if (score != oldScoreVal && lives < 6) {
				lives++;
				GameObject.Find ("SoundFX").SendMessage("playPickup");
				liveUpGuiText.enabled = true;
				liveUpBearRenderer.enabled = true;
				Invoke ("EndLifeUp", 2.0f);
		}
		oldScoreVal = score;
	}
	private void EndLifeUp(){
		liveUpGuiText.enabled = false;
		liveUpBearRenderer.enabled = false;
	}
}
