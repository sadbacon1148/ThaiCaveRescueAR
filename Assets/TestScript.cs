using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TestScript : MonoBehaviour {
    PlayableDirector maintimeline;
    public GameObject Cave;
    //public GameObject water;
    bool isPauseSideView, isPauseWater, isPauseFootsteps, isPauseRescue;
    // Use this for initialization
    void Start () {
        maintimeline = Cave.GetComponent<SaveObject>().WholeCave.GetComponent<PlayableDirector>();
    }
	
	// Update is called once per frame
	void Update () {
        if (maintimeline != null)
        {
            //Debug.Log(maintimeline.time);
            if (maintimeline.time >= 4.27f && isPauseSideView == true)
            {
                isPauseSideView = false;
                maintimeline.Pause();
                //water.SetActive(false);

                Debug.Log("sideviewdown");
            }

            if (maintimeline.time >= 14.14f && isPauseFootsteps == true)
            {
                isPauseFootsteps = false;
                maintimeline.Pause();
                //water.SetActive(false);

                Debug.Log("footstepsstart&pause");
            }

            if (maintimeline.time >= 21.00f && isPauseWater == true)
            {
                isPauseWater = false;
                maintimeline.Pause();
                Debug.Log("waterstart&pause");
                Debug.Log(maintimeline.time);
            }

            if (isPauseWater && maintimeline.state == PlayState.Paused)
            {
               // water.transform.localPosition = new Vector3(0, 0, 0);
            }

            if (maintimeline.time >= 35.27f && isPauseRescue == true)
            {
                isPauseRescue = false;
                maintimeline.Pause();
                Debug.Log("rescuestart&pause");
            }

            /*if (water.activeSelf)
            {
                Debug.Log(water.transform.localPosition);
            }*/
            /*if (maintimeline.time < 0.5f)
            {
                maintimeline.Pause();

            }*/
            /*if (maintimeline.time>=42.29)
            {
                maintimeline.Stop();
                maintimeline.time = 0;
            }*/
        }


    }

    public void Fullview()
    {
       // maintimeline.Pause();
        maintimeline.time = 39.33f;
        StartCoroutine(DelayPlay());
        Debug.Log("full");
    }


    public void SideView()
    {
        //maintimeline.Pause();
        maintimeline.time = 0f;
        StartCoroutine(DelayPlay());

        isPauseSideView = true;
    }


    public void Water()
    {
        //maintimeline.Pause();
        // water.SetActive(true);
        maintimeline.time = 16.10f;
        StartCoroutine(DelayPlay());

        isPauseWater = true;

    }

    public void FootSteps()
    {
       // maintimeline.Pause();
        maintimeline.time = 4.28f;
        StartCoroutine(DelayPlay());

        isPauseFootsteps = true;

    }

    public void Rescue()
    {
        //maintimeline.Pause();
        //water.SetActive(true);
        maintimeline.time = 21.01f;
        StartCoroutine(DelayPlay());

        isPauseRescue = true;

    }

    IEnumerator DelayPlay()
    {

        yield return new WaitForSeconds(0.1f);
        maintimeline.Play();
    }
}
