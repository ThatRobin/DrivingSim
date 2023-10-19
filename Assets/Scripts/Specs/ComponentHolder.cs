using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Component/Holder", fileName ="Holder")]
public class ComponentHolder : ScriptableObject {

    public string carName;
    public float maximumSpeed;
    public float acceleration;
    public Material material;

}
