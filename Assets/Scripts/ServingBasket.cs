using UnityEngine;

public class ServingBasket : MonoBehaviour
{
    [Header("Frytka")]
    [SerializeField] private GameObject fryInBasket;

    private void Start()
    {
        // Na start ukryj frytkę w koszyku
        if (fryInBasket != null)
        {
            fryInBasket.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriedFry"))
        {
            // Pokazuje frytkę w koszyku
            if (fryInBasket != null)
            {
                fryInBasket.SetActive(true);
                Destroy(other.gameObject);
            }

            // Usuwa przeciąganą frytkę
            //
        }
    }
}