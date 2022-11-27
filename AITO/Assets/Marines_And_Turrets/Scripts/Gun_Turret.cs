using UnityEngine;
using System.Collections;
//The collisions are deactivated when the turret is in "deactivated animation"
public class Gun_Turret : MonoBehaviour {
	Animator anim;
	public Transform spawner;//Gun point for instantiate bullets
	public GameObject Bullet_Prefab;
	public int Bullet_speed = 10;
	public float Activation__time = 0f;//Delay Time after colision with ground to activate the turret
	public int Shoot_Delay = 35;//Time between shots 
	public int Duration = 500;//Turret durantion

	private int Count = 0;//Aux Count delay between shots
	private bool is_activated;//Activation animation is finished
	private bool faceright;
	//About progress-bar --
	private float progress_bar;//Relative width
	private float normalize_constant;//Initial concordance between actual progress bar size and duration.
	private float initial_progress_bar_size;//Initial Progress bar width

	//private Vector3 characterPos;
	private float screen_h;
	private int Turret_Height;
	private int Turret_Width;

	//--OnGUI Interface--
	void OnGUI() {
		if (((int)progress_bar > 0)&&(is_activated==true)) {
			Render_Colored_Rectangle (Get_Turret_position("x") - (int)(Get_Turret_size("x")/2), Screen.height - Get_Turret_position("y") - Get_Turret_size("y") + (int)(Get_Turret_size("y")/10)*3, (int)initial_progress_bar_size, (int)(Get_Turret_size("y")/20), 0, 0, 0);//Black Progress Bar
			Render_Colored_Rectangle (Get_Turret_position("x") - (int)(Get_Turret_size("x")/2), Screen.height - Get_Turret_position("y") - Get_Turret_size("y") + (int)(Get_Turret_size("y")/10)*3, (int)progress_bar, (int)(Get_Turret_size("y")/20), 255, 255, 255);//White Progress Bar
		}
	}
	//--End OnGUI

	// Use this for initialization
	void Start () {
		//--
		faceright = Player_Faceright ();//Main Player position
		Init_();//Progress Bar Initialization
		is_activated = false;//Activation animation is off because not collision with ground
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("shoot", false);
		anim.SetBool ("on_ground", false);//Not collision with ground
		anim.SetBool ("deactivated", false);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (is_activated == false) {
			Invoke ("Activation", Activation__time);
			Destroy (GameObject.Find ("Grenade_Target1"));
		}
	}

	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate(){
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Destroy")) {
			Destroy (this.gameObject);
		}
		if ((anim.GetCurrentAnimatorStateInfo (0).IsName ("Explotion"))&&(is_activated==true)) {
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Activated")) {
			is_activated=true;
		}
		if (is_activated == true) {//The activation animation is finished.
			if(Duration>0){
				LaunchProjectile();
				Normalize_ ();
				Duration--;
			}else{
				Disable_Collision();
				anim.SetBool ("deactivated", true);
			}
		}
	}

	void Normalize_(){//Relative width size of the progress bar
		progress_bar = normalize_constant * Duration;	
	}

	void Get_Normalize_Constant(){
		initial_progress_bar_size = Get_Turret_size("x");
		normalize_constant = initial_progress_bar_size / Duration;
	}

	void Activation(){
		anim.SetBool ("on_ground", true);
	}

	void LaunchProjectile(){
		if (Count < Shoot_Delay) {
			Count++;
			anim.SetBool ("shoot", false);
		} else {
			anim.SetBool ("shoot", true);
			Instantiate_Bullet();
			Count = 0;
		}
	}

	void Render_Colored_Rectangle(int x, int y, int w, int h, float r, float g, float b)
	{
		Texture2D rgb_texture = new Texture2D(w, h);
		Color rgb_color = new Color(r, g, b);
		int i, j;
		for(i = 0;i<w;i++)
		{
			for(j = 0;j<h;j++)
			{
				rgb_texture.SetPixel(i, j, rgb_color);
			}
		}
		rgb_texture.Apply();
		GUIStyle generic_style = new GUIStyle();
		GUI.skin.box = generic_style;
		GUI.Box (new Rect (x,y,w,h), rgb_texture);
	}

	void Init_(){
		Get_Normalize_Constant();
		Normalize_ ();
	}

	//Turret relative position and size
	int Get_Turret_size(string var){//Return the width ("x") or height ("y") of this sprite relative to the screen
		int aux = 0;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Renderer mesh = gameObject.GetComponent<SpriteRenderer>();
		Vector3 posStart = cam.WorldToScreenPoint(new Vector3(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.min.z));
		Vector3 posEnd = cam.WorldToScreenPoint(new Vector3(mesh.bounds.max.x, mesh.bounds.max.y, mesh.bounds.min.z));
		if (var == "x") { aux = (int)(posEnd.x - posStart.x);}//Gameobject width
		if (var == "y") { aux = (int)(posEnd.y - posStart.y);}//Gameobject height

		return aux;
	}

	int Get_Turret_position(string var){//Get the turret position relative to the screen ("x") or ("y") 
		int aux = 0;
		Vector3 pos;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		pos = cam.WorldToScreenPoint(this.gameObject.transform.position);
		if (var == "x") {aux = (int)pos.x;}//Gameobject width
		if (var == "y") {aux = (int)pos.y;}//Gameobject height

		return aux;
	}
	//--

	void Instantiate_Bullet(){
		GameObject Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner.position.x,spawner.position.y,spawner.position.z), Quaternion.identity)as GameObject;
		Bullet.name="Bullet";
		if(faceright==true){
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_speed, 0f);
		}else{
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed, 0f);
		}
	}

	bool Player_Faceright(){
		bool aux_=true;
		float player_x = GameObject.Find("Soldier").transform.position.x;
		if (GameObject.Find ("Soldier").transform.position.x < this.gameObject.transform.position.x) {
			aux_ = true;
		} else {
			aux_ = false;
			Flip ();
		}
		return aux_;
	}

	void Flip(){
		faceright=!faceright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	//--Collision Control--
	void Disable_Collision(){
		if (this.GetComponent<Rigidbody2D>().isKinematic == false) {
			this.GetComponent<Rigidbody2D>().isKinematic = !this.GetComponent<Rigidbody2D>().isKinematic;
			this.gameObject.GetComponent<Collider2D>().enabled = !this.gameObject.GetComponent<Collider2D>().enabled;
		}
	}

	void Enable_Collision(){
		if (this.GetComponent<Rigidbody2D>().isKinematic == true) {
			this.GetComponent<Rigidbody2D>().isKinematic = !this.GetComponent<Rigidbody2D>().isKinematic;
			this.gameObject.GetComponent<Collider2D>().enabled = !this.gameObject.GetComponent<Collider2D>().enabled;
		}
	}
	//--
}
