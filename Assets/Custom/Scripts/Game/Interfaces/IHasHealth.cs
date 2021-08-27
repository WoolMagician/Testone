public delegate void HealthChangedArgs(float oldValue, float newValue);

public interface IHasHealth
{
    float Health { get; }

    void SetHealth(float healthValue);
    void DecreaseHealth(float value);
    void IncreaseHealth(float value);

    event HealthChangedArgs OnHealthChanged;

}