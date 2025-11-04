using UnityEngine;

public class RandomNormalMapYes : MonoBehaviour
{
    public Texture2D[] normalMaps; // Assign your normal map sprites here
    private Material mat;

    void Start()
    {
        // Get a unique instance of the material
        mat = GetComponent<Renderer>().material;

        // Pick a random normal map
        Texture2D chosenNormal = normalMaps[Random.Range(0, normalMaps.Length)];

        // Assign it to the material
        mat.SetTexture("_BumpMap", chosenNormal);

        // Make sure Unity knows it's a normal map
        mat.EnableKeyword("_NORMALMAP");
    }
}
