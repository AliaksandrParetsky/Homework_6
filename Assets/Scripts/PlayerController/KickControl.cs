using System.Collections.Generic;
using UnityEngine;

public class KickControl : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> punchAnimationsList = new List<AnimationClip>();

    private Animator characterAnimator;
    public Animator CharacterAnimator{ get{if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>(); return characterAnimator;}}

    private void Start()
    {
        for (int i = 0; i < punchAnimationsList.Count; i++)
        {
            if (punchAnimationsList[i] == null)
            {
                Debug.LogError($"Element number {i} in punchAnimationsList is NULL !");
                return;
            }
        }
    }

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
