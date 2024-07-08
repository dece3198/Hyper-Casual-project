using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static JoyStick instance;
    [SerializeField] private RectTransform backGround;
    [SerializeField] private RectTransform joyStick;
    [SerializeField] private GameObject player;
    public float moveSpeed;

    private float radius;
    private bool isTouch = false;
    private Vector3 movePosition;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        radius = backGround.rect.width * 0.25f;
    }

    private void Update()
    {
        if(isTouch)
        {
            player.transform.position += movePosition;
            if(movePosition.magnitude > 0)
            {
                player.GetComponent<PlayerController>().animator.SetBool("Run", true);
            }
        }
        else
        {
            player.GetComponent<PlayerController>().animator.SetBool("Run", false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)backGround.position;
        value = Vector2.ClampMagnitude(value, radius);
        joyStick.localPosition = value;

        float distance = Vector2.Distance(backGround.position, joyStick.position) / radius;

        value = value.normalized;
        movePosition = new Vector3(value.x * moveSpeed * distance * Time.deltaTime,0f , value.y * moveSpeed * distance * Time.deltaTime);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.LookRotation(movePosition), Time.deltaTime * 3);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backGround.gameObject.SetActive(true);
        backGround.transform.position = eventData.position;
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        backGround.gameObject.SetActive(false);
        joyStick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
    }

}
