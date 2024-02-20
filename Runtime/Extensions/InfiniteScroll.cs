using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
	[SerializeField] private ScrollRect scrollRect;
	RectTransform Content => scrollRect.content;
	RectTransform Viewport => scrollRect.viewport;

	private readonly List<RectTransform> originalItems = new();
	private readonly List<RectTransform> duplicatedItems = new();
	private void Start()
	{
		// Initialize();
	}

	private void Initialize()
	{
		int count = Content.childCount;
		for (int i = 0; i < count; i++)
		{
			RectTransform item = (RectTransform)Content.GetChild(i);
			if (item.gameObject.activeSelf)
				originalItems.Add(item);
		}
		for (int i = 0; i < 4; i++)
		{
			DuplicateItems();
		}
		scrollRect.verticalNormalizedPosition = 0.5f;
	}
	private void DuplicateItems()
	{
		for (int i = 0; i < originalItems.Count; i++)
		{
			RectTransform item = originalItems[i];
			duplicatedItems.Add(Instantiate(item, Content));
		}
	}

	private void LateUpdate()
	{
		if (Content.rect.height < Viewport.rect.height) return;
		
		var velocity = scrollRect.velocity;

		const float THRESHOLD = 0.125f;
		const float INCREMENT = 0.25f;

		if (scrollRect.verticalNormalizedPosition < THRESHOLD)
		{
			scrollRect.verticalNormalizedPosition += INCREMENT;
		}
		if (scrollRect.verticalNormalizedPosition > 1 - THRESHOLD)
		{
			scrollRect.verticalNormalizedPosition -= INCREMENT;
		}

		scrollRect.velocity = velocity;
	}
}