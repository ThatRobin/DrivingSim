using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRenderer : MonoBehaviour {

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

}
