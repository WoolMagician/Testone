using UnityEngine;

public interface IPowerUP
{
    void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject);
}

// Forse è meglio tipizzare?
//public interface IPowerUP<T> where T : IData
//{
//    void ApplyPowerUP(T data);
//}