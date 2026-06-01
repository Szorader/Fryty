using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
   // moves the credits upwards
   
   public float scrollSpeed = 40f;
   
   private RectTransform rectTransform;

   void Start()
   {
      // get the transform of the UI panel with the credits text
      
      rectTransform = GetComponent<RectTransform>();
   }

   void Update()
   {
      // Move the text upwards over time
      rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
   }
}
