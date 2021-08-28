using System.Collections.Generic;

public interface IHasPowerUPs
{
    List<IPowerUP> PowerUPs { get; set; }

    void ApplyPowerUPs();
}

// Forse è meglio tipizzare?
//public interface IHasPowerUPs<T>
//{
//    List<IPowerUP<T>> PowerUPs { get; set; }

//    void ApplyPowerUPs();
//}