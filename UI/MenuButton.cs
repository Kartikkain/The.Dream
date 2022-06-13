using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] public int ButtonIndex;
    MainMenu menu;
    Animator animator;

    private void OnEnable()
    {
        //menu = FindObjectOfType<MainMenu>();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //menu = FindObjectOfType<MainMenu>();
    }
    // Start is called before the first frame update
    void Start()
    {
        MainMenu m = GetComponentInParent<MainMenu>();
        menu = FindObjectOfType<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name + menu.index);
        Debug.Log(menu.index);
        if (menu.index == ButtonIndex)
        {
            animator.SetBool("Selected", true);
            if (Input.GetAxisRaw("Submit") == 1)
            {
                animator.SetBool("Pressed", true);
            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);

            }
        }
        else
        {
            animator.SetBool("Selected",false);

        }
    }
}
