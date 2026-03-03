/**************************************************************************************************
* THE OMICRON PROJECT
 *-------------------------------------------------------------------------------------------------
 * Copyright 2010-2022		Electronic Visualization Laboratory, University of Illinois at Chicago
 * Authors:										
 *  Arthur Nishimoto		anishimoto42@gmail.com
 *-------------------------------------------------------------------------------------------------
 * Copyright (c) 2010-2022, Electronic Visualization Laboratory, University of Illinois at Chicago
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted 
 * provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions 
 * and the following disclaimer. Redistributions in binary form must reproduce the above copyright 
 * notice, this list of conditions and the following disclaimer in the documentation and/or other 
 * materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE  GOODS OR SERVICES; LOSS OF 
 * USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *************************************************************************************************/

using UnityEngine;
using System.Collections;

public class SliderMovement : CAVE2Interactable
{

    public enum HoldingStyle { ButtonPress, ButtonHold };

    [SerializeField]
    bool grabbed;

    [SerializeField]
    CAVE2.Button grabButton = CAVE2.Button.Button3;

    [SerializeField]
    CAVE2.InteractionType grabStyle = CAVE2.InteractionType.Any;

    [SerializeField]
    HoldingStyle holdInteraction = HoldingStyle.ButtonHold;

    [SerializeField]
    bool allowWandCollision = true;

    [SerializeField]
    bool centerOnWand = false;

    bool usedGravity;

    [SerializeField]
    float yMin = -0.458f;

    [SerializeField]
    float yMax = 0.447f;

    float initialLocalX;
    float initialLocalZ;
    float grabOffset;

    [SerializeField]
    Transform grabber;
    Collider[] grabberColliders;

    int grabbingWandID;

    [Header("Visuals")]
    GameObject pointingOverHighlight;
    new MeshRenderer renderer;

    [SerializeField]
    bool showPointingOver = true;

    [SerializeField]
    float highlightScaler = 1.05f;

    [SerializeField]
    Mesh defaultMesh = null;

    [SerializeField]
    bool useSimplifiedMesh = false;

    [SerializeField]
    Mesh simpleMesh = null;

    [SerializeField]
    Material pointingOverMaterial = null;

    Color originalPointingMatColor;

    [SerializeField]
    bool showTouchingOver = true;

    [SerializeField]
    Material touchingOverMaterial = null;

    Color originalTouchingMatColor;

