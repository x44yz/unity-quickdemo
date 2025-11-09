using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public Vector2 size;
    public Color boardColor;
    public bool showBoardGizmos;
    public bool showBoardTxt;
    public bool showBoardLines;
    public LineRenderer render;
    public TMP_Text tmpTxtGridPos;

    public int gridCount => width * height;

    private void Awake() 
    {
        if (render == null)
            render = GetComponent<LineRenderer>();

        if (showBoardLines)
            InitGridRender();

        if (showBoardTxt && tmpTxtGridPos != null)
        {
            for (int i = 0; i < gridCount; ++i)
            {
                var txtGridPos = Instantiate(tmpTxtGridPos);
                txtGridPos.gameObject.SetActive(true);
                txtGridPos.transform.SetParent(transform, false);
                txtGridPos.text = $"{(i + 1)}";
                txtGridPos.transform.position = GetWorldPos(i);
            }
            tmpTxtGridPos.gameObject.SetActive(false);
        }

        usedGrids = new List<int>();
    }

    private void OnDestroy() 
    {
        if (render != null)
            render.positionCount = 0;
    }

    public Vector2Int GetGridPos(Vector3 wp)
    {
        Vector3 offset = new Vector3(size.x * -(width / 2.0f - 0.5f), size.y * (height / 2.0f - 0.5f), 0f);
        wp -= transform.position;
        wp -= offset;
        return new Vector2Int(Mathf.RoundToInt(wp.x / size.x), Mathf.RoundToInt(-wp.y / size.y));
    }

    public Vector2Int GetGridPos(int boardIdx)
    {
        int x = boardIdx % width;
        int y = boardIdx / width;
        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPos(Vector3 wp)
    {
        var xy = GetGridPos(wp);
        return GetWorldPos(xy.x, xy.y);
    }

    public Vector3 GetWorldPos(int boardIdx)
    {
        var gpos = GetGridPos(boardIdx);
        return GetWorldPos(gpos.x, gpos.y);
    }

    public Vector3 GetWorldPos(Vector2Int gpos)
    {
        return GetWorldPos(gpos.x, gpos.y);
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        Vector3 offset = new Vector3(size.x * -(width / 2.0f - 0.5f), size.y * (height / 2.0f - 0.5f), 0f);
        return transform.position + offset + new Vector3(x * size.x, -y * size.y, 0);
    }

    public int GetBoardIdx(Vector2Int gpos)
    {
        return gpos.x + gpos.y * width;
    }

    public int GetBoardIdx(int x, int y)
    {
        return x + y * width;
    }

    public int GetBoardIdx(Vector3 wp)
    {
        var gpos = GetGridPos(wp);
        return GetBoardIdx(gpos);
    }

    public Vector3 SnapToGridWorldPos(Vector3 wp)
    {
        var xy = GetGridPos(wp);
        return GetWorldPos(xy.x, xy.y);
    }

    private List<int> usedGrids;
    public Vector2Int GetValidGridPos()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                var bidx = GetBoardIdx(x, y);
                if (usedGrids.Contains(bidx))
                    continue;
                usedGrids.Add(bidx);
                return GetGridPos(bidx); 
            }
        }
        Debug.LogError("cant get valid grid pos");
        return new Vector2Int(-1, 0);
    }

    public void RecycleGridPos(Vector2Int gpos)
    {
        var bidx = GetBoardIdx(gpos);
        if (bidx < 0 || bidx >= width * height)
        {
            Debug.LogError($"cant recycle because grid out of range > {gpos}");
            return;
        }

        if (usedGrids.Contains(bidx) == false)
        {
            Debug.LogError($"cant recycle because no grid > {gpos}");
            return;
        }

        Debug.Log($"recycle grid > {gpos}");
        usedGrids.Remove(bidx);
    }

    private void OnDrawGizmos() 
    {
        if (!showBoardGizmos)
            return;

        Vector3 lt = new Vector3(-size.x * 0.5f, size.y * 0.5f, 0f);
        // Vector3 lb = new Vector3(-cardSize.x * 0.5f, -cardSize.y * 0.5f, 0f);
        // Vector3 rt = new Vector3(-cardSize.x * 0.5f, cardSize.y * 0.5f, 0f);

        Gizmos.color = boardColor;
        for (int i = 0; i < width + 1; ++i)
        {
            var p0 = GetWorldPos(i, 0) + lt;
            var p1 = GetWorldPos(i, height) + lt;
            Gizmos.DrawLine(p0, p1);
        }
        for (int i = 0; i < height + 1; ++i)
        {
            var p0 = GetWorldPos(0, i) + lt;
            var p1 = GetWorldPos(width, i) + lt;
            Gizmos.DrawLine(p0, p1);
        }
    }

    private void InitGridRender()
    {
        if (render == null)
        {
            Debug.LogWarning("cant init grid render because linerenderer is null");
            return;
        }

        Vector3 lt = new Vector3(-size.x * 0.5f, size.y * 0.5f, 0f);
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i <= width; ++i)
        {
            Vector3 bpos = GetWorldPos(i, 0) + lt;
            Vector3 epos = GetWorldPos(i, height) + lt;
            if (i % 2 == 0)
            {
                points.Add(bpos);
                points.Add(epos);
            }
            else
            {
                points.Add(epos);
                points.Add(bpos);
            }
        }

        for (int j = 0; j <= height; ++j)
        {
            Vector3 bpos = GetWorldPos(0, j) + lt;
            Vector3 epos = GetWorldPos(width, j) + lt;
            if (j % 2 == 1)
            {
                points.Add(bpos);
                points.Add(epos);
            }
            else
            {
                points.Add(epos);
                points.Add(bpos);
            }
        }

        render.positionCount = points.Count;
        render.SetPositions(points.ToArray());
        render.Simplify(0);
    }
}
