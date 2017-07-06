using UnityEngine;
using System.Collections;

public class SoundFXController : MonoBehaviour {

	public AudioClip pickup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void playPickup(){
		audio.PlayOneShot (pickup);
	}
}
