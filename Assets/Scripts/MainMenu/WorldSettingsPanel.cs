using UnityEngine;

public class WorldSettingsPanel : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetBool("GoIn", true);
        animator.SetBool("GoOut", false);
    }

    public void Close()
    {
        animator.SetBool("GoIn", false);
        animator.SetBool("GoOut", true);
    }


}
