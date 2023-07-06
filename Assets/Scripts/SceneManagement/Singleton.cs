using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class implements a generic singleton pattern for MonoBehaviour classes.
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance; //The static instance of the singleton class.
    public static T Instance { get { return instance; } } //The public property to access the instance.

    //This method is called when the script instance is being loaded.
    protected virtual void Awake()
    {
        //If the instance already exists and this game object is not the same as the instance, destroy this game object.
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Otherwise, set the instance to this script.
            instance = (T)this;
        }

        //If this game object has no parent, make it persistent across scenes.
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }
}
