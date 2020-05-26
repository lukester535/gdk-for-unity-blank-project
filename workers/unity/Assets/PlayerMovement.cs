using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialosGame;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using System.Collections.Specialized;
using System.Security.Policy;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Require] private PlayerTransformWriter playerTransformWriter;
    public Transform head;
    public Transform camera;
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;


    void OnEnable()
    {
        UnityEngine.Debug.Log("I have write access! GameObject: " + gameObject.name);
        // The MonoBehaviour is automatically enabled and disabled based on
        // whether requirements for your injected types are met.
        // OnEnable() is only called when healthReader is available.
        //Debug.Log($"Current health: {healthReader.Data.Value}");
    }

    void OnDisable()
    {
        // It will be automatically called when at least one requirements
        // for your declared Requirables is not met anymore.
        // Your injected types will be automatically disposed.
    }

    void OnTriggerEnter()
    {
        if (playerTransformWriter != null)
        {
            // OnTriggerEnter() can be called even if the MonoBehaviour
            // is disabled. You need to check whether the reader is available.
        }
    }

    void OnCollisionEnter()
    {
        if (playerTransformWriter != null)
        {
            // OnCollisionEnter() can be called even if the MonoBehaviour
            // is disabled. You need to check whether the reader is available.
        }
    }

    void Start()
    {
    }

    private void Update()
    {
        if (playerTransformWriter != null)
        {
            var transformUpdate = new PlayerTransform.Update
            {
                Position = Vec3ToVec3f(camera.position + head.position),
                Rotation = QuatToQuatf(camera.rotation),
                Lposition = Vec3ToVec3f(leftHand.position),
                Lrotation = QuatToQuatf(leftHand.rotation),
                Rposition = Vec3ToVec3f(rightHand.position),
                Rrotation = QuatToQuatf(rightHand.rotation),
                Bposition = Vec3ToVec3f(body.position),
                Brotation = QuatToQuatf(body.rotation)
            };

            // Send component update to the SpatialOS Runtime
            playerTransformWriter.SendUpdate(transformUpdate);
        }
    }
    Improbable.Vector3f Vec3ToVec3f(Vector3 Vec)
    {
        return new Vector3f(Vec.x, Vec.y, Vec.z);
    }
    Improbable.Quaternionf QuatToQuatf(Quaternion Quat)
    {
        return new Quaternionf(Quat.x,Quat.y,Quat.z,Quat.w);
    }
}
