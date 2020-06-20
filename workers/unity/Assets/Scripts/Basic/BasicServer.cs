using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialosGame;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using System.Collections.Specialized;
using System.Security.Policy;

public class BasicServer : MonoBehaviour
{
    [Require] private BasicObjectServerReader basicObjectServerReader;
    [Require] private BasicObjectServerWriter basicObjectServerWriter;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vec3ftoVec3(basicObjectServerReader.Data.Position);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic == false)
        {
            if (basicObjectServerWriter != null)
            {
                var transformUpdate = new BasicObjectServer.Update
                {
                    Position = Vec3ToVec3f(transform.position)
                };

                basicObjectServerWriter.SendUpdate(transformUpdate);
            }
        }
    }

    Improbable.Vector3f Vec3ToVec3f(Vector3 Vec)
    {
        return new Vector3f(Vec.x, Vec.y, Vec.z);
    }

    Vector3 Vec3ftoVec3(Improbable.Vector3f Vec)
    {
        return new Vector3(Vec.X, Vec.Y, Vec.Z);
    }
}
