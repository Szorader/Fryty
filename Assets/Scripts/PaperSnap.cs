using UnityEngine;
public class PaperSnap : MonoBehaviour
{
    private Rigidbody rb;

    private bool isSnapped = false;
    private bool isBeingDestroyed = false;

    private BoardGrid currentBoard;
    private Vector2Int slotIndex;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
                    DestroyPaper();
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (isBeingDestroyed) return;

        if (!CompareTag("OrderTicket")) return;
        if (isSnapped) return;

        BoardGrid board = col.collider.GetComponentInParent<BoardGrid>();
        if (board == null) return;

        if (board.TryGetFreeSlot(out Vector3 pos, out Quaternion rot, out Vector2Int index))
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