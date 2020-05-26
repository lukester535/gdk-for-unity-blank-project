using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hand;
    public GameObject model;
    public int lerpSpeed = 32;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var handT = hand.transform;
        var modelT = model.transform;

        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.Scale(rigidBody.velocity, new Vector3(0.25f, 0.25f, 0.25f));
        var dirVel = (Quaternion.FromToRotation(Vector3.up, handT.position - transform.position) * new Vector3(0,500,0));
        var curVel = rigidBody.velocity;
        var dist = Vector3.Distance(transform.position, handT.position);

        rigidBody.MovePosition(Vector3.Lerp(transform.position, handT.position, Time.deltaTime * lerpSpeed));
        rigidBody.MoveRotation( Quaternion.Lerp(transform.rotation, handT.rotation, Time.deltaTime * lerpSpeed) );
        //rigidBody.AddForce(dirVel * Mathf.Pow(dist,2));
        //rigidBody.AddTorque((hand.transform.rotation * Quaternion.Inverse(transform.rotation)) * Vector3.forward);
    }
}
