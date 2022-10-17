using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl ctrl;

    public Transform[] obstacles = new Transform[6];
    public List<Transform> image = new List<Transform>();
    public GameObject obs;
    public GameObject images;
    public int index;
    public int imageindex;
    public Transform[] trampoline = new Transform[6];
    public GameObject tramp;
    public int indexTramp;
    public GameObject collisionObj;

    public int animPos;
    public float lerpSpeed;
    bool tap;
    public bool play;
    public Vector3 newPos;

    private Rigidbody rb;
    public float gravityY;
    private AudioClip jump,item,fail;
    private Animator anim;


    private void Awake()
    {
        ctrl = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i] = obs.transform.GetChild(i);

        }
        for (int i = 0; i < trampoline.Length; i++)
        {
            trampoline[i] = tramp.transform.GetChild(i);

        }
        for (int i = 0; i < image.Count; i++)
        {
            image[i] = images.transform.GetChild(i);

        }
        play = true;
        collisionObj = null;
        imageindex = Random.Range(0, image.Count);
        images.transform.GetChild(imageindex).gameObject.SetActive(true);
        newPos = transform.position;
        anim = GetComponent<Animator>();
        jump = Resources.Load<AudioClip>("jump");
        item = Resources.Load<AudioClip>("item");
        fail = Resources.Load<AudioClip>("fail");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (play)
        {
            if (other.gameObject.CompareTag("Obs"))
            {
                if (image[imageindex].transform.name == obstacles[index].transform.name)
                {

                    GetComponent<AudioSource>().PlayOneShot(item, .5f);

                    newPos = new Vector3(transform.position.x, trampoline[indexTramp].position.y, trampoline[indexTramp].position.z);
                    other.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    tap = false;

                    index++;
                    collisionObj = other.gameObject;

                    Invoke("Delay", 1);

                }
                else
                {
                    rb.AddForce(Vector3.down * gravityY * 100 * Time.deltaTime);

                    AnimFalse();
                    anim.SetBool("die",true);
                    newPos = transform.position;
                    rb.isKinematic = false;
                    other.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    GetComponent<AudioSource>().PlayOneShot(fail, .5f);
                    GameManager.gm.fail = true;
                }

            }
            if (other.gameObject.CompareTag("TrampolinePos"))
            {

                rb.isKinematic = false;
                AnimFalse();
                indexTramp++;
                newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                for (int i = 0; i < image.Count; i++)
                {
                    image[i].gameObject.SetActive(false);

                }
                imageindex = Random.Range(0, image.Count);

                images.transform.GetChild(imageindex).gameObject.SetActive(true);

                other.gameObject.SetActive(false);

            }

        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (play)
        {
            if (collision.gameObject.CompareTag("Trampoline"))
            {
                if (!tap)
                {
                    
                    anim.CrossFadeInFixedTime("idle", 1);
                    GetComponent<AudioSource>().PlayOneShot(jump, .5f);
                    rb.AddForce(Vector3.up * gravityY * Time.deltaTime);
                    if (index >= 6)
                    {
                        play = false;
                        GameManager.gm.nLevel = true;

                    }
                    else
                    {
                        if (image[imageindex].transform.name == obstacles[index].transform.name)
                        {
                            collision.gameObject.transform.GetChild(2).transform.GetComponent<SpriteRenderer>().material.color = Color.green;
                        }
                        else
                            collision.gameObject.transform.GetChild(2).transform.GetComponent<SpriteRenderer>().material.color = Color.red;

                    }

                }
                if (tap)
                {
                    AnimControl();

                    rb.isKinematic = true;
                    newPos = new Vector3(transform.position.x, obstacles[index].position.y + 1f, obstacles[index].position.z);
                }

            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                newPos = transform.position;

            }

        }
    }


    void Delay()
    {
        collisionObj.SetActive(false);
        collisionObj = null;
    }


    public void imageChangeButton()
    {
        if (!tap && play)
        {
            for (int i = 0; i < image.Count; i++)
            {
                image[i].gameObject.SetActive(false);

            }
            imageindex++;
            if (imageindex >=6)
            {
                imageindex = 0;
            }

            images.transform.GetChild(imageindex).gameObject.SetActive(true);

        }
    }

    public void JumpButton()
    {
        if (!tap)
        {
            tap = true;
        }

    }


    void AnimControl()
    {

        if (image[imageindex].name == "back")
        {
            anim.SetBool("back", true);
        }

        if (image[imageindex].name == "handUp")
        {
            anim.SetBool("handUp", true);
        }

        if (image[imageindex].name == "right")
        {
            anim.SetBool("right", true);
        }

        if (image[imageindex].name == "relax")
        {
            anim.SetBool("relax", true);
        }

        if (image[imageindex].name == "left")
        {
            anim.SetBool("left", true);
        }

        if (image[imageindex].name == "sit")
        {
            anim.SetBool("sit", true);
        }

    }

    void AnimFalse()
    {
        anim.SetBool("back", false);
        anim.SetBool("handUp", false);
        anim.SetBool("right", false);
        anim.SetBool("relax", false);
        anim.SetBool("left", false);
        anim.SetBool("sit", false);

    }
}
