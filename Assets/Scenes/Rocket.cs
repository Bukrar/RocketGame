using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    [SerializeField] float rotationspeed = 80f;
    [SerializeField] float thrustspeed = 20f;
    [SerializeField] AudioClip main;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip succes;
    [SerializeField] ParticleSystem mainParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem succesParticle;
    // Start is called before the first frame update
    enum State
    {
        Alive, Dying, Transcending
    }
    State state = State.Alive;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            //do Debug Input
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(succes);
                succesParticle.Play();
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                print("EE");
                deathParticle.Play();
                Invoke("LoadFirstScene", 3f);
                break;
        }
    }

    private void LoadNextScene()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneIndex = 0;
        }
        SceneManager.LoadScene(SceneIndex + 1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;
 

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * rotationspeed);
        }

        rigidbody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * thrustspeed);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(main);
                mainParticle.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainParticle.Stop();
        }

    }
}
