using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animator2D : MonoBehaviour
{
	[SerializeField]
	private RuntimeAnimatorController controller;

	private Animator animator;

	public RuntimeAnimatorController Controller => controller;

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
}
