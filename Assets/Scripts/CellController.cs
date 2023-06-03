using UnityEngine;

public class CellController : MonoBehaviour
{
    private ParticleSystem effectGameObject;
    private Renderer rendererGameObject;
    private Vector3 size;
    private void Start()
    {
        size = GetComponent<BoxCollider>().size;
        rendererGameObject = transform.GetChild(1).GetComponent<Renderer>();
        effectGameObject = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.name.Contains("PartOfPainting")) return;
        if (!(otherCollider.transform.rotation.y % 90 <= 30) &&
            !(otherCollider.transform.rotation.y % 90 >= 60)) return;
        if (!(otherCollider.transform.position.x < transform.position.x + size.x / 2f)
            || !(otherCollider.transform.position.x > transform.position.x - size.x / 2f)
            || !(otherCollider.transform.position.z < transform.position.z + size.z / 2f)
            || !(otherCollider.transform.position.z > transform.position.z - size.z / 2f)) return;
        rendererGameObject.enabled = false;
        effectGameObject.Stop();
    }
    private void OnTriggerExit(Collider otherCollider)
    {
        if (rendererGameObject.enabled) return;
        rendererGameObject.enabled = true;
        effectGameObject.Play();
    }
}
