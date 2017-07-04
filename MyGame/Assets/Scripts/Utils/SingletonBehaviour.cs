using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
	public static T Current;
	public SingletonBehaviour() {			
		Current = (T)this;
	}
}

