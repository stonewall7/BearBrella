using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

	// Use this for initialization
	public GameObject ret1;
	public GameObject ret2;
	public GameObject ret3;

	private SpriteRenderer ret1spriter;
	private SpriteRenderer ret2spriter;
	private SpriteRenderer ret3spriter;


	public bool ret1out = false;
	public bool ret2out = false;
	public bool ret3out = false;

	void Start () {
		ret1spriter = ret1.GetComponent<SpriteRenderer> ();
		ret2spriter = ret2.GetComponent<SpriteRenderer> ();
		ret3spriter = ret3.GetComponent<SpriteRenderer> ();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetRet(Vector2 location){
		if (!ret1out)
		{
			ret1.transform.position = location;
			ret1spriter.enabled = true;
			ret1out = true;
			return;
		}
		else if (!ret2out)
		{
			ret2.transform.position = location;
			ret2spriter.enabled = true;
			ret2out = true;
			return;
		}
		else if (!ret3out)
		{
			ret3.transform.position = location;
			ret3spriter.enabled = true;
			ret3out = true;
			return;
		}

	}

	void FreeRet(){

		ret1out = false; ret1spriter.enabled = false;
		ret2out = false; ret2spriter.enabled = false; 
		ret3out = false; ret3spriter.enabled = false;


	}
}
