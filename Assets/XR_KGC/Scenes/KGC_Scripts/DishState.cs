using UnityEngine;

public class DishState : MonoBehaviour
{
    public SpriteRenderer sr;
    public bool clearDish;

    public int mealCount = 1; //¹ä ¾ç

    public Sprite[] dish = new Sprite[2];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mealCount <= 0)
        {
            clearDish = true;
        }
        else
        {
            clearDish = false;
        }


        if (clearDish)
        {
            sr.sprite = dish[0];
        }
        else if (!clearDish)
        {
            sr.sprite = dish[1];
        }
    }

    public void Consume()
    {
        mealCount -= 1;
    }

    public void ChargeMeal()
    {
        if (mealCount < 1)
        {
        mealCount += 1;
        }
        else
        {
            //¹äÀÌ °¡µæÂ÷ÀÖ´Ù°í ¾Ë·ÁÁÖ±â
        }
    }

}
