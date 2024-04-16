using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    
    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning;

    private void Start()
    {
        isTransitioning = false;
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }

        else
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    StartLevelUpSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        }

        void StartCrashSequence()
        {
            
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(crashSound);
            GetComponent<Movement>().enabled = false;
            crashParticle.Play();
            Invoke("ReloadLevel", 1f);



        }
    }

        void StartLevelUpSequence()
        {
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(successSound);
            GetComponent<Movement>().enabled = false;
            successParticles.Play();
            Invoke("LoadNextLevel", levelLoadDelay);

        }

        void ReloadLevel()
        {
            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextLevelIndex = 0;
            }
            SceneManager.LoadScene(nextLevelIndex);
        }
    
}
