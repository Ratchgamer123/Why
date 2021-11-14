using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class JumpBoost : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private VisualEffect vfx;

    public float jumpBoostForce = 40f;
    public float respawnTime = 5f;

    private void Start()
    {
        TryGetComponent(out meshRenderer);
        TryGetComponent(out boxCollider);
        TryGetComponent(out vfx);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out rb))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpBoostForce, ForceMode.VelocityChange);
            AudioManager.instance.Play("JumpBoost");
            StartCoroutine(ReEnable());
        }
    }

    IEnumerator ReEnable()
    {
        boxCollider.enabled = false;
        vfx.Play();
        meshRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        boxCollider.enabled = true;
        meshRenderer.enabled = true;
    }
}
