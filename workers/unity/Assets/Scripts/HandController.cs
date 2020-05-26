using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandController : MonoBehaviour
{
    public SteamVR_Action_Vector2 move;
    public Transform moveAxis;
    public float speed;
    public GameObject body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 VecAxis = moveAxis.rotation.eulerAngles;
        Vector2 moveRotated = move.axis.Rotate(-VecAxis.y);
        body.transform.position += speed * Time.deltaTime * (new Vector3(moveRotated.x,0,moveRotated.y));
    }
}
