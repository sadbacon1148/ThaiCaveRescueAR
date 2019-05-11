//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
   
        using System.Collections.Generic;
        using GoogleARCore;
        using GoogleARCore.Examples.Common;
        using UnityEngine;
        using Lean.Touch;
        using DG.Tweening;
        using UnityEngine.UI;
    using UnityEngine.Playables;
    using System.Collections; //for IEnumurator

       


#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
   
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
        {
            /// <summary>
            /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
            /// </summary>
            public Camera FirstPersonCamera;

            /// <summary>
            /// A prefab for tracking and visualizing detected planes.
            /// </summary>
            public GameObject DetectedPlanePrefab;

            /// <summary>
            /// A model to place when a raycast from a user touch hits a plane.
            /// </summary>
            public GameObject AndyPlanePrefab;

            /// <summary>
            /// A model to place when a raycast from a user touch hits a feature point.
            /// </summary>
            public GameObject AndyPointPrefab;

            /// <summary>
            /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
            /// </summary>
            public GameObject SearchingForPlaneUI;

            /// <summary>
            /// The rotation in degrees need to apply to model when the Andy model is placed.
            /// </summary>
            private const float k_ModelRotation = 180.0f;

            /// <summary>
            /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
            /// the application to avoid per-frame allocations.
            /// </summary>
            private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

            /// <summary>
            /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
            /// </summary>
            private bool m_IsQuitting = false;
        public bool doRotate = false;
        
           
         GameObject andyObject;
        public Canvas startCanvas;
        public Canvas buttonCanvas;
        public Canvas backCanvas;
        GameObject stz, hfj; // gameobject.find example mung?
        bool setactivetrue = true, isPauseSideView, isPauseWater, isPauseFootsteps,isPauseRescue; //BOOL DEFAULT = FALSE
       
        public Canvas SelectCanvas;
        public Canvas BackButtonInSelectCanvas;
        PlayableDirector maintimeline;
       // GameObject water;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();
            if (maintimeline != null)
            {
                //Debug.Log(maintimeline.time);
                if (maintimeline.time >= 3.03f && isPauseSideView == true)
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

                if (maintimeline.time >= 38.36f && isPauseRescue == true)
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


            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
                bool showSearchingUI = true;
                for (int i = 0; i < m_AllPlanes.Count; i++)
                {
                    if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                    {
                        showSearchingUI = false;
                        break;
                    }
                }

                SearchingForPlaneUI.SetActive(showSearchingUI);

                // If the player has not touched the screen, we are done with this update.
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                   return;
                }


                // Raycast against the location the player touched to search for planes.
                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                    TrackableHitFlags.FeaturePointWithSurfaceNormal;

                if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {

                    if (!doRotate)
                    {
                       
                        // Choose the Andy model for the Trackable that got hit.
                        GameObject prefab;
                        if (hit.Trackable is FeaturePoint)
                        {
                            prefab = AndyPointPrefab;
                        }
                        else
                        {
                            prefab = AndyPlanePrefab;
                        }

                        // Instantiate Andy model at the hit pose.
                        if (GameObject.Find("jiji") != null)
                        {
                            Destroy(GameObject.Find("jiji"));
                        }


                        andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                        andyObject.name = "jiji";

                        andyObject.SetActive(setactivetrue);
                        

                        if (setactivetrue==true)
                        {
                            startCanvas.enabled = true; // to enable start canvas when plane is detected
                           
                           
                        }


                        // leanManualRotate2DSmooth.enabled = false;

                        // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                        // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                        // world evolves.
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make Andy model a child of the anchor.
                        andyObject.transform.parent = anchor.transform;
                    }
                    
                    }
                }
             
        }


        public void StartRotate()
        {
            doRotate = true;
      
            andyObject.GetComponent<LeanMultiSet>().enabled = true;
            andyObject.GetComponent<LeanManualRotate2DSmooth>().enabled = true;
            andyObject.GetComponent<LeanScale>().enabled = true;
            startCanvas.enabled = false;
            SelectCanvas.enabled = true;
            BackButtonInSelectCanvas.enabled = true;
            maintimeline = andyObject.GetComponent<SaveObject>().WholeCave.GetComponent<PlayableDirector>();
            //water = andyObject.GetComponent<SaveObject>().water;
        }

        /*public void EndRotate()  not using at the moment
        {
            doRotate = false;

            andyObject.GetComponentInChildren<LeanMultiSet>().enabled = false;
            andyObject.GetComponentInChildren<LeanManualRotate2DSmooth>().enabled = false;
        }*/


        public void ChooseCave()
        {
            
            SelectCanvas.enabled = false;
            buttonCanvas.enabled = true;
            backCanvas.enabled = true;
            andyObject.GetComponentInChildren<TerrainCollider>(true).gameObject.SetActive(false); //smallcube
            andyObject.GetComponent<SaveObject>().WholeCave.gameObject.SetActive(true); //Tham-kop-fbx1          
            andyObject.GetComponent<SaveObject>().invisiblecube.gameObject.SetActive(true); //invisible cube
           // maintimeline.time = 0;
           // maintimeline.Play();
           // maintimeline.Pause();
            
        }

        public void FirstBack()
        {
            SelectCanvas.enabled = false;
            startCanvas.enabled = true;
            doRotate = false;
            BackButtonInSelectCanvas.enabled = false;
            andyObject.GetComponentInChildren<TerrainCollider>(true).gameObject.SetActive(false); //smallcube
        }

        public void ChooseDiver()
        {
            
            SelectCanvas.enabled = false;
            backCanvas.enabled = true;
            andyObject.GetComponentInChildren<TerrainCollider>(true).gameObject.SetActive(false); //smallcube
            if (GameObject.Find("Capsule")!= null)
            {
                GameObject.Find("Capsule").GetComponent<Renderer>().material.DOFade(1f, 0f);
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


        public void SecondBack()
        {

            andyObject.GetComponent<SaveObject>().WholeCave.SetActive(false);
            if (GameObject.Find("Capsule") != null)
            {
                GameObject.Find("Capsule").GetComponent<Renderer>().material.DOFade(0f, 0f);
                Debug.Log("capsule gone");
            }
            andyObject.GetComponent<SaveObject>().smallcube.SetActive(true); //smallcube
            buttonCanvas.enabled = false;          
            SelectCanvas.enabled = true;
            backCanvas.enabled = false;
            StartCoroutine(DelayPivot());
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
            {
                // Exit the app when the 'back' button is pressed.
                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }

                // Only allow the screen to sleep when not tracking.
                if (Session.Status != SessionStatus.Tracking)
                {
                    const int lostTrackingSleepTimeout = 15;
                    Screen.sleepTimeout = lostTrackingSleepTimeout;
                }
                else
                {
                    Screen.sleepTimeout = SleepTimeout.NeverSleep;
                }

                if (m_IsQuitting)
                {
                    return;
                }

                // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
                if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
                {
                    _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                    m_IsQuitting = true;
                    Invoke("_DoQuit", 0.5f);
                }
                else if (Session.Status.IsError())
                {
                    _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                    m_IsQuitting = true;
                    Invoke("_DoQuit", 0.5f);
                }
            }

            /// <summary>
            /// Actually quit the application.
            /// </summary>
            private void _DoQuit()
            {
                Application.Quit();
            }

        

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                if (unityActivity != null)
                {
                    AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                    unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                            message, 0);
                        toastObject.Call("show");
                    }));
                }
        }
        IEnumerator DelayPlay()
        {

            yield return new WaitForSeconds(0.1f);
            maintimeline.Play();
        }
        IEnumerator DelayPivot()
        {

            yield return new WaitForSeconds(1.5f);
          
        }

    }
    
}
