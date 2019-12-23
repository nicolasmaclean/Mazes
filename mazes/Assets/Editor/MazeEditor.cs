using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MazeGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MazeGenerator mazeGen = (MazeGenerator)target;
		
		DrawDefaultInspector();

		if (GUILayout.Button ("Generate")) {
			mazeGen.generateMaze(false);
		}

		if (GUILayout.Button ("Solve")) {
			mazeGen.solveMaze();
		}

		if (GUILayout.Button ("Delete")) {
			mazeGen.deleteMaze();
		}

		if (GUILayout.Button ("Generate Braided")) {
			mazeGen.generateMaze(true);
		}
	}
}