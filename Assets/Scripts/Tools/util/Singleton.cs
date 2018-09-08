/********************************************************************
	created:	2017/07/19
	filename:	Singleton.cs
	author:		Benjamin
	purpose:	[managed singleton for MonoBehaviour and classes]
*********************************************************************/
using UnityEngine;
using System;


public class SingletonMB<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static object _lock = new object();

	protected Action DestroyEventHandler;	// when object is deleted called

	// Returns the instance of the singleton
	public static T instance
	{
		get
		{
			lock (_lock) {
				if (null == _instance) {
					_instance = (T)FindObjectOfType(typeof(T));
					if (null == _instance) {
						GameObject obj = new GameObject(typeof(T).ToString());
						_instance = obj.AddComponent<T>();
						if (null == _instance)
							Debug.LogError("FATAL! Cannot create an instance of " + typeof(T) + ".");
						else {
							DontDestroyOnLoad(obj);
						}
					}
					else {
						Debug.Log("Aleady Instance of " + typeof(T) + " exists in the scene.");
					}
				}
			}
			return _instance;
		}
	}
	protected virtual void Awake()	{
		Debug.Log("Awake singleton : " + typeof(T));
	}

	public virtual void Initial() {

	}

	public static bool IsInstanced { get { return  null != _instance; } }
	public static void SelfDestroy()
	{
        if (null != _instance) {
			DestroyImmediate( _instance.gameObject );
			//_instance = null;
		}
	}

	void OnApplicationQuit()
	{
		_instance = null;
		_lock = null;
	}

	void OnDestroy()
	{
        if (this != _instance)
            return;
		_instance = null;

		if (DestroyEventHandler != null)
			DestroyEventHandler();

		//Debug.Log("Singleton object destroy");
	}
}


public abstract class ManualSingleton<T> where T : class, new()
{
	static protected T _instance;
	static public T instance
	{
		get { return _instance; }
		set
		{
			if (_instance != null)
				throw new System.ApplicationException("cannot set Instance twice!");

			_instance = value;
		}
	}
    public ManualSingleton() {
        if (_instance != null)
            throw new System.ApplicationException("cannot set Instance twice!");
    }

	static public T Instantiate()
	{
		instance = new T();
		return instance;
	}

	static public T SetInstance(T ins)
	{
		instance = ins;
		return ins;
	}

	static public void Destroy()
	{
		_instance = null;
	}



}


public abstract class Singleton<T> : ManualSingleton<T> where T : class, new()
{
	static new public T instance {
		get {
			if (_instance == null)
				Instantiate();
			return _instance;
		}
		set {
			if (_instance != null)
				throw new System.ApplicationException("cannot set Instance twice!");

			_instance = value;
		}
	}

    public Singleton() {
        if (_instance != null)
            throw new System.ApplicationException("cannot set Instance twice!");
    }
}


public abstract class ManualSingletonMB<T> : UnityEngine.MonoBehaviour where T : ManualSingletonMB<T>
{
	static private T _instance;
	static public T instance
	{
		get { return _instance; }
		set
		{
			if (_instance != null)
				throw new System.ApplicationException("cannot set Instance twice!");

			_instance = value;
		}
	}

	protected Action DestroyEventHandler;

	protected virtual void Awake()	{
		Debug.Log("Awake Manual singleton : " + typeof(T));
	}
	void OnDestroy()
	{
		if (_instance != this)
			return;

		if (DestroyEventHandler != null)
			DestroyEventHandler();
		_instance = null;
	}
}
