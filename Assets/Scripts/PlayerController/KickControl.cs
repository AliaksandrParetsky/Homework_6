using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KickControl : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> punchAnimationsList = new List<AnimationClip>();

    private Animator characterAnimator;
    public Animator CharacterAnimator{ get{if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>(); return characterAnimator;}}

    public void Kick()
    {
        CharacterAnimator.SetTrigger("Kick");
        SetAnimationClip();
    }

    private int GetRandomClip()
    {
        int animClip = Random.Range(0, punchAnimationsList.Count);

        return animClip;
    }

    private void SetAnimationClip()
    {
        CharacterAnimator.SetInteger("KickID", GetRandomClip());
    }
}
