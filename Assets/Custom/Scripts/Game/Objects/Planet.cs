using UnityEngine;

public class Planet : Singleton<Planet>
{
    public Shield shield;

    public GameObject orbitPrefab;
    public GameObject[] planetMeshes;

    public delegate void PlanetHit(GameObject hitObject);
    public event PlanetHit OnPlanetHit;

    void Start()
    {
        //Randomize planet mesh every new simulation
        this.InstantiatePlanetMesh(Random.Range(0, planetMeshes.Length - 1));
    }

    void Update()
    {

    }

    #region SHIELD
    
    private void SetShield(int shieldIndex)
    {

    }

    private void RemoveShield()
    {

    }

    #endregion

    private void InstantiatePlanetMesh(int meshIndex)
    {
        Instantiate(planetMeshes[meshIndex], this.transform).name = "Planet Mesh";
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            OnPlanetHit(other.gameObject);
        }
    }
}


