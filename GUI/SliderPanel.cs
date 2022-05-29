using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPanel : MonoBehaviour
{
    [SerializeReference] private GameObject panel;

    public void ShowPanel()
    {
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            bool isShowing = animator.GetBool("showing");
            animator.SetBool("showing", !isShowing);
            if (this.transform.parent.name == "Setup Panel")
            {
                GetComponentInChildren<TMPro.TextMeshProUGUI>().text = isShowing ? "<" : ">";
            }
            else
            {
                GetComponentInChildren<TMPro.TextMeshProUGUI>().text = isShowing ? ">" : "<";
            }
        }
    }

}
