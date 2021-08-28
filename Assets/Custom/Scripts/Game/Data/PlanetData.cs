using UnityEngine;

[System.Serializable]
public class PlanetData: BaseData
{
    public int id;
    public string name;
    public float radius;
    public float health;
    public GameObject planetObject;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}