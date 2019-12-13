using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MazeGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MazeGenerator mazeGen = (MazeGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mazeGen.autoUpdate) {
				mazeGen.generateMaze();
			}
		}

		if (GUILayout.Button ("Generate")) {
			mazeGen.generateMaze();
		}

		if (GUILayout.Button ("Delete")) {
			mazeGen.deleteMaze();
		}
	}
}