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

public class PlayerMovementServer : MonoBehaviour
{
    [Require] private PlayerTransformReader playerTransformReader;
    [Require] private PositionWriter positionWriter;


    void OnEnable()
    {
        UnityEngine.Debug.Log("I have transform access! GameObject: " + gameObject.name);
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
        if (playerTransformReader != null)
        {
            // OnTriggerEnter() can be called even if the MonoBehaviour
            // is disabled. You need to check whether the reader is available.
        }
    }

    void OnCollisionEnter()
    {
        if (playerTransformReader != null)
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
        if (playerTransformReader != null)
        {
            var transformUpdate = new Position.Update
            {
                Coords = Vec3toCoords(playerTransformReader.Data.Position)
            };

            // Send component update to the SpatialOS Runtime
            positionWriter.SendUpdate(transformUpdate);
        }
    }

    Improbable.Coordinates Vec3toCoords(Improbable.Vector3f Vec)
    {
        return new Improbable.Coordinates(Vec.X*100, Vec.Y * 100, Vec.Z * 100);
    }
}
