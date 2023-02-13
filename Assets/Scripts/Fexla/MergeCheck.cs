using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeCheck : MonoBehaviour
{

    private Vector2[] checkPoints =  {
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(-0.5f, -0.5f),
        };

    private int[] checkPointDirection = {
            0b1010,
            0b0110,
            0b0101,
            0b1001,
        };

    public float range = 0.03f;

    public bool TryMerge()
    {
        Vector2 pos = transform.position;
        Dictionary<GameObject, int> contacted = new Dictionary<GameObject, int>();
        for (int i = 0; i < 4; i++)
        {
            var checkPoint = checkPoints[i];
            var localScale = transform.localScale;
            Vector2 bias = new Vector2(checkPoint.x * localScale.x, checkPoint.y * localScale.y);
            var result = Physics2D.OverlapCircleAll(pos + bias, range);
            foreach (var hit in result)
            {
                if (!hit.CompareTag("Corner")) continue;
                var opposite = hit.transform.parent.parent.gameObject;
                // if (!opposite.TryGetComponent(out Box box) || !box.MergeAble) {
                //     continue;
                // }
                if (opposite == gameObject) continue;
                if (contacted.ContainsKey(opposite))
                {
                    int direction = checkPointDirection[i] & contacted[opposite];
                    bool isHorizontal = direction == 1 || direction == 2;
                    Merge(opposite, isHorizontal);
                    return true;
                }
                else
                {
                    contacted[opposite] = checkPointDirection[i];
                }
            }
        }
        return false;
    }

    private void Merge(GameObject opposite, bool isVertical)
    {
        transform.position = (transform.position + opposite.transform.position) / 2;
        Vector3 scale = transform.localScale;
        if (isVertical)
        {
            scale.y += opposite.transform.localScale.y;
        }
        else
        {
            scale.x += opposite.transform.localScale.x;
        }
        transform.localScale = scale;
        Destroy(opposite);
    }
}