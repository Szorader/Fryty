using UnityEngine;

public class sexykon : MonoBehaviour
{
    [SerializeField] private DeathScreenManager deathScreen;
    public FMOD.Studio.EventInstance footstepInstance;

    private bool hasKilled = false;

    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();

        if (player == null) return;

        hasKilled = true;

        player.Die(); // to stop footsteps

        deathScreen.ShowDeath(player.gameObject);
    }
}
