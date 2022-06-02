using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Wire : MonoBehaviour
{
    [SerializeField] private List<GameObject> WiresToBeConnected;
    [SerializeField] private string wireName;
    private Rigidbody go_rigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (WiresToBeConnected.Contains(other.gameObject))
        {
            go_rigidbody = GetComponent<Rigidbody>();
            go_rigidbody.constraints = RigidbodyConstraints.FreezePosition;

            go_rigidbody.gameObject.AddComponent<IgnoreHovering>();
            other.gameObject.AddComponent<IgnoreHovering>();

            string returnValue = wireName + " " + other.gameObject.GetComponent<Wire>().wireName;
            GameLogic.OnWireConnect(returnValue);
        }
    }

    public Transform whatTheRopeIsConnectedTo;
    public Transform whatIsHangingFromTheRope;

    private LineRenderer lineRenderer;
    public List<Vector3> allRopeSections = new List<Vector3>();

    private float ropeLength = 1f;
    private float loadMass = 100f;

    SpringJoint springJoint;

    void Start()
    {
        springJoint = whatTheRopeIsConnectedTo.GetComponent<SpringJoint>();
        lineRenderer = GetComponent<LineRenderer>();
        UpdateSpring();
        whatIsHangingFromTheRope.GetComponent<Rigidbody>().mass = loadMass;
    }

    void Update()
    {
        DisplayRope();
    }
    private void UpdateSpring()
    {
        float density = 7750f;
        float radius = 0.02f;
        float volume = Mathf.PI * radius * radius * ropeLength;
        float ropeMass = volume * density;

        ropeMass += loadMass;

        float ropeForce = ropeMass * 9.81f;
        float kRope = ropeForce / 0.01f;

        springJoint.spring = kRope * 1.0f;
        springJoint.damper = kRope * 0.8f;
        springJoint.maxDistance = ropeLength;
    }

    private void DisplayRope()
    {
        float ropeWidth = 0.2f;

        //lineRenderer.startWidth = ropeWidth;
        //lineRenderer.endWidth = ropeWidth;

        Vector3 A = whatTheRopeIsConnectedTo.position;
        Vector3 D = whatIsHangingFromTheRope.position;

        Vector3 B = A + whatTheRopeIsConnectedTo.up * (-(A - D).magnitude * 0.1f);
        Vector3 C = D + whatIsHangingFromTheRope.up * ((A - D).magnitude * 0.5f);

        BezierCurve.GetBezierCurve(A, B, C, D, allRopeSections);

        Vector3[] positions = new Vector3[allRopeSections.Count];

        for (int i = 0; i < allRopeSections.Count; i++)
        {
            positions[i] = allRopeSections[i];
        }

     
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }


}
