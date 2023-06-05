using UnityEngine;
using System;

public class CellController : MonoBehaviour
{
    private const string CorrectlyString = "[Correctly]";
    private string nameDefault, nameOtherCollider;
    private ParticleSystem effectGameObject;
    private Renderer rendererGameObject;
    private Vector3 size;
    private int index;
    private void Start()
    {
        nameDefault = name;
        index = Convert.ToInt32(name.Split('(', ')')[1]);
        size = GetComponent<BoxCollider>().size;
        rendererGameObject = transform.GetChild(1).GetComponent<Renderer>();
        effectGameObject = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (!otherCollider.name.Contains("PartOfPainting")) return;
        var rotation = otherCollider.transform.rotation.normalized.eulerAngles;
        if ((!(Math.Abs(rotation.x) % 90 <= 15) &&
             !(Math.Abs(rotation.x) % 90 >= 75)) ||
            (!(Math.Abs(rotation.y) % 90 <= 15) &&
             !(Math.Abs(rotation.y) % 90 >= 75)) ||
            (!(Math.Abs(rotation.z) % 90 <= 15) &&
             !(Math.Abs(rotation.z) % 90 >= 75))) return;
        if (!(otherCollider.transform.position.x < transform.position.x + size.x / 2f)
            || !(otherCollider.transform.position.x > transform.position.x - size.x / 2f)
            || !(otherCollider.transform.position.z < transform.position.z + size.z / 2f)
            || !(otherCollider.transform.position.z > transform.position.z - size.z / 2f)) return;
        nameOtherCollider = otherCollider.name;
        rendererGameObject.enabled = false;
        effectGameObject.Stop();
        if (index != Convert.ToInt32(otherCollider.name.Split('(', ')')[1])) return;
        if(CheckCorrectly(rotation))
            name = $"{nameDefault} {CorrectlyString}";
    }
    private void OnTriggerExit(Collider otherCollider)
    {
        if (rendererGameObject.enabled || nameOtherCollider != otherCollider.name) return;
        name = nameDefault;
        rendererGameObject.enabled = true;
        effectGameObject.Play();
    }
    private static bool CheckCorrectly(Vector3 rotation)
    {
        return (Math.Abs(rotation.x) % 360 < 5 || Math.Abs(rotation.x) % 360 > 355) &&
               (Math.Abs(rotation.z) % 360 < 5 || Math.Abs(rotation.z) % 360 > 355) &&
               (Math.Abs(rotation.y) % 360 < 15 || Math.Abs(rotation.y) % 360 > 345);
    }
}