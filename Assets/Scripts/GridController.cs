using UnityEngine;
using System;

public class GridController : GridConstructor
{
    private const string CorrectlyString = "[Correctly]";
    private readonly Tuple<int, int> cubeQty = new(3, 2); //[SerializeField]
    private GameObject cubes;
    private GameObject grid;
    private bool isWin;
    protected override void Start()
    {
        base.Start();
        cubes = GameObject.Find("Cubes");
        grid = GameObject.Find("Grid");
    }
    private void Update()
    {
        if (cubes.transform.childCount != cubeQty.Item1 * cubeQty.Item2 || isWin) return;
        CheckCorrectly();
    }
    private void CheckCorrectly()
    {
        var isCorrectly = true;
        for(var i = 0; i < cubeQty.Item1 * cubeQty.Item2; i++)
            if (!grid.transform.GetChild(i).name.Contains(CorrectlyString))
                isCorrectly = false;
        if (!isCorrectly) return;
        isWin = true;
        Debug.Log("You win!");
    }
}