using UnityEngine;
using System.Collections;
//Example Script for motion (Walk, jump and dying), for dying press 'k'...
//Grenade (Mouse Right button) is used to call the selected turret
//1,2 or 3 for select the turret
public class Example_Motion_Controller : MonoBehaviour {
	//--Character movement
	private float maxspeed; //walk speed
	Animator anim;
	private bool faceright; //face side of sprite activated
	private bool jumping=false;
	private bool isdead=false;
	//-- 
	//--Grenade_Target and Bullet
	public Transform spawner;//Gun point for instantiate bullets
	public GameObject Grenade_Target_Prefab;//Grenade_Target Prefab used for call a turret
	public GameObject Bullet_Prefab;
	public int Bullet_speed = 15;
	private bool shooting_grenade;//Use for instantiate grenade after animation finish
	//--
	//--"Walking and shooting"
	private int Shoot_time = 0; //Counter for "walking and shooting" animation visible time
	//--
	//--OnGUI Interface--
	private Color b1_color;//Button backgrounds
	private Color b2_color;
	private Color b3_color;
	public Texture Turret1_Texture;//Button texture turret1
	public Texture Turret2_Texture;//Button texture turret2
	public Texture Turret3_Texture;//Shield texture
	private int Turret_selection;// 1, 2 or 3
	void OnGUI() {
		if ((!Turret1_Texture)||(!Turret2_Texture)||(!Turret3_Texture)) {
			Debug.LogError("Please assign a texture on the inspector");
			return;
		}

		GUI.backgroundColor = b1_color;
		if (GUI.Button (new Rect (10, 20, 50, 50), Turret1_Texture)) {
				Turret_selection = 1;
				b1_color=Color.yellow;
				b2_color=Color.black;
				b3_color=Color.black;
			}
		if (GUI.Button (new Rect (10, 0, 50, 20), "Gun")) {}
		GUI.backgroundColor = b2_color;
		if (GUI.Button (new Rect (70, 20, 50, 50), Turret2_Texture)) {
				Turret_selection = 2;
				b1_color=Color.black;
				b2_color=Color.yellow;
				b3_color=Color.black;
		}
		if (GUI.Button (new Rect (70, 0, 50, 20), "Blade")) {}
		GUI.backgroundColor = b3_color;
		if (GUI.Button (new Rect (130, 20, 50, 50), Turret3_Texture)) {
				Turret_selection = 3;
				b1_color=Color.black;
				b2_color=Color.black;
				b3_color=Color.yellow;
		}
		if (GUI.Button (new Rect (130, 0, 50, 20), "Shield")) {}
	}

	//---End OnGUI--

	void Start () {
		this.gameObject.name = "Soldier";
		Turret_selection = 1;//Default selection is Gun Turret (1)
		Interface_Load ();//--Buttons OnGUI Background Preset
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
		maxspeed=2f;//Set walk speed
		faceright=true;//Default right side
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("walk", false);//Walking animation is deactivated
		anim.SetBool ("dead", false);//Dying animation is deactivated
		anim.SetBool ("jump", false);//Jumping animation is deactivated
		anim.SetBool ("attack", false);//Shooting animation is deactivated
		shooting_grenade=false;
	}

	void OnCollisionEnter2D(Collision2D coll) {
			jumping=false;
			anim.SetBool ("jump", false);
	}
	
	void Update () {
		Is_Shooting_Grenade ();
		Animation_Layer_Controller ();
				
		if(isdead==false){
			//--Turret Selection
			if(Input.GetKey ("1")){//Turret 1 is selected
				Turret_selection = 1;
				Interface_Load();
			}
			if(Input.GetKey ("2")){//Turret 2 is selected
				Turret_selection = 2;
				Interface_Load();
			}
			if(Input.GetKey ("3")){//Turret 3 is selected
				Turret_selection = 3;
				Interface_Load();
			}
			//--
			if (Input.GetMouseButtonDown(0)){
				anim.SetBool ("attack", true);
				anim.SetLayerWeight(1, 1f);
				Shoot_time = 0;
				Instantiate_Bullet();
			}
			if (Input.GetMouseButtonDown(1)){//Only call the turret when is in stop mode
				if (GameObject.Find("Grenade_Target") == null)
				{
					if (anim.GetCurrentAnimatorStateInfo (1).IsName ("Stop")) {
						anim.SetBool ("grenade", true);
					}
				}
			}
			//--DYING
			if(Input.GetKey ("k")){//###########Change the dead event, for example: life bar=0
				anim.SetBool ("dead", true);
				isdead=true;
			}
			//--END DYING
			
			//--JUMPING
			if (Input.GetButtonDown("Jump")){
				if(jumping==false){//only once time each jump
					GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,200));
					jumping=true;
					anim.SetBool ("jump", true);
				}
			}
			//--END JUMPING
			
			//--WALKING
			float move = Input.GetAxis ("Horizontal");
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxspeed, GetComponent<Rigidbody2D>().velocity.y);
			if(move>0){//Go right
				anim.SetBool ("walk", true);//Walking animation is activated
				//anim.Play("Walking", -1, 0f);
				if(faceright==false){
					Flip ();
				}
			}
			if(move==0){//Stop
				anim.SetBool ("walk", false);
			}			
			if((move<0)){//Go left
				anim.SetBool ("walk", true);
				//anim.Play("Walking", -1, 0f);
				if(faceright==true){
					Flip ();
				}
			}
			//END WALKING
		}
	}
	void Instantiate_Bullet(){
			GameObject Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner.position.x,spawner.position.y,spawner.position.z), Quaternion.identity)as GameObject;
			Bullet.name="Bullet";
			if(faceright==true){
				Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_speed, 0f);
			}else{
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed, 0f);
			}
		}
	void Instantiate_Grenade(){//Instantiate a Grenade_Target for call a turret
			GameObject Grenade_Target = Instantiate(Grenade_Target_Prefab, new Vector3(spawner.position.x,spawner.position.y,spawner.position.z), Quaternion.identity)as GameObject;
			Grenade_Target.name="Grenade_Target" + Turret_selection;//Setting the name for call a turret
			if(faceright==true){
				Grenade_Target.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 1f);
			}else{
				Grenade_Target.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 1f);
			}
		}
	void Flip(){
		faceright=!faceright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void Is_Shooting_Grenade(){//First "Shooting grenade animation", when finished then instantiate and launch the grenade
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Shooting_Grenade")) {
			shooting_grenade=true;//Shooting_Grenade animation is up
		} else {
			anim.SetBool ("grenade", false);
			if(shooting_grenade==true){//When "Shooting_Grenade animation is finished" then launch the grenade
				Instantiate_Grenade();
				shooting_grenade=false;
			}
		}
	}
	void Animation_Layer_Controller(){//Visible time of the animations in different layer
		int aux_time = 5;
		if (anim.GetCurrentAnimatorStateInfo (1).IsName ("Shooting")) {//If stop+shooting time=15, walk+shooting time=5
			aux_time=15;
		}
		if (Shoot_time < aux_time) {
			Shoot_time ++;		
		} else {
			anim.SetLayerWeight (1, 0f);
			anim.SetBool ("attack", false);
		}
	}
	void Interface_Load(){
		switch (Turret_selection) {
		case 1:
			b1_color = Color.yellow;
			b2_color = Color.black;
			b3_color = Color.black;
			break;
		case 2:
			b1_color = Color.black;
			b2_color = Color.yellow;
			b3_color = Color.black;
			break;
		case 3:
			b1_color = Color.black;
			b2_color = Color.black;
			b3_color = Color.yellow;
			break;
		}
	}
}
