using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quadcopter : MonoBehaviour {
    public Rigidbody[] rigidbodies;//компоненты физики
    public QuadRotor[] quadRotors;//классы роторов
    void Start () {
        for (int i = 0; i < 4; i++)
        {
            quadRotors[i].motor = rigidbodies[i];
            quadRotors[i].quadcopter = this;
            quadRotors[i].transformQuad = this.transform;
        }
       
	}
}