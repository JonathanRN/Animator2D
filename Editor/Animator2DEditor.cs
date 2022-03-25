using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Animator2D))]
public class Animator2DEditor : Editor
{
    private static string path = "Assets/Scripts/Enums";

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.Space(10);
		if (GUILayout.Button("Generate Enum"))
		{
			Animator2D animator = (Animator2D)target;
			Generate(animator.Controller);
		}
	}

	private void Generate(RuntimeAnimatorController animatorController)
	{
		if (!Directory.Exists(path))
		{
            Directory.CreateDirectory(path);
		}

        string filename = animatorController.name + "Enum.cs";
        string fullpath = Path.Combine(path, filename);

        if (File.Exists(fullpath))
		{
            File.Delete(fullpath);
		}

		var stream = File.CreateText(fullpath);
        string[] states = animatorController.animationClips.Select(x => x.name).ToArray();
		stream.WriteLine($"public enum {Path.GetFileNameWithoutExtension(fullpath)}");
        stream.WriteLine("{");
		foreach (var state in states)
		{
            stream.WriteLine($"\t{state},");
		}
        stream.WriteLine("}");
		stream.Close();

		AssetDatabase.Refresh();
		Debug.Log($"Generated {fullpath}");
		Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(fullpath);
	}
}
