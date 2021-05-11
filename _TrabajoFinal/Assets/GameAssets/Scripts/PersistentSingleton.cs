using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }
                //           else
                //           {
                //Debug.LogError(_instance.name);
                //}
            }
            //Debug.LogError("returning instance");
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            //Debug.LogError("Already present: " + _instance.gameObject.name);
            //Debug.LogError("Is destroyed and self the same? " + (_instance == this));

            if (_instance != this)
            {
                //Debug.LogError("Destroying " + gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}
