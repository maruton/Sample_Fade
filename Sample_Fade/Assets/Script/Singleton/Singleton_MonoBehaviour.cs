/*!
 * 	@file			Singleton_MonoBehaviour.cs 
 * 	@brief			Singleton design pattern. 
 * 	@note			See: http://wiki.unity3d.com/index.php?title=Singleton
 *	@attention 		None
 */

using UnityEngine;

/*!
 * 	@brief		Singleton_MonoBehaviour クラス。 <br>
 * 				MonoBehaviour を継承した Singleton クラス。 				 <br>
 * 	@attention	MonoBehaviour を継承したクラスはstatic classにできない。 	 <br> 
 * 				シーン中で１つのインスタンスのみ生成する為に利用する。 		 <br> 
 * 	@note		自動的に DontDestroyOnLoad() 化される。 
 * 	@sa			None
 * 	@author		Maruton.
 */
public class Singleton_MonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour{
	private static T _instance;
	
	private static object _lock = new object();
	
	public static T Instance{
		get{
			if (applicationIsQuitting) {
				Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
				                 "' already destroyed on application quit." +
				                 " Won't create again - returning null.");
				return null;
			}
			
			lock(_lock){// https://msdn.microsoft.com/ja-jp/library/c5kehkcz.aspx
				if (_instance == null){
					_instance = (T) FindObjectOfType(typeof(T));
					
					if ( FindObjectsOfType(typeof(T)).Length > 1 ){
						Debug.LogError("[Singleton] Something went really wrong " +
						               " - there should never be more than 1 singleton!" +
						               " Reopening the scene might fix it.");
						return _instance;
					}
					
					if (_instance == null){
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) "+ typeof(T).ToString();
						
						DontDestroyOnLoad(singleton);
						/*
						Debug.Log("[Singleton] An instance of " + typeof(T) + 
						          " is needed in the scene, so '" + singleton +
						          "' was created with DontDestroyOnLoad.");
						          */
					} else {
						Debug.Log("[Singleton] Using instance already created: " +
						          _instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}
	}
	
	private static bool applicationIsQuitting = false;
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
 
	/*!	Destroy時に呼ばれる 
	 *	Destroy時に呼ばれる 
   	 * 	@note		 	None  
   	 * 	@attention		None  
   	 */
	public void OnDestroy () {
		applicationIsQuitting = true;
	}
}
