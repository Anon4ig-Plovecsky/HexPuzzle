using UnityEngine;

public class ChangeMaterialHoverEvent : MonoBehaviour
{
    private string nameOfObject;
    private const int CubeSize = 6;
    [SerializeField] private Material selectedMaterial;
    private readonly Texture[] originalTextures = new Texture[CubeSize];
    private readonly Material[] materialsOfObject = new Material[CubeSize];
    private void Start()
    {
        nameOfObject = transform.parent.name;
        for (var i = 0; i < CubeSize; i++)
        {
            materialsOfObject[i] = transform.parent.GetChild(0).GetChild(i).GetComponent<Renderer>().material;
            originalTextures[i] = materialsOfObject[i].mainTexture;
        }
    }
    public void ObjectSelected(bool selected)
    {
        // for (var i = 0; i < CubeSize; i++)
        //     materialsOfObject[i].mainTexture = selected ? selectedMaterial.mainTexture : originalTextures[i];
    }
}