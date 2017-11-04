using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour {

    [SerializeField]
    float strenghRepulsion = 10f;
    [SerializeField]
    float delayDust = 0.25f;
    [SerializeField]
    GameObject dustCloud;
    [SerializeField]
    Sprite[] dustSprite;

    Animator knightAnimator;
    Rigidbody2D knightRB;
    bool playerIsAttracted = false;
    float dustTimeStamp = 100f;

    // Use this for initialization
    void Start()
    {
        knightAnimator = GetComponent<Animator>();
        knightRB = GetComponent<Rigidbody2D>();

        ReleasePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        knightAnimator.SetFloat("Velocity", knightRB.velocity.y);

        if (knightRB.velocity.y > 0.9f && Time.time > dustTimeStamp)
        {
            dustTimeStamp = dustTimeStamp + delayDust;
            Vector2 dustPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            GameObject dustInstance = Instantiate(dustCloud, dustPosition, transform.rotation);//should do object pooling...
            dustInstance.GetComponent<SpriteRenderer>().sprite = dustSprite[Random.Range(0, dustSprite.Length)];
            StartCoroutine(DestroyDust(dustInstance));
        }
    }

    public void ReleasePlayer()
    {
        playerIsAttracted = false;
        dustTimeStamp = Time.time + delayDust;

        knightRB.velocity = Vector2.zero;
        knightRB.AddForce(new Vector2(0, 1) * strenghRepulsion, ForceMode2D.Impulse);
    }

    public void AttractPlayer()
    {
        playerIsAttracted = true;
    }

    IEnumerator DestroyDust(GameObject _dust)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(_dust);
    }
}
