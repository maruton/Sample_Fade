using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Fade_uGUI : Singleton_MonoBehaviour<Fade_uGUI>{
	GameObject go_Image_Fade; 	//!< uGUI for Fade object.
	Image Image_Fade;			//!< uGUI Image.

	float fadeAlpha;
	bool fadeDir = false;	//!< false:FadeIn  true:FadeOut
	bool fading = false;	//!< false:Inacrive  true:Active
	float fadeTime = 1.0f;	//!< Fading time.
	float fadeDeltaTime;

	/*!	fadeDir のアクセサ 
	 *	現在/最後に行ったFadeの方向を返す。   
	 *	@return			false:FadeIn  true:FadeOut
   	 * 	@note		 	None 
   	 * 	@attention		None 
   	 */
	public bool FadeDir{
		get{ return(fadeDir); }
	}
	/*!	fading のアクセサ 
	 *	現在Fade out/in 動作中か否かを返す。 
	 *	@return			false:no work  true:Fading In/Out
   	 * 	@note		 	None 
   	 * 	@attention		None 
   	 */
	public bool Fading{
		get{ return(fading); }
	}

	public delegate void Delegate_cb_FadeComplete();		//!< Type of callback function. 
	public Delegate_cb_FadeComplete Event_cb_FadeComplete;	//!< callback function.

	public void SetCallback(Delegate_cb_FadeComplete func){
		Event_cb_FadeComplete = func;
	}

	public void SetFadeIn(float t){
		fadeTime = t;
		SetAlpha(Image_Fade, 1f);
		fadeDeltaTime = 0f;
		fadeDir = false;//FadeIn
		fading = true;
	}
	public void SetFadeOut(float t){
		fadeTime = t;
		SetAlpha(Image_Fade, 0f);
		fadeDeltaTime = 0f;
		fadeDir = true;//FadeOut
		fading = true;
	}
	public void SetTime(float t){
		fadeTime = t;
	}

	void Init(){
		go_Image_Fade = GameObject.Find("Canvas/Image_Fade");
		Image_Fade = go_Image_Fade.GetComponent<Image>();
	}
	void Awake (){
		Init();
	}
	void Start(){
	}

	void Update (){
		if(fading){
			fadeDeltaTime += Time.deltaTime;
			fadeAlpha = fadeDeltaTime / fadeTime;
			if(fadeAlpha>1f){
				fading = false;
				if(fadeDir){//false:FadeIn  true:FadeOut
					SetAlpha(Image_Fade, 1.0f);
				}
				else{
					SetAlpha(Image_Fade, 0.0f);
				}
				if(Event_cb_FadeComplete!=null) Event_cb_FadeComplete();
			}
			else{
				if(fadeDir){//false:FadeIn  true:FadeOut
					SetAlpha(Image_Fade, fadeAlpha);
				}
				else{
					SetAlpha(Image_Fade, 1.0f - fadeAlpha);
				}
			}
		}
	}

	void SetAlpha(Image img, float alpha){
		Color c = img.color;
		c.a = alpha;
		img.color = c;
	}
}
