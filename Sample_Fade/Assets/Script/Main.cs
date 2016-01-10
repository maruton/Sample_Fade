using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	Animator animator;
	AnimatorStateInfo animinfo;
	float AnimatorSpeed;
	void Rez_Model(){
		const string Model_CandyRockStar = "CandyRockStar";//!< UnityChan Candy Rock Star.
		GameObject prefab = (GameObject)Resources.Load ("Prefabs/CandyRockStar");
		GameObject go = (GameObject)Instantiate (prefab, Vector3.zero, Quaternion.identity);
		animator = go.GetComponent<Animator>();
		AnimatorSpeed = animator.speed;
	}
	void Animation_PauseOn(){
		animator.speed = 0f; // PAUSE animation on begin.
	}
	void Animation_PauseOff(){
		animator.speed = AnimatorSpeed;// Pause to Play.
	}
	void Animation_Replay(){
		animinfo = animator.GetCurrentAnimatorStateInfo(0);//Layer 0
		animator.Play(animinfo.nameHash, 0, 0.0f);//name, layer#, time
		if(animator.speed==0f) animator.speed = AnimatorSpeed;// Pause to Play.
	}
	void Animation_Standby(){
		animinfo = animator.GetCurrentAnimatorStateInfo(0);//Layer 0
		animator.Play(animinfo.nameHash, 0, 0.0f);//name, layer#, time
		Animation_PauseOn();
	}

	//Fade_uGUI fade;
	void Start () {
		Rez_Model();
		mode = eMode.Fadein;
	}
	enum eMode{
		Halt,Fadein,Playing,Fadeout
	};
	eMode mode = eMode.Halt;
	eMode before_mode = eMode.Halt;
	void Update () {
		if(mode!=before_mode){
			Debug.Log ("mode="+mode);
			switch(mode){
			case eMode.Halt:
				break;
			case eMode.Fadein:
				Animation_Standby();
				ActionFadeIn();//Fade in
				break;
			case eMode.Playing:
				Animation_PauseOff();
				break;
			case eMode.Fadeout:
				ActionFadeOut();//Fade out
				break;
			default:
				break;
			}
			before_mode = mode;
		}
		animinfo = animator.GetCurrentAnimatorStateInfo(0);//Layer 0
		if (animinfo.nameHash == Animator.StringToHash("Base Layer.003_NOT01_Final")){
		}
		if(mode==eMode.Playing){
			if(animinfo.normalizedTime >1.0f){
				mode = eMode.Fadeout;
			}
		}
	}

	void cb_FadeIn_Complete(){
		mode = eMode.Playing;
	}
	void cb_FadeOut_Complete(){
		mode = eMode.Fadein;
		Debug.Log ("Fadeout Complete.");
	}

	void ActionFadeIn(){
		Debug.Log("ActionFadeIn:");
		Fade_uGUI.Instance.SetFadeIn(3f);
		Fade_uGUI.Instance.SetCallback(cb_FadeIn_Complete);//fade.Event_cb_FadeComplete = cb_FadeComplete;
	}
	void ActionFadeOut(){
		Debug.Log("ActionFadeOut:");
		Fade_uGUI.Instance.SetFadeOut(3f);
		Fade_uGUI.Instance.SetCallback(cb_FadeOut_Complete);//fade.Event_cb_FadeComplete = cb_FadeComplete;
	}

}
