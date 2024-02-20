using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-2)]
public class InfiniteScroll : MonoBehaviour
{
	[SerializeField] private ScrollRect scrollRect;
	RectTransform Content => scrollRect.content;
	RectTransform Viewport => scrollRect.viewport;

	[Min(1)] public int originalItemsCount = 4;
	public uint clonesPerItem = 3;
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
		if (Mathf.Abs(velocity.y) <= 4)
		{
			target = Mathf.Lerp(target, target - target % itemProportion * 0.75f, Time.deltaTime * 5);
		}

		scrollRect.verticalNormalizedPosition = target;

		scrollRect.velocity = Mathf.Abs(velocity.y) > 4 ? velocity : Vector2.zero;
	}
}