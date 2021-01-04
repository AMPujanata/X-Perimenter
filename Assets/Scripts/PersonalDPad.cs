using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PersonalDPad : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	public int xAxis;
	public int yAxis;

#pragma warning disable 0649
	[Tooltip("Radius of the deadzone at the center of the Dpad that will yield no input")]
	[SerializeField]
	private float deadzoneRadius = 20f;
	private float deadzoneRadiusSqr;
#pragma warning restore 0649

	private RectTransform rectTransform;


	private void Awake()
	{
		rectTransform = (RectTransform)transform;

		deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
	}
#if UNITY_EDITOR
	private void OnValidate()
	{
		deadzoneRadiusSqr = deadzoneRadius * deadzoneRadius;
	}
#endif

	public void OnPointerDown(PointerEventData eventData)
	{
		CalculateInput(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		CalculateInput(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		ResetInput();
	}

	public void ResetInput()
    {
		xAxis = 0;
		yAxis = 0;
	}

	public int GetHorizontalInput()
    {
		return xAxis;
    }

	public int GetVerticalInput()
    {
		return yAxis;
    }

	private void CalculateInput(PointerEventData eventData)
	{
		Vector2 pointerPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pointerPos);

		if (pointerPos.sqrMagnitude <= deadzoneRadiusSqr)
		{
			xAxis = 0;
			yAxis = 0;
		}
		else
		{
			float angle = Vector2.Angle(pointerPos, Vector2.right);
			if (pointerPos.y < 0f)
				angle = 360f - angle;

			if (angle >= 45f && angle <= 135f)
				yAxis = 1;
			else if (angle >= 225f && angle <= 315f)
				yAxis = -1;
			else
				yAxis = 0;

			if (angle <= 45f || angle >= 315f)
				xAxis = 1;
			else if (angle >= 135f && angle <= 225f)
				xAxis = -1;
			else
				xAxis = 0;
			/*if (pointerPos.y < 0f)
				angle = 360f - angle;

			if (angle >= 25f && angle <= 155f)
				yAxis = 1;
			else if (angle >= 205f && angle <= 335f)
				yAxis = -1;
			else
				yAxis = 0;

			if (angle <= 65f || angle >= 295f)
				xAxis = 1;
			else if (angle >= 115f && angle <= 245f)
				xAxis = -1;
			else
				xAxis = 0;*/
		}
	}
}