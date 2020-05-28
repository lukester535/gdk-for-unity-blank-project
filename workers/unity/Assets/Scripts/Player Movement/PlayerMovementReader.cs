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
    public GameObject body;

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
            head.transform.localPosition = Vec3ftoVec3(serverTransformReader.Data.Position);
            head.transform.rotation = QuatftoQuat(serverTransformReader.Data.Rotation);
            leftHand.transform.localPosition = Vec3ftoVec3(serverTransformReader.Data.Lposition);
            leftHand.transform.rotation = QuatftoQuat(serverTransformReader.Data.Lrotation);
            rightHand.transform.localPosition = Vec3ftoVec3(serverTransformReader.Data.Rposition);
            rightHand.transform.rotation = QuatftoQuat(serverTransformReader.Data.Rrotation);
            body.transform.localPosition = Vec3ftoVec3(serverTransformReader.Data.Bposition);
            body.transform.rotation = QuatftoQuat(serverTransformReader.Data.Brotation);
        }
    }

    Vector3 Vec3ftoVec3(Improbable.Vector3f Vec)
    {
        return new Vector3(Vec.X, Vec.Y, Vec.Z);
    }
    Quaternion QuatftoQuat(Improbable.Quaternionf Quat)
    {
        return new Quaternion(Quat.X, Quat.Y, Quat.Z, Quat.W);
    }
}
