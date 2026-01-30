using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShallowWater : MonoBehaviour
{
    [Header("Splash Settings")]
    public bool enableSplash = true;
    public GameObject splashEffectPrefab;
    public AudioClip splashSound;
    [Range(0f, 1f)]
    public float splashVolume = 0.5f;

    [Header("Footstep Splash")]
    public bool enableFootstepSplash = true;
    public GameObject footstepSplashPrefab;
    public float footstepInterval = 0.4f;

    private AudioSource audioSource;
    private float lastFootstepTime;
    private Transform playerInWater;

    void Awake()
    {
        // Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        // Setup audio source for splash sounds
        if (splashSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D sound
        }
    }

    void Update()
    {
        // Footstep splashes while player is moving in water
        if (playerInWater != null && enableFootstepSplash)
        {
            CharacterController cc = playerInWater.GetComponent<CharacterController>();
            if (cc != null && cc.velocity.magnitude > 0.5f)
            {
                if (Time.time - lastFootstepTime > footstepInterval)
                {
                    SpawnFootstepSplash();
                    lastFootstepTime = Time.time;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInWater = other.transform;

            // Entry splash
            if (enableSplash)
            {
                SpawnSplash(other.transform.position);
                PlaySplashSound();
            }

            Debug.Log("[ShallowWater] Player entered water");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Exit splash
            if (enableSplash)
            {
                SpawnSplash(other.transform.position);
                PlaySplashSound();
            }

            playerInWater = null;
            Debug.Log("[ShallowWater] Player exited water");
        }
    }

    void SpawnSplash(Vector3 position)
    {
        if (splashEffectPrefab != null)
        {
            Vector3 spawnPos = new Vector3(position.x, transform.position.y + 0.1f, position.z);
            GameObject splash = Instantiate(splashEffectPrefab, spawnPos, Quaternion.identity);
            Destroy(splash, 2f);
        }
    }

    void SpawnFootstepSplash()
    {
        if (footstepSplashPrefab != null && playerInWater != null)
        {
            Vector3 spawnPos = new Vector3(playerInWater.position.x, transform.position.y + 0.05f, playerInWater.position.z);
            GameObject splash = Instantiate(footstepSplashPrefab, spawnPos, Quaternion.identity);
            Destroy(splash, 1f);
        }
    }

    void PlaySplashSound()
    {
        if (audioSource != null && splashSound != null)
        {
            audioSource.PlayOneShot(splashSound, splashVolume);
        }
    }
}
