using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismController : MonoBehaviour {

    [SerializeField]
    float magnetStrength = 10f;
    [SerializeField]
    float maxMagnetForce = 100f;
    [SerializeField]
    float slowDownStrengh = 0.25f;
    [SerializeField]
    float minimumSlowDownSpeed = 0.1f;
    [SerializeField]
    float magnetMultiplicatorX = 2f;
    [SerializeField]
    GameObject avatar;
    [SerializeField]
    GameObject magneticPoint;
    
    Rigidbody2D avatarRB;
    Transform avatarTransform;
    KnightController knightController;
    Transform magneticPointTransform;

    bool buttonIsPressed = false;
    bool avatarIsAttached = false;

    // Use this for initialization
    void Start () {
        avatarRB = avatar.GetComponent<Rigidbody2D>();
        avatarTransform = avatar.GetComponent<Transform>();
        knightController = avatar.GetComponent<KnightController>();
        magneticPointTransform = magneticPoint.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (buttonIsPressed)
        {
            if (!avatarIsAttached && IsAvaterOnMagneticPoint())
                avatarIsAttached = true;

            if(avatarIsAttached)
                avatarTransform.position = magneticPointTransform.position;
            else if(avatarRB.velocity.y != 0f)
            {
                avatarRB.velocity *= slowDownStrengh;
                if (avatarRB.velocity.y <= minimumSlowDownSpeed)
                    avatarRB.velocity *= 0f;
            }
            else
                PullAvatar();
        }
    }

    void OnMouseDown()
    {
        buttonIsPressed = true;

        knightController.AttractPlayer();
    }

    void OnMouseUp()
    {
        buttonIsPressed = false;
        avatarIsAttached = false;

        knightController.ReleasePlayer();
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

    void PullAvatar()
    {
        Vector2 directionForce = magneticPointTransform.position - avatarTransform.position;
        float step = ComputeMagnetForce(directionForce.magnitude) * Time.deltaTime;
        float y = Vector3.MoveTowards(avatarTransform.position, magneticPointTransform.position, step).y;
        step *= magnetMultiplicatorX;
        float x = Vector3.MoveTowards(avatarTransform.position, magneticPointTransform.position, step).x;

        avatar.transform.position = new Vector2(x, y);
    }
}
