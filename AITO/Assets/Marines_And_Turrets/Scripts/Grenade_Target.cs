using UnityEngine;
using System.Collections;
//The grenade calls the turret according to the grenade name: "Grenade_Target1 calls Gun_Turret", "Grenade_Target2 calls Blade..."
//The grenade name is asigned by soldier in "Example_Motion_Controller.cs" script associated
public class Grenade_Target : MonoBehaviour {
	public GameObject Turret1_Prefab;
	public GameObject Turret2_Prefab;
	public GameObject Turret3_Prefab;
	private Vector3 stageDimensions;//It is used for instantiate the turret in top of screen
	// Use this for initialization
	void Start () {
		Camera_Dimensions ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Invoke("Instantiate", 2);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void Instantiate(){
		if (GameObject.Find ("Grenade_Target1")) {
			GameObject Turret = Instantiate(Turret1_Prefab, new Vector3(this.gameObject.transform.position.x,stageDimensions.y,this.gameObject.transform.position.z), Quaternion.identity)as GameObject;
			Turret.name="Gun_Turret";
		}
		if (GameObject.Find ("Grenade_Target2")) {
			GameObject Turret = Instantiate(Turret2_Prefab, new Vector3(this.gameObject.transform.position.x,stageDimensions.y,this.gameObject.transform.position.z), Quaternion.identity)as GameObject;
			Turret.name="Blade_Turret";
		}
		if (GameObject.Find ("Grenade_Target3")) {
			GameObject Turret = Instantiate(Turret3_Prefab, new Vector3(this.gameObject.transform.position.x,stageDimensions.y,this.gameObject.transform.position.z), Quaternion.identity)as GameObject;
			Turret.name="Shield_Turret";
		}
		Destroy (this.gameObject);
	}

	void Camera_Dimensions(){
		stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
	}
}
