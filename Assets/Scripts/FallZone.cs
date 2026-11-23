using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Zona (trigger) debajo del hueco. Si Bart cae: sonido + animación y reinicio automático.
[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    public AudioClip fallSound;
    public float restartDelay = 1.5f;

    AudioSource audioSource;
    bool triggered;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        triggered = true;
        player.Die();                                   // animación "Die"
        if (audioSource && fallSound) audioSource.PlayOneShot(fallSound);
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
