using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to a scene GameObject.
/// </summary>
public class SceneData : MonoBehaviour {
    
    // This scene's element group. 
    public GameObject currentChildrenGroup;

    // The background image of this scene.
    public Cubemap backgroundTexture;

    
}

/// <summary>
/// Class reserved for future use. It will help save and load scene data from database.
/// </summary>
public class SceneDataHelper
{
    public Vector3 backgroundRotation;

    public string backgroundImagePath;

}
