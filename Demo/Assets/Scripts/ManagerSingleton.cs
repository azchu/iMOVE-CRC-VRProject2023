using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Generic singleton pattern for all managers to inherit.
/// </summary>
/// <typeparam name="T">Define the type of the singleton class. Just use the class name.</typeparam>
public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    /// <summary>
    /// Return the only instance of this class, or create it if no instance now.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    _instance = singleton.AddComponent<T>();
                }
            }
            return _instance;
        }
    }


    /// <summary>
    /// Check if there is an instance existing in the awake stage. 
    /// If there is already one instance in this scene, destroy this one (the entire gameObject). If null, make this one persist. 
    /// </summary>
    public virtual void Awake()
    {

        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this as T;
    }
}

