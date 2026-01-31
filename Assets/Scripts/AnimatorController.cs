using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EasyPeasyFirstPersonController;

public class AnimatorController : MonoBehaviour
{
    public GameObject plane;
    public Image fadeImage;
    public Color FutureColor,PastColor;
    private Animator anim;
    private InputManagerOld input;
    private Camera cam;
    private bool startAnimation,calledOnce;
    private Vector3 locationPlane;
    
    private void setTransparcyForPlane(bool see)
    {
        Renderer r = plane.GetComponent<Renderer>();
        Material mat = r.material;

        Color c = mat.GetColor("_BaseColor");
        c.a = see?1f:0f;
        
        mat.SetColor("_BaseColor", c);
    }
    private void callAll(bool future)
    {
        ToggleTimeScript[] objects = FindObjectsOfType<ToggleTimeScript>();
        foreach(ToggleTimeScript toggleTimeScript in objects)
        {
            toggleTimeScript.OnGlobalToggleChanged(future);
        }
    }
    public IEnumerator FadeInOut(bool setActivePlane)
    {
        yield return Fade(1f, setActivePlane);
        Debug.Log("Fade end:"+setActivePlane);
        startAnimation = false;
        calledOnce = setActivePlane;
        //yield return Fade(1, false);
    }
    public IEnumerator Fade(float duration,bool colorOfFuture)
    {
        Color c = colorOfFuture?FutureColor:PastColor;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            c.a = !colorOfFuture?(t / duration):(1 - (t / duration));
            if(t>0.5f)
                setTransparcyForPlane(colorOfFuture);
            fadeImage.color = c;
            yield return null;
        }
        c.a = !colorOfFuture?0:0.3f;
        fadeImage.color = c;
        callAll(colorOfFuture);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        input = playerObj.GetComponent<InputManagerOld>();
        cam = playerObj.GetComponentInChildren<Camera>();
        startAnimation =false;
        calledOnce = false;
        setTransparcyForPlane(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.mask && !startAnimation){
            //set oppsite
            startAnimation = true;
            anim.SetBool("HaveMask", !anim.GetBool("HaveMask"));
            Debug.Log(anim.GetBool("HaveMask") + "," +  anim.GetBool("MaskChange"));
            if(!anim.GetBool("HaveMask") && anim.GetBool("MaskChange"))
            {
                StartCoroutine(FadeInOut(anim.GetBool("HaveMask")));
                anim.SetBool("MaskChange", false);
            }
            input.mask = false;
        }
        if(anim.GetBool("HaveMask") && anim.GetBool("MaskChange")&&!calledOnce)
        {
            calledOnce = true;
            StartCoroutine(FadeInOut(true));
        }
        if(input.interact && input.hold)
            anim.SetBool("Interact",input.interact);
        else if(!input.hold)
            anim.SetBool("Interact",input.interact);
    }
}
