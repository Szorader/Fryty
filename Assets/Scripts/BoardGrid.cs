using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(5, 5);

    [Header("Spacing (real distance between cards)")]
    public float spacingX = 0.25f;
    public float spacingY = 0.35f;

    public Transform origin;

    private Transform[,] slots;
    private bool[,] occupied;

    void Awake()
    {
        slots = new Transform[gridSize.x, gridSize.y];
        occupied = new bool[gridSize.x, gridSize.y];

        Vector3 basePos = origin != null ? origin.position : transform.position;

        Vector3 startOffset = new Vector3(
            -((gridSize.x - 1) * spacingX) / 2f,
            -((gridSize.y - 1) * spacingY) / 2f,
            0
        );

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject slot = new GameObject($"Slot_{x}_{y}");
                slot.transform.parent = transform;

                Vector3 offset = new Vector3(
                    x * spacingX,
                    y * spacingY,
                    0
                );

                slot.transform.position = basePos + startOffset + offset;
                slot.transform.rotation = transform.rotation;

                slots[x, y] = slot.transform;
                occupied[x, y] = false;
            }
        }
    }

    // NOWA METODA — NAJBLIŻSZY WOLNY SLOT
    public bool TryGetClosestFreeSlot(Vector3 fromPosition, out Vector3 pos, out Quaternion rot, out Vector2Int index)
    {
        float bestDistance = float.MaxValue;
        Vector2Int bestIndex = new Vector2Int(-1, -1);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (occupied[x, y]) continue;

                float dist = Vector3.SqrMagnitude(slots[x, y].position - fromPosition);

                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    bestIndex = new Vector2Int(x, y);
                }
            }
        }

        if (bestIndex.x != -1)
        {
            occupied[bestIndex.x, bestIndex.y] = true;

            pos = slots[bestIndex.x, bestIndex.y].position;
            rot = slots[bestIndex.x, bestIndex.y].rotation;
            index = bestIndex;

            return true;
        }

        pos = Vector3.zero;
        rot = Quaternion.identity;
        index = new Vector2Int(-1, -1);

        return false;
    }

    // STARA METODA — jeśli nadal potrzebna
    public bool TryGetFreeSlot(out Vector3 pos, out Quaternion rot, out Vector2Int index)
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                if (!occupied[x, y])
                {
                    occupied[x, y] = true;

                    pos = slots[x, y].position;
                    rot = slots[x, y].rotation;
                    index = new Vector2Int(x, y);

                    return true;
                }
            }
        }

        pos = Vector3.zero;
        rot = Quaternion.identity;
        index = new Vector2Int(-1, -1);

        return false;
    }

    public void FreeSlot(Vector2Int index)
    {
        if (index.x >= 0 && index.y >= 0 &&
            index.x < gridSize.x && index.y < gridSize.y)
        {
            occupied[index.x, index.y] = false;
        }
    }
}