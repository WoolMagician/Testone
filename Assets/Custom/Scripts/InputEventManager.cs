using UnityEngine;

public class InputEventManager : Singleton<InputEventManager>
{
    public LayerMask enemyMask;
    public LayerMask lootMask;
    public LayerMask planetMask;

    public delegate void LootClickDelegate(Loot loot);
    public event LootClickDelegate OnLootClick;

    public delegate void EnemyClickDelegate(Enemy enemy);
    public event EnemyClickDelegate OnEnemyClick;

    public delegate void PlanetClickDelegate(Planet planet);
    public event PlanetClickDelegate OnPlanetClick;

    // Update is called once per frame
    void Update()
    {
        // Need to be changed to unity touch input
        if (Input.GetMouseButtonDown(0))
        {
            //Doesn't work with post-process lens
            Ray ray = Director.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, lootMask))
            {
                this.OnLootClick(hit.collider.gameObject.GetComponent<Loot>());
            }
            else if (Physics.Raycast(ray, out hit, float.MaxValue, planetMask))
            {
                this.OnPlanetClick(hit.collider.gameObject.GetComponent<Planet>());
            }
            else if (Physics.Raycast(ray, out hit, float.MaxValue, enemyMask))
            {
                this.OnEnemyClick(hit.collider.gameObject.GetComponent<Enemy>());
            }
        }
    }
}
