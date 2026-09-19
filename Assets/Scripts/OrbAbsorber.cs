using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbAbsorber : MonoBehaviour
{
    [Header("Input Settings")]
    public KeyCode absorbKey = KeyCode.E;

    [Header("Absorption Settings")]
    public float absorbRadius = 10f;        
    public float pullSpeed = 12f;           
    public float collectDistance = 0.8f;    

    [Header("Targeting & Layers")]
    public LayerMask orbLayer;              
    public Transform holdTarget;           

    [Header("Visual Effects")]
    public bool shrinkWhilePulling = true;
    public ParticleSystem absorbParticles;

    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip absorbLoopSound;
    public AudioClip collectSound;

    
    private Dictionary<Transform, Vector3> initialScales = new Dictionary<Transform, Vector3>();

    private void Start()
    {
        if (holdTarget == null) holdTarget = transform;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(absorbKey))
        {
            StartAbsorbEffects();
            PullNearbyOrbs();
        }
        else if (Input.GetKeyUp(absorbKey) || !Input.GetKey(absorbKey))
        {
            StopAbsorbEffects();
        }
    }

    private void PullNearbyOrbs()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, absorbRadius, orbLayer);

        foreach (var col in hitColliders)
        {
            Transform orbTransform = col.transform;

            if (!initialScales.ContainsKey(orbTransform))
            {
                initialScales.Add(orbTransform, orbTransform.localScale);
            }

            Vector3 targetPosition = holdTarget.position;
            float currentDistance = Vector3.Distance(orbTransform.position, targetPosition);

            orbTransform.position = Vector3.MoveTowards(
                orbTransform.position,
                targetPosition,
                pullSpeed * Time.deltaTime
            );

            if (shrinkWhilePulling)
            {
                float progress = Mathf.Clamp01(currentDistance / absorbRadius);
                orbTransform.localScale = Vector3.Lerp(Vector3.zero, initialScales[orbTransform], progress);
            }


            if (currentDistance <= collectDistance)
            {
                CollectOrb(col.gameObject);
            }
        }
    }

    private void CollectOrb(GameObject orb)
    {
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        if (initialScales.ContainsKey(orb.transform))
        {
            initialScales.Remove(orb.transform);
        }

        if (orb.TryGetComponent<OrbItem>(out var orbComponent))
        {
            orbComponent.OnCollected();
        }

        Destroy(orb);
    }

    private void StartAbsorbEffects()
    {
        if (absorbParticles != null && !absorbParticles.isPlaying)
            absorbParticles.Play();

        if (audioSource != null && absorbLoopSound != null && !audioSource.isPlaying)
        {
            audioSource.clip = absorbLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopAbsorbEffects()
    {
        if (absorbParticles != null && absorbParticles.isPlaying)
            absorbParticles.Stop();

        if (audioSource != null && audioSource.isPlaying && audioSource.clip == absorbLoopSound)
        {
            audioSource.Stop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, absorbRadius);
    }
}