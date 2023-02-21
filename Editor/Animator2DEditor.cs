using UnityEditor;
using UnityEngine;

namespace Jroynoel.Animator2D.Editor
{
	[CustomEditor(typeof(Animator2D))]
	public class Animator2DEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Generate Animations"))
			{
				(target as Animator2D).CreateAssets();
			}
		}
	}
}