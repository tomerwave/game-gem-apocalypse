using UnityEngine;
using EasyPeasyFirstPersonController;

public class AnimatorController : MonoBehaviour
{
    public GameObject plane;
    private Animator anim;
    private InputManagerOld input;


    void Start()
    {
        anim.GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        input = playerObj.GetComponent<InputManagerOld>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.mask)
        {
            anim.SetBool("hasMask",true);
        }
        else
        {
            anim.SetBool("hasMask",false);
        }
    }
}
