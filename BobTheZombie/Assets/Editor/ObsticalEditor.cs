using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor( typeof (ObsticleGenerator))]
public class ObsticalEditor : Editor {

	public override void OnInspectorGUI () {

		base.OnInspectorGUI ();

		ObsticleGenerator gen = target as ObsticleGenerator;

		gen.GenerateObsticles ();
	}
}
