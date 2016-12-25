using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuAnimatorScript : MonoBehaviour {
	
	public GameObject[] Items;
    public ToggleGroup ToggleGroup;
    public float DistanceBetweenItems = 0.0f;
	public float AnimationTime = 1f;
	public float TransitionDelay = 0f;
	public int StartItemIndex = 0;
	public LeanTweenType TransitionType;
	public Button LeftButtonObject;
	public Button RightButtonObject;

    Vector2 firstElementPosition;
	float width;
	int actualElement;

	private float XMovement
	{
		get {
			return width + DistanceBetweenItems;
		}
	}
	
	public int ActualElement
	{
		set {
			if(value == 0)
			{
				if(LeftButtonObject != null)
				LeftButtonObject.transform.gameObject.SetActive(false);
			}
			else
			{
				if(LeftButtonObject != null)
				LeftButtonObject.transform.gameObject.SetActive(true);
			}
			
			if(value == (Items.Length-1))
			{
				if(RightButtonObject != null)
				RightButtonObject.transform.gameObject.SetActive(false);

            }
			else
			{
				if(RightButtonObject != null)
				RightButtonObject.transform.gameObject.SetActive(true);

            }
			actualElement = value;
            CheckToggle();
        }
		get {
			return actualElement;
		}
	}
	
	void Start ()
    {
		ChceckIfEverythingSetupCorrect ();

		if (Items.Length > 0 && Items [0] != null) 
		{
			firstElementPosition = Items [0].transform.localPosition;
			var rectTransform = Items [0].GetComponent<RectTransform> ();
			if(rectTransform != null)
			{
				width = rectTransform.rect.width;
			}
		}

		Vector2 newItemPosition = firstElementPosition;
		for (int i = 0; i < Items.Length; i++) 
		{
			if(Items[i] != null)
			{
				Items [i].transform.localPosition = newItemPosition;
				newItemPosition.x = XMovement * (i+1);
			}
		}

		if (RightButtonObject != null) {
			RightButtonObject.onClick.AddListener (MoveRight);

        }
		if (LeftButtonObject != null) {
			LeftButtonObject.onClick.AddListener (MoveLeft);
		}

		SetStartPosition (StartItemIndex);
        CheckToggle();
    }

    protected void CheckToggle()
    {
        Toggle[] v_toggles = ToggleGroup != null? ToggleGroup.GetComponentsInChildren<Toggle>() : new Toggle[0];
        for (int i = 0; i < v_toggles.Length; i++)
        {
            if (v_toggles[i] != null)
                v_toggles[i].isOn = (i == actualElement);
        }
    }

    public void MoveRight()
    {
        if (ActualElement < Items.Length - 1)
        {
            LeanTween.cancelAll(true);
            AnimateLevels(true);
        }
        else
        {
            MoveNeutral();
        }
    }

    public void MoveLeft()
    {
        if (ActualElement > 0)
        {
            LeanTween.cancelAll(true);
            AnimateLevels(false);
        }
        else
        {
            MoveNeutral();
        }
    }

    public void MoveNeutral()
    {
        LeanTween.cancelAll(true);
        AnimateNeutral();

    }



    public void GoBackToInitialInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            if (ActualElement > 0)
            {
                LeanTween.cancelAll(true);
                AnimateLevels(false);
            }
        }
    }

    void AnimateLevels (bool isRight)
	{
		if(isRight) ActualElement++;	                 
		else ActualElement--;

		for (int i = 0; i < Items.Length; i++) {
			if(Items[i] != null && Items [i].GetComponent<RectTransform> ()!= null)
			{
				Vector2 newPosition = CalculateNewPosition(i, ActualElement);
				LeanTween.move (Items [i].GetComponent<RectTransform> (), newPosition, AnimationTime).setDelay (TransitionDelay).setEase (TransitionType);
			}
		}
	}

	Vector2 CalculateNewPosition (int newElementIndex, int actualElementIndex)
	{
		if (Items [newElementIndex] != null) {
			Vector2 newPosition = Items [newElementIndex].transform.localPosition;
			newPosition.x = firstElementPosition.x - XMovement * actualElementIndex + XMovement * newElementIndex;
			return newPosition;
		}
		return new Vector2 ();
	}

	void SetStartPosition (int startItemIndex)
	{
		ActualElement = startItemIndex;

		for (int i = 0; i < Items.Length; i++) {
			if(Items[i] != null)
			{
				Vector2 newPosition = CalculateNewPosition(i, ActualElement);
				Items [i].transform.localPosition = newPosition;
			}
		}
	}

    void AnimateNeutral()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null && Items[i].GetComponent<RectTransform>() != null)
            {
                Vector2 newPosition = CalculateNewPosition(i, ActualElement);
                LeanTween.move(Items[i].GetComponent<RectTransform>(), newPosition, AnimationTime).setDelay(TransitionDelay).setEase(TransitionType);
            }
        }
    }


    void ChceckIfEverythingSetupCorrect ()
	{
		if(AnimationTime < 0)
		{
			Debug.LogException(new UnityException(string.Format("Animation Time can not be less than 0. Your is: {0}", AnimationTime)));
		}
		if(TransitionDelay < 0)
		{
			Debug.LogException(new UnityException(string.Format("Transition Delay can not be less than 0. Your is: {0}", TransitionDelay)));
		}
		if (Items.Length > 0) {
			if(StartItemIndex < 0 || StartItemIndex > Items.Length-1)
			{
				Debug.LogException(new UnityException(string.Format("Start Item Index sould be in following range <{0},{1}>. Your is: {2}", 0, Items.Length-1, StartItemIndex)));
			}
		}
		else
		{
			Debug.LogError(new UnityException(string.Format("You miss add Items references. Put Your Objects in Items array in the inspector.")));
		}

		if(LeftButtonObject == null)
		{	
			//Debug.LogWarning(new UnityException(string.Format("Don't You miss add Left Button reference?")));
		}
		if(RightButtonObject == null)
		{
			//Debug.LogWarning(new UnityException(string.Format("Don't You miss add Right Button reference?")));
		}

		for (int i = 0; i < Items.Length; i++) {
		
			var actualItem = Items[i];
			if(actualItem == null)
			{
				Debug.LogException(new UnityException(string.Format("Nothing is assigned to Item {0}. Put something there or change items count to appriopriate.", i)));
			}

			var itemRectTransform = Items[i].GetComponent<RectTransform>();
			if(itemRectTransform == null)
			{
				Debug.LogException(new UnityException(string.Format("Item {0} is not a UI Panel. Every item should be type of Panel. To do this put Your staff inside a Panel and then change reference in the inspector to this new object.", i)));
			}
		}
	}
}
