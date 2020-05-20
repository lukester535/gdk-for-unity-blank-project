using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hand;
    public GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.Scale(rigidBody.velocity, new Vector3(0.25f, 0.25f, 0.25f));
        var dirVel = (Quaternion.FromToRotation(Vector3.up, hand.transform.position - transform.position) * new Vector3(0,500,0));
        var curVel = rigidBody.velocity;
        
        rigidBody.AddForce(dirVel);
        //rigidBody.AddTorque((hand.transform.rotation * Quaternion.Inverse(transform.rotation)) * Vector3.forward);
    }
}
