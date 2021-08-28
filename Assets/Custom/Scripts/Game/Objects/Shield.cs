using FXV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class Shield : MonoBehaviour
{ 

    private MeshFilter mFilter;
    private Renderer mRenderer;
    private Collider coll;
    private Rigidbody rb;

    [ShowOnly]
    public bool shieldActive = true;
    public bool autoHitPatternScale = true;

    private Material baseMaterial;
    private Material activationMaterial;

    private float shieldActivationTime;
    private float shieldActivationDir;
    private float shieldActivationRim = 0.2f;

    //Property IDs
    private readonly int activationTimeProperty = Shader.PropertyToID("_ActivationTime");
    private readonly int shieldDirectionProperty = Shader.PropertyToID("_ShieldDirection");
    private readonly int mainColorProperty = Shader.PropertyToID("_Color");
    private readonly int texColorProperty = Shader.PropertyToID("_TextureColor");
    private readonly int patternColorProperty = Shader.PropertyToID("_PatternColor");

    private int currentHitIndex = 1;

    private bool initialized = false;

    [SerializeField]
    private LayerMask collidingLayerMask;

    [SerializeField]
    private ShieldSO _shieldScriptableObject;

    [SerializeField]
    private ShieldData _shieldData;

    public ShieldSO ShieldSO
    {
        get
        {
            return _shieldScriptableObject;
        }
        set
        {
            _shieldScriptableObject = value;
            if (_shieldScriptableObject != null)
            {
                _shieldData = _shieldScriptableObject.Data;
            }
            else
            {
                _shieldData = null;
            }
            if(initialized)
            {
                this.SetShieldData(_shieldData);
            }
        }
    }

    void Awake()
    {
        // Initialize basic shield components
        this.InitializeComponents();

        shieldActivationDir = 0.0f;

        if (shieldActive)
        {
            shieldActivationTime = 1.0f;
            coll.enabled = true;
        }
        else
        {
            shieldActivationTime = 0.0f;
            coll.enabled = false;
        }
        initialized = true;

        this.SetShieldData(_shieldData);
    }

    void Update()
    {
        if (mRenderer.sharedMaterial != null)
        {
            if (shieldActivationDir > 0.0f)
            {
                shieldActivationTime += _shieldData.shieldActivationSpeed * Time.deltaTime;
                if (shieldActivationTime >= 1.0f)
                {
                    shieldActivationTime = 1.0f;
                    shieldActivationDir = 0.0f;
                    mRenderer.sharedMaterial = baseMaterial;
                }
            }
            else if (shieldActivationDir < 0.0f)
            {
                shieldActivationTime -= _shieldData.shieldActivationSpeed * Time.deltaTime;
                if (shieldActivationTime <= -shieldActivationRim)
                {
                    shieldActivationTime = -shieldActivationRim;
                    shieldActivationDir = 0.0f;
                    mRenderer.enabled = false;
                    mRenderer.sharedMaterial = baseMaterial;
                }
            }
            mRenderer.sharedMaterial.SetFloat(activationTimeProperty, shieldActivationTime);

        }
    }

    void OnValidate()
    {
        ShieldSO = _shieldScriptableObject;
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (baseMaterial)
                Destroy(baseMaterial);
            if (activationMaterial)
                Destroy(activationMaterial);
        }
        else
        {
            if (baseMaterial)
                DestroyImmediate(baseMaterial);
            if (activationMaterial)
                DestroyImmediate(activationMaterial);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if object is in collision LayerMask
        if (collidingLayerMask.Contains(other.gameObject.layer))
        {
            // Handles collisions with IHasHealth scripts;
            this.HandleHasHealthCollision(other.gameObject);

            if (!this.GetIsDuringActivationAnim())
            {
                this.OnHit(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), 2f);
            }
        }
    }

    void InitializeComponents()
    {
        mFilter = GetComponent<MeshFilter>();
        mRenderer = GetComponent<MeshRenderer>();
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        if (mFilter == null)
        {
            mFilter = this.gameObject.AddComponent<MeshFilter>();
        }

        if (mRenderer == null)
        {
            mRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }

        if (coll == null)
        {
            coll = this.gameObject.AddComponent<SphereCollider>();
        }

        if (rb == null)
        {
            rb = this.gameObject.AddComponent<Rigidbody>();
        }
        mRenderer.material = null;
        mRenderer.shadowCastingMode = ShadowCastingMode.Off;
        coll.isTrigger = true;
        rb.isKinematic = true;
    }

    private void SetShieldData(ShieldData data)
    {
        if (data != null)
        {
            if (data.shieldMesh != null)
            {
                mFilter.mesh = data.shieldMesh;
            }
            this.SetShieldActive(false, false);
            this.SetMaterial(data.shieldMaterial);
            this.SetShieldActive(true);
        }
        else
        {
            if (mFilter != null)
            {
                mFilter.mesh = null;
            }

            if (mRenderer != null)
            {
                mRenderer.material = null;
            }
        }
    }

    private void HandleHasHealthCollision(GameObject obj)
    {
        List<IHasHealth> interfaceList = obj.GetInterfaces<IHasHealth>();

        foreach (IHasHealth hasHealth in interfaceList)
        {
            hasHealth.DecreaseHealth(_shieldData.shieldDamageOnHit);
        }
    }

    #region

    public void SetMaterial(Material newMat)
    {
        if (baseMaterial)
            DestroyImmediate(baseMaterial);
        if (activationMaterial)
            DestroyImmediate(activationMaterial);

        if (newMat == null) { return; };

        baseMaterial = new Material(newMat);
        baseMaterial.SetFloat(activationTimeProperty, 1.0f);
        activationMaterial = new Material(baseMaterial);
        shieldActivationRim = activationMaterial.GetFloat("_ActivationRim");

        List<string> keywords = new List<string>(baseMaterial.shaderKeywords);
        if (keywords.Contains("ACTIVATION_EFFECT_ON"))
        {
            keywords.Remove("ACTIVATION_EFFECT_ON");
        }
        baseMaterial.shaderKeywords = keywords.ToArray();
        mRenderer.sharedMaterial = baseMaterial;

        SetShieldEffectDirection(new Vector3(1.0f, 0.0f, 0.0f));
    }

    public void SetMainColor(Color c)
    {
        activationMaterial.color = c;
        baseMaterial.color = c;
        mRenderer.sharedMaterial.color = c;
    }

    public void SetTextureColor(Color c)
    {
        activationMaterial.SetColor(texColorProperty, c);
        baseMaterial.SetColor(texColorProperty, c);
        mRenderer.sharedMaterial.SetColor(texColorProperty, c);
    }

    public void SetPatternColor(Color c)
    {
        activationMaterial.SetColor(patternColorProperty, c);
        baseMaterial.SetColor(patternColorProperty, c);
        mRenderer.sharedMaterial.SetColor(patternColorProperty, c);
    }

    public bool GetIsShieldActive()
    {
        return (shieldActivationTime == 1.0f) || (shieldActivationDir == 1.0f);
    }

    public bool GetIsDuringActivationAnim()
    {
        return shieldActivationDir != 0.0f;
    }

    public void SetShieldActive(bool active, bool animated = true)
    {
        if (animated)
        {
            shieldActivationDir = (active) ? 1.0f : -1.0f;
            if (activationMaterial)
            {
                activationMaterial.SetFloat("_ActivationRim", shieldActivationRim);
                activationMaterial.SetFloat(activationTimeProperty, shieldActivationTime);

                mRenderer.sharedMaterial = activationMaterial;
            }
            mRenderer.enabled = active;

        }
        else
        {
            shieldActivationTime = (active) ? 1.0f : 0.0f;
            shieldActivationDir = 0.0f;
        }
        shieldActive = active;

        if (coll != null)
        {
            coll.enabled = active;
        }
    }

    public void SetShieldEffectDirection(Vector3 dir)
    {
        Vector4 dir4 = new Vector4(dir.x, dir.y, dir.z, 0.0f);
        mRenderer.sharedMaterial.SetVector(shieldDirectionProperty, dir4);
        baseMaterial.SetVector(shieldDirectionProperty, dir4);
        activationMaterial.SetVector(shieldDirectionProperty, dir4);
    }

    public void OnHit(Vector3 hitPos, float hitScale)
    {
        AddHitMeshAtPos(gameObject.GetComponent<MeshFilter>().mesh, hitPos, hitScale);
        GameManager.Instance.simulationData.shieldHitsLeft = Mathf.Clamp(GameManager.Instance.simulationData.shieldHitsLeft - 1, 0, GameManager.Instance.simulationData.shieldHitsLeft);

        if(GameManager.Instance.simulationData.shieldHitsLeft == 0 && shieldActive)
        {
            StartCoroutine(DeactivateAfterTime(_shieldData.shieldHitEffectDuration));
        }
    }

    private void AddHitMeshAtPos(Mesh mesh, Vector3 hitPos, float hitScale)
    {
        if (_shieldData.shieldHitMaterial != null)
        {
            GameObject hitObject = new GameObject("hitFX");
            hitObject.transform.parent = transform;
            hitObject.transform.position = transform.position;
            hitObject.transform.localScale = Vector3.one;
            hitObject.transform.rotation = transform.rotation;

            Vector3 hitLocalSpace = transform.InverseTransformPoint(hitPos);

            Vector3 dir = hitLocalSpace.normalized;
            Vector3 tan1 = Vector3.up - dir * Vector3.Dot(dir, Vector3.up);
            tan1.Normalize();
            Vector3 tan2 = Vector3.Cross(dir, tan1);

            MeshRenderer mr = hitObject.AddComponent<MeshRenderer>();
            MeshFilter mf = hitObject.AddComponent<MeshFilter>();
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mf.mesh = mesh;
            mr.material = new Material(_shieldData.shieldHitMaterial);

            mr.material.SetVector("_HitPos", hitLocalSpace);
            mr.material.SetVector("_HitTan1", tan1);
            mr.material.SetVector("_HitTan2", tan2);
            mr.material.SetFloat("_HitRadius", hitScale);
            mr.material.SetVector("_WorldScale", transform.lossyScale);
            mr.material.SetFloat("_HitShieldCovering", 1.0f);
            mr.material.renderQueue = mr.material.renderQueue + currentHitIndex;

            if (autoHitPatternScale)
            {
                if (mRenderer.material.HasProperty("_PatternScale"))
                    mr.material.SetFloat("_PatternScale", mRenderer.material.GetFloat("_PatternScale"));
                else
                    autoHitPatternScale = false;
            }
            mr.material.color = _shieldData.shieldHitColor;

            FXVShieldHit hit = hitObject.AddComponent<FXVShieldHit>();
            hit.StartHitFX(_shieldData.shieldHitEffectDuration);

            currentHitIndex++;
            if (currentHitIndex > 100)
                currentHitIndex = 1;
        }
    }

    public IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.SetShieldActive(true);
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.SetShieldActive(false);
    }

    #endregion
}