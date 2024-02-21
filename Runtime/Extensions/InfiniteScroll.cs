using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[DefaultExecutionOrder(-2)]
public class InfiniteScroll : MonoBehaviour, IDragHandler, IEndDragHandler
{
	enum SnapPosition
	{
		CENTER, TOP, BOTTOM
	}

	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private SnapPosition snapPosition;
	RectTransform Content => scrollRect.content;
	RectTransform Viewport => scrollRect.viewport;

	[Min(1)] public int originalItemsCount = 4;
	public uint clonesPerItem = 3;
	private bool isDragging;
	private void Start()
	{
		scrollRect.verticalNormalizedPosition = 0.5f;
	}

	private void LateUpdate()
	{
		if (Content.rect.height < Viewport.rect.height || originalItemsCount <= 0) return;

		var velocity = scrollRect.velocity;

		float itemProportion = Content.rect.height / Content.childCount / Content.rect.height;

		float threshold = originalItemsCount * itemProportion;

		float target = scrollRect.verticalNormalizedPosition;

		//Infinite Scroll

		if (target < threshold)
		{
			target += threshold;
		}
		else if (target > 1 - threshold)
		{
			target -= threshold;
		}
		//Snap
		if (Mathf.Abs(velocity.y) <= 50 && !isDragging)
		{
			target = Mathf.Lerp(target, target + (target % (itemProportion / 2f)) * 0.5f * (velocity.y > 0 ? 1 : -1), Time.deltaTime * 10);
		}

		scrollRect.verticalNormalizedPosition = target;

		scrollRect.velocity = Mathf.Abs(velocity.y) > 50 ? velocity : Vector2.zero;
	}

	public void OnDrag(PointerEventData eventData)
	{
		isDragging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}
}