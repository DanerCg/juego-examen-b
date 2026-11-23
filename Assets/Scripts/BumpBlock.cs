using System.Collections;
using UnityEngine;

/// Bloque que reacciona (sonido + animación) cuando Bart lo golpea desde abajo.
public class BumpBlock : MonoBehaviour
{
    public AudioClip bumpSound;
    public float bumpHeight = 0.2f;
    public float bumpTime = 0.12f;

    AudioSource audioSource;
    Animator anim;
    Vector3 startPos;
    bool bumping;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
    }

    public void Bump()
    {
        if (bumping) return;
        if (audioSource && bumpSound) audioSource.PlayOneShot(bumpSound);
        if (anim != null) anim.SetTrigger("Bump");   // opcional
        StartCoroutine(BumpRoutine());
    }

    IEnumerator BumpRoutine()
    {
        bumping = true;
        Vector3 up = startPos + Vector3.up * bumpHeight;

        for (float t = 0; t < bumpTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, up, t / bumpTime);
            yield return null;
        }
        for (float t = 0; t < bumpTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(up, startPos, t / bumpTime);
            yield return null;
        }

        transform.position = startPos;
        bumping = false;
    }
}
