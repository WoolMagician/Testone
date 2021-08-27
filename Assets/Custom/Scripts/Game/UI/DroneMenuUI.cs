using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[System.Serializable]
//public class TurretMenuItem
//{
//    public TurretData turretData;
//    public AmmoData ammoData;
//    public Transform transform;
//    public Transform turretPivot;

//    [HideInInspector]
//    public Vector3 oldPos;
//}

public class DroneMenuUI : MonoBehaviour
{
    public Transform mainPivot;
    public bool menuVisible;
    public DroneMenuButton[] items = new DroneMenuButton[3];

    //public delegate void SelectedTurretArgs(TurretMenuItem turretData);
    //public event SelectedTurretArgs OnSelectedTurret;



    // Start is called before the first frame update
    void Start()
    {
        foreach (DroneMenuButton item in items)
        {
            item.gameObject.SetActive(false);
            item.transform.position = this.transform.position;
        }
    }

    public void HandleTurretSelection(int index)
    {
        //if (items[index].turretData != null && items[index].ammoData != null)
        //{
        //    OnSelectedTurret(items[index]);
        //}
    }

    //public void SetItemData(int itemIndex, TurretData turrData, AmmoData ammoData)
    //{
    //    if (itemIndex < items.Length)
    //    {
    //        items[itemIndex].turretData = turrData;
    //        items[itemIndex].ammoData = ammoData;
    //        this.InitializeMenuItems();
    //    }
    //}

    public void ShowMenu(bool value)
    {
        StartCoroutine(ShowMenuAsync(value));
    }

    private IEnumerator ShowMenuAsync(bool value)
    {
        menuVisible = value;
        DOTween.CompleteAll(true);

        foreach (DroneMenuButton item in items)
        {
            if (value)
            {
                item.gameObject.SetActive(true);
                item.transform.localScale = Vector3.zero;
                item.transform.DOMove(item.startPosition, 1f).SetEase(Ease.OutElastic, 0.1f, 1f);
                item.transform.DOScale(Vector3.one * 1.3f, 0.3f);
            }
            else
            {
                item.transform.DOMove(this.transform.position, 0.3f).OnComplete(() => item.gameObject.SetActive(false));
                item.transform.DOScale(Vector3.zero, 0.3f);
            }

        }
        yield return new WaitForSeconds(0);
    }

    //private void InitializeMenuItems()
    //{
    //    foreach (TurretMenuButton item in items)
    //    {
    //        item.oldPos = item.transform.position;
    //        item.transform.position = this.transform.position;

    //        if (item != null && item.turretData != null && item.turretData.levels.Length > 0)
    //        {
    //            if (item.turretData.levels[0].turretObject != null)
    //            {
    //                GameObject turretPreview = Instantiate(item.turretData.levels[0].turretObject, item.turretObjectPivot);
    //                turretPreview.layer = LayerMask.NameToLayer("UI");
    //                turretPreview.transform.localScale = Vector3.one * 25f;
    //            }
    //            else
    //            {
    //                //Metti mesh di default
    //            }
    //        }
    //    }
    //}
}
