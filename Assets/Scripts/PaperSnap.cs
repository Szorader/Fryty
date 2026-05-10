using UnityEngine;
public class PaperSnap : MonoBehaviour
{
    private Rigidbody rb;

    private bool isSnapped = false;
    private bool isBeingDestroyed = false;

    private BoardGrid currentBoard;
    private Vector2Int slotIndex;
    
    private TutorialManager tutorialManager;
    private bool tutorialActive = true;
    
    private bool canSnap = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    void Update()
    {
        if (!isSnapped || isBeingDestroyed) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    ReleasePaper();
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (isBeingDestroyed) return;
        if (!CompareTag("OrderTicket")) return;
        if (isSnapped) return;
        if (!canSnap) return;

        BoardGrid board = col.collider.GetComponentInParent<BoardGrid>();
        if (board == null) return;

        Vector3 pos;
        Quaternion rot;
        Vector2Int index;

        if (board.TryGetClosestFreeSlot(transform.position, out pos, out rot, out index))
        {
            Snap(board, pos, rot, index);
        }
    }

    void Snap(BoardGrid board, Vector3 pos, Quaternion rot, Vector2Int index)
    {
        currentBoard = board;
        slotIndex = index;
        isSnapped = true;

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = pos;
        transform.rotation = rot;
        transform.SetParent(board.transform);
        if (tutorialActive && tutorialManager.tutorialStep == 2)
        {
            tutorialManager.NextStep();
            tutorialActive = false;
        }
    }
    
    void ReleasePaper()
    {
        if (!isSnapped || isBeingDestroyed) return;

        isSnapped = false;
        canSnap = false;

        if (currentBoard != null)
        {
            currentBoard.FreeSlot(slotIndex);
            currentBoard = null;
        }

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

        // po chwili znów może snapować (gdy wyleci z obszaru)
        Invoke(nameof(EnableSnap), 0.3f);
    }
    
    void EnableSnap()
    {
        canSnap = true;
    }

    void DestroyPaper()
    {
        if (isBeingDestroyed) return;

        isBeingDestroyed = true;

        if (currentBoard != null)
        {
            currentBoard.FreeSlot(slotIndex);
        }

        Destroy(gameObject);
    }
}