using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismController : MonoBehaviour {

    [SerializeField]
    float magnetStrength = 10f;
    [SerializeField]
    float maxMagnetForce = 100f;
    [SerializeField]
    GameObject avatar;
    [SerializeField]
    GameObject magneticPoint;

    Rigidbody2D avatarRB;
    Transform avatarTransform;
    Transform magneticPointTransform;

    bool buttonIsPressed = false;

    // Use this for initialization
    void Start () {
        avatarRB = avatar.GetComponent<Rigidbody2D>();
        avatarTransform = avatar.GetComponent<Transform>();
        magneticPointTransform = magneticPoint.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (buttonIsPressed)
        {
            Vector2 directionForce = magneticPointTransform.position - avatarTransform.position;
            float step = ComputeMagnetForce(directionForce.magnitude) * Time.deltaTime;
            avatarTransform.position = Vector3.MoveTowards(avatarTransform.position, magneticPointTransform.position, step);
        }
    }

    private void FixedUpdate()
    {
        /*if (buttonIsPressed)
        {
            Vector2 directionForce = magneticPointTransform.position - avatarTransform.position;
            float force = ComputeMagnetForce(directionForce.magnitude);
            directionForce.Normalize();

            avatarRB.AddForce(directionForce * force * Time.deltaTime);
        }*/
    }

    void OnMouseDown()
    {
        buttonIsPressed = true;
    }

    void OnMouseUp()
    {
        buttonIsPressed = false;

        if (IsAvaterOnMagneticPoint())
        {
            avatarRB.velocity = Vector2.zero;
            avatarRB.AddForce(new Vector2(0, 1) * 5f, ForceMode2D.Impulse);
        }
    }

    float ComputeMagnetForce(float distance)
    {
        return Mathf.Clamp((magnetStrength / distance), 0f, maxMagnetForce);
    }

    bool IsAvaterOnMagneticPoint()
    {
        if ((avatarTransform.position - magneticPointTransform.position).sqrMagnitude <= 0.01)
            return true;

        return false;
    }
}
