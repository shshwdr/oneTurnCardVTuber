using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class CardVisual : MonoBehaviour
{
    private bool initalize = false;

    [FormerlySerializedAs("parentCard")] [Header("Card")]
    public CardSlot parentCardSlot;
    private Transform cardTransform;
    private Vector3 rotationDelta;
    private int savedIndex;
    Vector3 movementDelta;
    private Canvas canvas;

    [Header("References")]
    public Transform visualShadow;
    private float shadowOffset = 20;
    private Vector2 shadowDistance;
    private Canvas shadowCanvas;
    [SerializeField] private Transform shakeParent;
    [SerializeField] private Transform tiltParent;
    [SerializeField] private Image cardImage;

    [Header("Follow Parameters")]
    [SerializeField] private float followSpeed = 30;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAmount = 20;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float autoTiltAmount = 30;
    [SerializeField] private float manualTiltAmount = 20;
    [SerializeField] private float tiltSpeed = 20;

    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = .15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    [Header("Select Parameters")]
    [SerializeField] private float selectPunchAmount = 20;

    [Header("Hober Parameters")]
    [SerializeField] private float hoverPunchAngle = 5;
    [SerializeField] private float hoverTransition = .15f;

    [Header("Swap Parameters")]
    [SerializeField] private bool swapAnimations = true;
    [SerializeField] private float swapRotationAngle = 30;
    [SerializeField] private float swapTransition = .15f;
    [SerializeField] private int swapVibrato = 5;

    [Header("Curve")]
    [SerializeField] private CurveParameters curve;

    private float curveYOffset;
    private float curveRotationOffset;
    private Coroutine pressCoroutine;

    private void Start()
    {
        shadowDistance = visualShadow.localPosition;
    }

    public void Initialize(CardSlot target, int index = 0)
    {
        //Declarations
        parentCardSlot = target;
        cardTransform = target.transform;
        canvas = GetComponent<Canvas>();
        shadowCanvas = visualShadow.GetComponent<Canvas>();

        //Event Listening
        parentCardSlot.PointerEnterEvent.AddListener(PointerEnter);
        parentCardSlot.PointerExitEvent.AddListener(PointerExit);
        parentCardSlot.BeginDragEvent.AddListener(BeginDrag);
        parentCardSlot.EndDragEvent.AddListener(EndDrag);
        parentCardSlot.PointerDownEvent.AddListener(PointerDown);
        parentCardSlot.PointerUpEvent.AddListener(PointerUp);
        parentCardSlot.SelectEvent.AddListener(Select);

        //Initialization
        initalize = true;
    }

    public void UpdateIndex(int length)
    {
        transform.SetSiblingIndex(parentCardSlot.transform.parent.GetSiblingIndex());
    }

    void Update()
    {
        if (!initalize || parentCardSlot == null)
        {
            return;
        }

        HandPositioning();
        SmoothFollow();
        FollowRotation();
        CardTilt();

    }

    private void HandPositioning()
    {
        // curveYOffset = (curve.positioning.Evaluate(parentCard.NormalizedPosition()) * curve.positioningInfluence) * parentCard.SiblingAmount();
        // curveYOffset = parentCard.SiblingAmount() < 5 ? 0 : curveYOffset;
        // curveRotationOffset = curve.rotation.Evaluate(parentCard.NormalizedPosition());
    }

    private void SmoothFollow()
    {
        Vector3 verticalOffset = (Vector3.up * (parentCardSlot.isDragging ? 0 : curveYOffset));
        transform.position = Vector3.Lerp(transform.position, cardTransform.position + verticalOffset, followSpeed * Time.deltaTime);
    }

    private void FollowRotation()
    {
        Vector3 movement = (transform.position - cardTransform.position);
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (parentCardSlot.isDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    private void CardTilt()
    {
        savedIndex = parentCardSlot.isDragging ? savedIndex : parentCardSlot.ParentIndex();
        float sine = Mathf.Sin(Time.time + savedIndex) * (parentCardSlot.isHovering ? .2f : 1);
        float cosine = Mathf.Cos(Time.time + savedIndex) * (parentCardSlot.isHovering ? .2f : 1);

        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float tiltX = parentCardSlot.isHovering ? ((offset.y * -1) * manualTiltAmount) : 0;
        float tiltY = parentCardSlot.isHovering ? ((offset.x) * manualTiltAmount) : 0;
        float tiltZ = parentCardSlot.isDragging ? tiltParent.eulerAngles.z : (curveRotationOffset * (curve.rotationInfluence * parentCardSlot.SiblingAmount()));

        float lerpX = Mathf.LerpAngle(tiltParent.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(tiltParent.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

        tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    private void Select(CardSlot cardSlot, bool state)
    {
        DOTween.Kill(2, true);
        float dir = state ? 1 : 0;
        shakeParent.DOPunchPosition(shakeParent.up * selectPunchAmount * dir, scaleTransition, 10, 1);
        shakeParent.DOPunchRotation(Vector3.forward * (hoverPunchAngle/2), hoverTransition, 20, 1).SetId(2);

        if(scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

    }

    public void Swap(float dir = 1)
    {
        if (!swapAnimations)
            return;

        DOTween.Kill(2, true);
        shakeParent.DOPunchRotation((Vector3.forward * swapRotationAngle) * dir, swapTransition, swapVibrato, 1).SetId(3);
    }

    private void BeginDrag(CardSlot cardSlot)
    {
        if(scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

        canvas.overrideSorting = true;
    }

    private void EndDrag(CardSlot cardSlot)
    {
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        canvas.overrideSorting = false;
    }

    private void PointerEnter(CardSlot cardSlot)
    {
        if(scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
        
        DOTween.Kill(2, true);
        shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
        
        canvas.overrideSorting = true;
    }

    private void PointerExit(CardSlot cardSlot)
    {
        if (!parentCardSlot.wasDragged)
            transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        
        
        canvas.overrideSorting = false;
    }

    private void PointerUp(CardSlot cardSlot, bool longPress)
    {
        // if(scaleAnimations)
        //     transform.DOScale(longPress ? scaleOnHover : scaleOnSelect, scaleTransition).SetEase(scaleEase);
        // canvas.overrideSorting = false;
        //
        // visualShadow.localPosition = shadowDistance;
        // shadowCanvas.overrideSorting = true;
    }

    private void PointerDown(CardSlot cardSlot)
    {
        // if(scaleAnimations)
        //     transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);
        //     
        // visualShadow.localPosition += (-Vector3.up * shadowOffset);
        // shadowCanvas.overrideSorting = false;
    }

}
