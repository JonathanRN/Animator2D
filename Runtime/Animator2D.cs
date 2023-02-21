using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Jroynoel.Animator2D
{
	[RequireComponent(typeof(Animator))]
	public class Animator2D : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		private RuntimeAnimatorController controller;

		[SerializeField]
		private SpritesheetAnimation[] spritesheetAnimations;

		private Animator animator;

#if UNITY_EDITOR
		[SerializeField]
		private string folder, entityName;
#endif

		private void Awake()
		{
			animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = controller;
		}

		private void OnValidate()
		{
			if (animator == null)
			{
				animator = GetComponent<Animator>();
			}
			animator.hideFlags = HideFlags.HideInInspector;
		}

		public void Play(object stateName)
		{
			if (animator != null)
			{
				animator.Play(stateName.ToString());
			}
		}

		public string[] GetAnimationsByName(string name)
		{
			return controller.animationClips.Where(x => x.name.Contains($"_{name}_")).Select(x => x.name).ToArray();
		}

#if UNITY_EDITOR
		public void CreateAssets()
		{
			string projectRelativePath = $"Assets/{folder}/{entityName}";
			if (!Directory.Exists(projectRelativePath))
			{
				Directory.CreateDirectory(projectRelativePath);
				Debug.Log($"[{nameof(Animator2D)}] Directory \"{projectRelativePath}\" created.");
			}

			string controllerPath = $"{projectRelativePath}/{entityName}.controller";
			controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
			Debug.Log($"[{nameof(Animator2D)}] File \"{controllerPath}\" created.");

			foreach (var spritesheet in spritesheetAnimations)
			{
				AnimationClip[] clips = spritesheet.GenerateAnimationClips();
				for (int i = 0; i < clips.Length; i++)
				{
					string clipPath = $"{projectRelativePath}/{entityName}_{clips[i].name}.anim";
					AssetDatabase.CreateAsset(clips[i], clipPath);
					Debug.Log($"[{nameof(Animator2D)}] File \"{clipPath}\" created.");
					(controller as AnimatorController).AddMotion(clips[i]);
				}
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
	}
}