using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    public float lerpSpeed;


    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z);

        transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);

    }
}
