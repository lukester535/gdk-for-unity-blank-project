using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialosGame;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using System.Collections.Specialized;
using System.Security.Policy;

public class PlayerMovementReader : MonoBehaviour
{
    [Require] private ServerTransformReader serverTransformReader;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    void OnEnable()
    {
        UnityEngine.Debug.Log("I have read access! GameObject: " + gameObject.name);
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
        if (serverTransformReader != null)
        {
            // OnTriggerEnter() can be called even if the MonoBehaviour
            // is disabled. You need to check whether the reader is available.
        }
    }

    void OnCollisionEnter()
    {
        if (serverTransformReader != null)
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
        if (serverTransformReader != null)
        {
            head.transform.position = Vec3ftoVec3(serverTransformReader.Data.Position);
            head.transform.rotation = Quaternion.Euler(Vec3ftoVec3(serverTransformReader.Data.Rotation));
            leftHand.transform.position = Vec3ftoVec3(serverTransformReader.Data.Lposition);
            leftHand.transform.rotation = Vec3ftoQuat(serverTransformReader.Data.Lrotation);
            rightHand.transform.position = Vec3ftoVec3(serverTransformReader.Data.Rposition);
            rightHand.transform.rotation = Vec3ftoQuat(serverTransformReader.Data.Rrotation);
        }
    }

    Vector3 Vec3ftoVec3(Improbable.Vector3f Vec)
    {
        return new Vector3(Vec.X, Vec.Y, Vec.Z);
    }
    Quaternion Vec3ftoQuat(Improbable.Vector3f Vec)
    {
        return Quaternion.Euler(Vec.X * 360, Vec.Y * 360, Vec.Z * 360);
    }
}
