using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class JumpBoost : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private string audioName;

    public float jumpBoostUpForce = 40.0f;
    public float jumpBoostForwardForce = 0.0f;
    public float respawnTime = 5.0f;

    private void Start()
    {
        TryGetComponent(out meshRenderer);
        TryGetComponent(out boxCollider);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out rb))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpBoostUpForce, ForceMode.VelocityChange);
            rb.AddForce(transform.forward * jumpBoostForwardForce, ForceMode.VelocityChange);
            AudioManager.instance.Play(audioName);
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
