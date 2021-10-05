using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceChange : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Stance1", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Stance 1"))
        {
            SetStanceFalse();
            anim.SetBool("Stance1", true);
            Debug.Log("In stance 1");
        }
        if (Input.GetButtonDown("Stance 2"))
        {
            SetStanceFalse();
            anim.SetBool("Stance2", true);
            Debug.Log("In stance 2");
        }
        if (Input.GetButtonDown("Stance 3"))
        {
            SetStanceFalse();
            anim.SetBool("Stance3", true);
            Debug.Log("In stance 3");
        }
        if (Input.GetButtonDown("Stance 4"))
        {
            SetStanceFalse();
            anim.SetBool("Stance4", true);
            Debug.Log("In stance 4");
        }
    }

    public void SetStanceFalse()
    {
        anim.SetBool("Stance1", false);
        anim.SetBool("Stance2", false);
        anim.SetBool("Stance3", false);
        anim.SetBool("Stance4", false);
    }
}
