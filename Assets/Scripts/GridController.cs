using UnityEngine;

public class GridController : GridGenerator
{
    private const string CorrectlyString = "[Correctly]";
    private GameObject cubes;
    private bool isWin;
    protected override void Start()
    {
        base.Start();
        cubes = GameObject.Find("Cubes");
    }
    private void Update()
    {
        if (cubes.transform.childCount != CubeQty.Item1 * CubeQty.Item2 || isWin) return;
        CheckCorrectly();
    }
    private void CheckCorrectly()
    {
        var isCorrectly = true;
        for(var i = 0; i < CubeQty.Item1 * CubeQty.Item2; i++)
            if (!GridGameObject.transform.GetChild(i).name.Contains(CorrectlyString))
                isCorrectly = false;
        if (!isCorrectly) return;
        isWin = true;
        Debug.Log("You win!");
    }
}