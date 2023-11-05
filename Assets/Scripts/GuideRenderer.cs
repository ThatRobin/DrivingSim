using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRenderer : MonoBehaviour
{
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);


    }

    public static List<string> STUDENTS = new List<string>();

    public static void Main(string[] args) {
        // list "STUDENTS" is empty.
        
        STUDENTS.Add("Robin"); // Robin is added at the zero index    | Robin  -> 0
        STUDENTS.Add("Damien"); // Damien is added at the one index   | Damien -> 1
        STUDENTS.Add("Jason"); // Jason is added at the two index     | Jason  -> 2
        STUDENTS.Add("Mark"); // Mark is added at the three index     | Mark   -> 3

        // adding new students will always give them a unique index.

        STUDENTS.IndexOf("Robin"); // this will return zero as an integer.
        STUDENTS.IndexOf("Damien"); // this will return one as an integer.
        STUDENTS.IndexOf("Jason"); // this will return two as an integer.
        STUDENTS.IndexOf("Mark"); // this will return three as an integer.

        string sEightDigit = STUDENTS.IndexOf("Robin").ToString("000000");
        // this will create a string version of the index with six digits
    }
    

}
