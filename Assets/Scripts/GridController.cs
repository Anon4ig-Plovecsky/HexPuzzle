using UnityEngine;

public class GridController : GridConstructor
{
    private GameObject cubes;
    protected override void Start()
    {
        base.Start();
        cubes = GameObject.Find("Cubes");
    }
    void Update()
    {
        
    }
}
