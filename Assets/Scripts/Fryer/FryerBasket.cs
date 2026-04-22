using UnityEngine;

public class FryerSystem : MonoBehaviour
{
    [Header("TYPE")]
    public OrderDatabase.FriesType friesType = OrderDatabase.FriesType.None;

    [Header("COOKING")]
    [Range(0, 4)] public int cookLevel = 0;
    public string cookDes;

    [Header("MODELS")]
    public GameObject straightModel;
    public GameObject crinkleModel;
    public GameObject wedgesModel;

    [Header("COOK MATERIALS")]
    public Material[] cookMaterials;

    [Header("MOVE")]
    public float downOffset = -0.2f;
    public float moveSpeed = 5f;

    [Header("FRYING")]
    public float cookInterval = 2f;

    [Header("SHOOT")]
    public Transform shootPoint;
    public GameObject friesPrefab;
    public float shootForce = 5f;
    public Vector3 shootDirection = Vector3.forward;

    private Vector3 startPos;
    public bool hasFries;
    private float cookTimer;

    public bool returning;
    private bool readyToShoot;

    private GameObject activeModel;
    
    private TutorialManager tutorialManager;
    private bool tutorialActive = true;

    private void Awake()
    {
        startPos = transform.localPosition;
        RefreshVisuals();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void Update()
    {
        HandleMovement();

        if (hasFries && !returning)
            HandleCooking();
    }

    private void HandleMovement()
    {
        Vector3 target = startPos;

        if (hasFries && !returning)
            target = startPos + new Vector3(0, 0, downOffset);

        transform.localPosition =
            Vector3.Lerp(transform.localPosition, target, Time.deltaTime * moveSpeed);
    }

    private void HandleCooking()
    {
        if (cookLevel >= 4) return;

        cookTimer += Time.deltaTime;

        if (cookTimer >= cookInterval)
        {
            cookTimer = 0f;
            cookLevel++;
            RefreshVisuals();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFries) return;

        FriesData fries = other.GetComponentInParent<FriesData>();
        if (!fries) return;

        if (fries.isFried) return;

        SetFries(fries.friesType, fries.cookLevel);

        fries.gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        if (!hasFries) return;

        returning = true;
        hasFries = false;
    }

    private void LateUpdate()
    {
        if (!returning) return;

        if (Vector3.Distance(transform.localPosition, startPos) < 0.01f)
        {
            transform.localPosition = startPos;
            returning = false;

            Shoot();
        }
    }

    public void SetFries(OrderDatabase.FriesType type, int cook)
    {
        friesType = type;
        cookLevel = cook;

        hasFries = true;
        cookTimer = 0f;

        RefreshVisuals();
    }

    public void Clear()
    {
        friesType = OrderDatabase.FriesType.None;
        cookLevel = 0;
        hasFries = false;
        cookTimer = 0f;

        RefreshVisuals();
    }

    private void Shoot()
    {
        if (!friesPrefab) return;

        GameObject obj = Instantiate(
            friesPrefab,
            shootPoint ? shootPoint.position : transform.position,
            Quaternion.identity
        );

        if (tutorialActive && tutorialManager.tutorialStep == 3)
        {
            tutorialManager.NextStep();
            tutorialActive = false;
        }
        
        FriesData data = obj.GetComponent<FriesData>();
        if (data)
        {
            data.SetFriesType(friesType);
            data.cookLevel = cookLevel;
            data.isFried = true;
            data.RefreshVisuals();
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            Vector3 dir = shootPoint ? shootPoint.forward : transform.TransformDirection(shootDirection);
            rb.AddForce(dir.normalized * shootForce, ForceMode.Impulse);
        }

        Clear();
    }

    private void OnValidate()
    {
        RefreshVisuals();
    }

    public void RefreshVisuals()
    {
        UpdateText();
        UpdateModel();
        UpdateMaterial();
    }

    private void UpdateText()
    {
        cookDes = cookLevel switch
        {
            0 => "Raw",
            1 => "Light",
            2 => "Perfect",
            3 => "Over",
            4 => "Burnt",
            _ => "?"
        };
    }

    private void UpdateModel()
    {
        if (straightModel) straightModel.SetActive(false);
        if (crinkleModel) crinkleModel.SetActive(false);
        if (wedgesModel) wedgesModel.SetActive(false);

        activeModel = friesType switch
        {
            OrderDatabase.FriesType.Straight => straightModel,
            OrderDatabase.FriesType.Crinkle => crinkleModel,
            OrderDatabase.FriesType.Wedges => wedgesModel,
            _ => null
        };

        if (activeModel)
            activeModel.SetActive(true);
    }

    private void UpdateMaterial()
    {
        if (!activeModel || cookMaterials == null || cookMaterials.Length == 0)
            return;

        int index = Mathf.Clamp(cookLevel, 0, cookMaterials.Length - 1);
        Material mat = cookMaterials[index];

        foreach (var r in activeModel.GetComponentsInChildren<Renderer>())
            r.sharedMaterial = mat;
    }
}