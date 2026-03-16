using UnityEngine;
using System.Collections;

public class deployableItem : CAVE2Interactable
{
    [Header("Grab Settings")]
    [SerializeField] CAVE2.Button grabButton = CAVE2.Button.Button3;
    [SerializeField] float shrinkScale = 0.2f;
    [SerializeField] float shrinkDuration = 0.25f;
    [SerializeField] Vector3 heldOffsetOnWand = new Vector3(-0.2f, -0.05f, 0.1f);

    [Header("Rotation Settings")]
    [SerializeField] bool enableRotation = true;
    [SerializeField] Vector3 rotationAxis = Vector3.up;
    [SerializeField] float rotationSpeed = 90f;

    public bool IsGrabbed { get; private set; } = false;
    private Vector3 originalScale;
    private Collider itemCollider;

    private Vector3 localCenterOffset;

    private void Start()
    {
        originalScale = transform.localScale;
        itemCollider = GetComponent<Collider>();
        CalculateGroupCenter();
    }

    private void Update()
    {
        UpdateWandOverTimer();

        if (enableRotation)
        {
            Vector3 currentWorldCenter = transform.TransformPoint(localCenterOffset);
            Vector3 currentWorldAxis = transform.TransformDirection(rotationAxis);
            transform.RotateAround(currentWorldCenter, currentWorldAxis, rotationSpeed * Time.deltaTime);
        }
    }

    void CalculateGroupCenter()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            localCenterOffset = Vector3.zero;
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        localCenterOffset = transform.InverseTransformPoint(bounds.center);
    }

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == grabButton && !IsGrabbed)
        {
            Transform grabbingWand = CAVE2.GetWandObject(evt.wandID).transform;
            GrabObject(grabbingWand);
        }
    }

    void GrabObject(Transform wand)
    {
        IsGrabbed = true;

        if (itemCollider != null) itemCollider.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        transform.SetParent(wand);
        transform.localPosition = heldOffsetOnWand - localCenterOffset;
        StartCoroutine(ShrinkObjectSmoothly(originalScale * shrinkScale));
    }

    IEnumerator ShrinkObjectSmoothly(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0;
        while (elapsedTime < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    public void Consume()
    {
        Destroy(gameObject);
    }
}