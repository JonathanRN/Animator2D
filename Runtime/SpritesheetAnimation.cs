using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Jroynoel.Animator2D
{
	[Serializable]
	public class SpritesheetAnimation
	{
		[field: SerializeField]
		public string Name;

		[field: SerializeField, Range(1, 30)]
		public int RowCount { get; private set; } = 1;

		[field: SerializeField, Range(1, 200)]
		public int FrameRate { get; private set; } = 1;

		[field: SerializeField]
		public bool Loop { get; private set; } = true;

		[SerializeField]
		private Sprite sprite;

#if UNITY_EDITOR
		public AnimationClip[] GenerateAnimationClips()
		{
			try
			{
				string spritePath = AssetDatabase.GetAssetPath(sprite);
				Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath).OfType<Sprite>().ToArray();
			
				AnimationClip[] clips = new AnimationClip[RowCount];

				for (int i = 0; i < RowCount; i++)
				{
					clips[i] = new AnimationClip
					{
						name = $"{Name}_{i}",
						frameRate = FrameRate
					};
					AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clips[i]);
					settings.loopTime = Loop;
					AnimationUtility.SetAnimationClipSettings(clips[i], settings);

					EditorCurveBinding curve = new EditorCurveBinding
					{
						type = typeof(SpriteRenderer),
						path = "",
						propertyName = "m_Sprite"
					};

					int spriteCount = sprites.Length / RowCount;
					ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[spriteCount];
					for (int j = 0; j < spriteCount; j++)
					{
						int spriteIndex = (j % spriteCount) + (spriteCount * i);
						keyframes[j] = new ObjectReferenceKeyframe
						{
							time = (float)j / FrameRate,
							value = sprites[spriteIndex]
						};
					}

					AnimationUtility.SetObjectReferenceCurve(clips[i], curve, keyframes);
				}

				return clips;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return null;
			}
		}
#endif
	}
}
