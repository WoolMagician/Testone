using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public DroneMenuUI droneMenu;

    public TextMeshProUGUI scoreCounterText;
    public TextMeshProUGUI enemyCounterText;
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI missileCounterText;
    public TextMeshProUGUI shieldCounterText;

    // Update is called once per frame
    void Update()
    {
        scoreCounterText.text = GameManager.Instance.simulationData.mineralAcquired.ToString(); //string.Format("Score: {0}", gm.acquiredMinerals);
        enemyCounterText.text = GameManager.Instance.simulationData.defeatedEnemies.ToString();
        missileCounterText.text = GameManager.Instance.simulationData.missilesLeft.ToString();
        shieldCounterText.text = GameManager.Instance.simulationData.shieldHitsLeft.ToString();
        waveCounterText.text = WaveManager.Instance.currentWaveIndex.ToString(); //string.Format("Waves: {0}", WaveManager.Instance.currentWaveIndex);
    }


    public void ToggleDroneMenu()
    {
        if (droneMenu.menuVisible)
        {
            Director.Instance.SwitchToCamera(0);
        }
        else
        {
            Director.Instance.SwitchToCamera(1);
        }
        droneMenu.ShowMenu(!droneMenu.menuVisible);
    }
}
