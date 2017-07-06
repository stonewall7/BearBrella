using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	private GameController gameControl;
	private PlayerController playControl;

	public Texture2D noUmbrella;
	
	enum GUIS{TITLE,COOL,LIVES,SCORE,PAUSE,OVER}
	
	public GUIText[] texts;

	public float ratio = 1;

	void Start() {
		gameControl = GameObject.Find ("GameManager").GetComponent <GameController> ();
		playControl = GameObject.Find ("Avatar").GetComponent <PlayerController> ();
		
		texts[(int)GUIS.TITLE].text = "BearBrella\nPress Space to Start";
	}
	
	void OnGUI() {
		if(gameControl.currState != GameController.STATES.SPLASH){
			GUI.Box(new Rect(60 * ratio, 50 * ratio, 300 * ratio, 100 * ratio), "");

			if(playControl.umbrellaTime > 0){
				float width = playControl.umbrellaTime * 300 / playControl.maxUmbrella;
				GUI.Box(new Rect(60 * ratio, 50 * ratio, width * ratio, 100 * ratio), "");
			}
			
			if(playControl.cooldownTime > 0){
				GUI.Label(new Rect(75 * ratio, 175 * ratio, 275 * ratio, 275 * ratio), noUmbrella);
			}
		}
	}

	void FixedUpdate() {        

		ratio = (Screen.width / 16f) / 100;

		texts [(int)GUIS.TITLE].fontSize = texts [(int)GUIS.PAUSE].fontSize = texts [(int)GUIS.OVER].fontSize = Mathf.RoundToInt(100 * ratio);
		texts [(int)GUIS.COOL].fontSize = texts [(int)GUIS.LIVES].fontSize = texts [(int)GUIS.SCORE].fontSize = Mathf.RoundToInt(50 * ratio);

		texts [(int)GUIS.COOL].pixelOffset = new Vector2(20 * ratio, 80 * ratio);
		texts [(int)GUIS.LIVES].pixelOffset = new Vector2(20 * ratio, 0);
		texts [(int)GUIS.SCORE].pixelOffset = new Vector2(20 * ratio, -80 * ratio);
		
		texts[(int)GUIS.COOL].text = "Cooldown: " + Mathf.CeilToInt(playControl.cooldownTime);
		texts[(int)GUIS.LIVES].text = "Lives: " + gameControl.lives;
		texts[(int)GUIS.SCORE].text = "Score: " + (int) gameControl.score;
		
		
		texts[(int)GUIS.TITLE].enabled = (gameControl.currState == GameController.STATES.SPLASH);
		
		texts[(int)GUIS.COOL].enabled = (gameControl.currState != GameController.STATES.SPLASH && playControl.cooldownTime > 0);
		texts[(int)GUIS.LIVES].enabled = (gameControl.currState != GameController.STATES.SPLASH);
		texts[(int)GUIS.SCORE].enabled = (gameControl.currState != GameController.STATES.SPLASH);
		
		texts[(int)GUIS.PAUSE].enabled = (gameControl.currState == GameController.STATES.PAUSE);
		texts[(int)GUIS.OVER].enabled = (gameControl.currState == GameController.STATES.LOSE);
	}
}