    private void Start()
    {
        // Visuals
        pointingOverHighlight = new GameObject("wandHighlight");
        initialLocalX = transform.localPosition.x;
        initialLocalZ = transform.localPosition.z;

        pointingOverHighlight.transform.parent = transform;
        pointingOverHighlight.transform.position = transform.position;
        pointingOverHighlight.transform.rotation = transform.rotation;
        pointingOverHighlight.transform.localScale = Vector3.one * highlightScaler;

        if (defaultMesh == null)
        {
            defaultMesh = GetComponent<MeshFilter>().mesh;
        }
        if (useSimplifiedMesh)
        {
            defaultMesh = simpleMesh;
        }

        pointingOverHighlight.AddComponent<MeshFilter>().mesh = defaultMesh;
        MeshCollider wandCollider = gameObject.AddComponent<MeshCollider>();
        wandCollider.sharedMesh = defaultMesh;
        wandCollider.convex = true;
        wandCollider.isTrigger = true;

        renderer = pointingOverHighlight.AddComponent<MeshRenderer>();

        if (pointingOverMaterial == null)
        {
            // Create a basic highlight material
            pointingOverMaterial = new Material(Shader.Find("Standard"));
            pointingOverMaterial.SetColor("_Color", new Color(0, 1, 1, 0.25f));
            pointingOverMaterial.SetFloat("_Mode", 3); // Transparent
            pointingOverMaterial.SetFloat("_Glossiness", 0);
        }
        else
        {
            pointingOverMaterial = new Material(pointingOverMaterial);
        }
        if (touchingOverMaterial == null)
        {
            // Create a basic highlight material
            touchingOverMaterial = new Material(Shader.Find("Standard"));
            touchingOverMaterial.SetColor("_Color", new Color(0, 1, 1, 0.25f));
            touchingOverMaterial.SetFloat("_Mode", 3); // Transparent
            touchingOverMaterial.SetFloat("_Glossiness", 0);
        }
        else
        {
            touchingOverMaterial = new Material(touchingOverMaterial);
        }
        originalPointingMatColor = pointingOverMaterial.color;
        originalTouchingMatColor = touchingOverMaterial.color;

        renderer.sharedMaterial = pointingOverMaterial;

        renderer.enabled = false;
    }
    void Update()
    {
        // Interaction
        UpdateWandOverTimer();

        if (holdInteraction == HoldingStyle.ButtonHold && CAVE2.Input.GetButtonUp(grabbingWandID, grabButton) && grabbed)
        {
            OnWandGrabRelease();
        }

        // Visuals
        if (showPointingOver)
        {
            if (wandPointing)
            {
                renderer.sharedMaterial = pointingOverMaterial;
                pointingOverMaterial.color = originalPointingMatColor;
                renderer.enabled = true;
            }
            else
            {
                renderer.enabled = false;
            }
        }
        if (showTouchingOver)
        {
            if (wandTouching)
            {
                renderer.sharedMaterial = touchingOverMaterial;
                touchingOverMaterial.color = originalTouchingMatColor;
                renderer.enabled = true;
            }
            else if (!wandPointing)
            {
                renderer.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (grabbed)
        {
            float grabberLocalY = transform.parent != null
                ? transform.parent.InverseTransformPoint(grabber.position).y
                : grabber.position.y;
            float targetLocalY = Mathf.Clamp(grabberLocalY - grabOffset, yMin, yMax);
            Vector3 localTarget = new Vector3(initialLocalX, targetLocalY, initialLocalZ);
            Vector3 worldTarget = transform.parent != null
                ? transform.parent.TransformPoint(localTarget)
                : localTarget;
            GetComponent<Rigidbody>().MovePosition(worldTarget);
        }
    }

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == grabButton)
        {
            if (!grabbed && (evt.interactionType == grabStyle || grabStyle == CAVE2.InteractionType.Any))
            {
                grabber = CAVE2.GetWandObject(evt.wandID).transform;
                OnWandGrab();
                grabbingWandID = evt.wandID;
            }
            else if (grabbed && holdInteraction == HoldingStyle.ButtonPress)
            {
                OnWandGrabRelease();
            }
        }
    }

    void OnWandGrab()
    {
        if (GetComponent<Rigidbody>() && transform.parent != grabber)
        {
            // Check if grabbing object already is grabbing something else
            if (grabber.GetComponentInChildren<CAVE2WandInteractor>().GrabbedObject(gameObject))
            {
                // Disable collisions between grabber and collider while held
                grabberColliders = grabber.root.GetComponentsInChildren<Collider>();
                foreach (Collider c in grabberColliders)
                {
                    Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
                }

                float grabberLocalY = transform.parent != null
                    ? transform.parent.InverseTransformPoint(grabber.position).y
                    : grabber.position.y;
                grabOffset = grabberLocalY - transform.localPosition.y;

                usedGravity = GetComponent<Rigidbody>().useGravity;
                GetComponent<Rigidbody>().useGravity = false;
            }
        }
        grabbed = true;
    }

    void OnWandGrabRelease()
    {
        grabber.GetComponentInChildren<CAVE2WandInteractor>().ReleaseObject(gameObject);

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = usedGravity;
        }

        // Re-enable collisions between grabber and collider after released
        if (grabberColliders != null)
        {
            foreach (Collider c in grabberColliders)
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>(), false);
            }
        }

        grabbed = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!allowWandCollision && collision.gameObject.GetComponent<CAVE2WandInteractor>())
        {
            Collider[] grabberColliders = collision.transform.root.GetComponentsInChildren<Collider>();
            foreach (Collider c in grabberColliders)
            {
                Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
            }
        }
    }
}
