using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMenuButton : MonoBehaviour
{
    public Image buttonImage;
    public Color unavailableTurretButtonColor;
    public DroneSO turretData;
    public Camera cam;
    public Canvas canvas;
    private Vector3 startPos;
    public Transform turretObjectPivot;
    public GameObject turretPreview;
    private GameObject selectedOrbit;
    public Vector3 startPosition = Vector3.zero;
    public TMPro.TextMeshProUGUI priceTag;

    private bool isTurretAvailable
    {
        get
        {
            return turretData != null && turretData.Data != null && turretData.Data.levels.Length > 0;
        }
    }

    private RectTransform rectTransform
    {
        get
        {
            return transform as RectTransform;
        }
    }

    private void Awake()
    {
        if (startPosition == Vector3.zero)
        {
            startPosition = this.rectTransform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        if (isTurretAvailable)
        {
            if (buttonImage != null)
            {
                buttonImage.color = Color.white;
            }

            if (turretData.Data.levels[0].droneObject != null)
            {
                if (turretPreview != null)
                {
                    Destroy(turretPreview);
                }
                turretPreview = Instantiate(turretData.Data.levels[0].droneObject, turretObjectPivot);
                turretPreview.layer = LayerMask.NameToLayer("UI");
                SetLayerAllChildren(turretPreview.transform, LayerMask.NameToLayer("UI"));
                turretPreview.transform.localScale = turretData.Data.levels[0].droneObjectScaleOverride * 75;

                if (priceTag != null)
                {
                    priceTag.text = turretData.Data.levels[0].energyCost.ToString();
                }
            }
            else
            {
                //Metti mesh di default
            }
        }
        else
        {
            if(buttonImage != null)
            {
                buttonImage.color = unavailableTurretButtonColor;
            }
        }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            //            Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }


    private void OnMouseUp()
    {
        // If no turret data is defined just skip
        if (!isTurretAvailable) return;

        if (selectedOrbit != null && float.TryParse(selectedOrbit.name, out float radius))
        {
            if (GameManager.Instance.simulationData.mineralAcquired >= turretData.Data.levels[0].energyCost)
            {
                //Start the game with one turret
                Drone newTurret = DroneFactory.Instance.Create(turretData);
                newTurret.SetOrbit(OrbitFactory.Instance.Create((int)radius));

                //TurretFactory.CreateTurret(OrbitFactory.Instance.CreateNewOrbit(radius), turretData, ammoData);
                GameManager.Instance.simulationData.mineralAcquired -= turretData.Data.levels[0].energyCost;
            }
            selectedOrbit.GetComponentInParent<OrbitDrawer>().lineWidth = 0.02f;
        }
        turretObjectPivot.position = new Vector3(this.transform.position.x, this.transform.position.y, turretObjectPivot.position.z);
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseDrag()
    {
        // If no turret data is defined just skip
        if (!isTurretAvailable) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (selectedOrbit != null)
        {
            selectedOrbit.GetComponentInParent<OrbitDrawer>().lineWidth = 0.02f;
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Orbit")))
        {
            selectedOrbit = hit.collider.gameObject;
            selectedOrbit.GetComponentInParent<OrbitDrawer>().lineWidth = 0.15f;
        }
        else
        {
            selectedOrbit = null;
        }

        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,
                                                                Input.mousePosition, cam,
                                                                out Vector3 movePos);

        turretObjectPivot.position = new Vector3(movePos.x, movePos.y, turretObjectPivot.position.z);
    }
}