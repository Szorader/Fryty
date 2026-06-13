using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
   // moves the credits upwards
   
   public float scrollSpeed = 40f;
   public bool inCreddits = false;
   
   private RectTransform rectTransform;
   private Vector2 startPosition;

   /*void Start()
   {
      // get the transform of the UI panel with the credits text
      rectTransform = GetComponent<RectTransform>();
      startPosition = rectTransform.anchoredPosition;
   }*/
   
   void Awake()
   {
      rectTransform = GetComponent<RectTransform>();
      if (rectTransform != null)
         startPosition = rectTransform.anchoredPosition;
   }

   void Update()
   {
      // Move the text upwards over time
      if (inCreddits)
         rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
   }

   public void SetInCreddits(bool value)
   {
      inCreddits = value;

      if (rectTransform == null)
         return;

      if (!value)
      {
         rectTransform.anchoredPosition = startPosition;
      }
   }
}
