using UnityEngine;

public class PuzzlesRotator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isClockwise;
    private void FixedUpdate()
    {
        transform.Rotate(0, 0.1f * speed * (isClockwise ? -1 : 1), 0);
    }
}
