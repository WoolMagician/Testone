[System.Serializable]
public abstract class BaseData : IData
{
    //Add serialization/deserialization methods to allow data saving.

    public abstract IData Copy();
}