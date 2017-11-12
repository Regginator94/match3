using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGrid : MonoBehaviour {

    GameObject[] itemPrefabs;
    public float items_width = 1f;
    private GridItems[,] gridItems;
    public int Xsize;
    public int Ysize;

    private GridItems SelectedItems;
    public static int minItemForMatch = 3;
	// Use this for initialization
	void Start () {
        GetItemsGrid();
        FillGridMethod();
        GridItems.OnMouseOverItemEventHandler += OnMouseOverItem;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetItemsGrid()
    {
        itemPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        for(int i=0; i<itemPrefabs.Length; i++)
        {
            itemPrefabs[i].GetComponent<GridItems>().id = i;
        }
    }

    void FillGridMethod()
    {
        gridItems = new GridItems[Xsize, Ysize];
        for(int i = 0; i <Xsize; i++)
        {
            for(int j = 0; j<Ysize; j++)
            {
                gridItems[i, j] = instantiate_items(i, j);
            }
        }

    }


    GridItems instantiate_items(int x, int y)
    {
        GameObject random_items = itemPrefabs[Random.Range(0, 5)];
        GridItems newItem = ((GameObject)Instantiate (random_items, 
            new Vector2(x * items_width, y),Quaternion.identity)).GetComponent<GridItems>();
        newItem.OnItemPostionChanged(x, y);
        return newItem;

    }

    private void OnMouseOverItem(GridItems item)
    {
        if(SelectedItems == null)
        {
            Debug.Log("Start Point");
            SelectedItems = item;
        }
        else
        {
            Debug.Log("End Point");
            float xDiff = Mathf.Abs(item.x - SelectedItems.x);
            float yDiff = Mathf.Abs(item.y - SelectedItems.y);
            if (xDiff + yDiff == 1)
            {
                Debug.Log("Try Match Function");
                StartCoroutine(TryMatch(SelectedItems, item));
            }
            else
            {
                Debug.Log("ZEPSUTE ACHTUNG!");
            }

            SelectedItems = null;
        }
    }

    IEnumerator TryMatch(GridItems a, GridItems b)
    {
        yield return StartCoroutine(Swap(a, b));
        Debug.Log("SWAP ITEMS");
    }



    List<GridItems> SearchHorizontally(GridItems item)
    {
        List<GridItems> H_items = new List<GridItems> { item };
        int left = item.x - 1;
        int right = item.x + 1;
        while(left>=0 && gridItems[left, item.y]!=null && gridItems[left, item.y].id == item.id)
        {
            H_items.Add(gridItems[left, item.y]);
            left--;
        }
        while (right < Xsize && gridItems[right, item.y] != null && gridItems[right, item.y].id == item.id)
        {
            H_items.Add(gridItems[right, item.y]);
            right++;
        }
        return H_items;
    }

    List<GridItems> SearchVertically(GridItems item)
    {
        List<GridItems> V_items = new List<GridItems> {item};
        int lower = item.x - 1;
        int Upper = item.x + 1;
        while (lower >= 0 && gridItems[item.x,lower] != null && gridItems[item.x,lower].id == item.id)
        {
            V_items.Add(gridItems[item.x,lower]);
            lower--;
        }
        while (Upper < Xsize && gridItems[item.x,Upper] != null && gridItems[item.x,Upper].id == item.id)
        {
            V_items.Add(gridItems[Upper, item.y]);
            Upper++;
        }
        return V_items;
    }

    int GetMinimumX(List<GridItems> items)
    {
        float[] indices = new float[items.Count];
        for(int i =0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;

        }
        return (int)Mathf.Min(indices);
    }

    int GetMaximumX(List<GridItems> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;

        }
        return (int)Mathf.Max(indices);
    }

    int GetMinimumY(List<GridItems> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;

        }
        return (int)Mathf.Min(indices);
    }

    int GetMaximumY(List<GridItems> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;

        }
        return (int)Mathf.Max(indices);
    }

    MatchInfo GetMatchInfo(GridItems item)
    {
        MatchInfo matchInfo = new MatchInfo();
        matchInfo.match = null;
        List<GridItems> H_match = SearchHorizontally(item);
        List<GridItems> V_match = SearchVertically(item);
        if(H_match.Count>=minItemForMatch && H_match.Count > V_match.Count)
        {
            matchInfo.matchStartingX = GetMaximumX(H_match);
            matchInfo.matchEndX = GetMinimumX(H_match);
            matchInfo.matchStartingY = matchInfo.matchEndY = H_match[0].x;
            matchInfo.match = H_match;

        }
        else if (V_match.Count >= minItemForMatch)
        {
            matchInfo.matchStartingX = GetMaximumY(V_match);
            matchInfo.matchEndY = GetMinimumY(H_match);
            matchInfo.matchStartingX = matchInfo.matchEndX = H_match[0].y;
            matchInfo.match = V_match;
        }
        return matchInfo;
    }

    IEnumerator Swap(GridItems a, GridItems  b)
    {
        changerGridBodyStatus(false);
        float moveduration = 0.1f;
        Vector3 apos = a.transform.position;
        Vector3 bpos = b.transform.position;
        StartCoroutine(a.transform.Move(bpos, moveduration));
        StartCoroutine(b.transform.Move(apos, moveduration));
        yield return new WaitForSeconds(moveduration);
        swapIndices(a, b);
        changerGridBodyStatus(true);
    }

    void swapIndices(GridItems gridA, GridItems gridB)
    {
        GridItems tempA = gridItems[gridA.x, gridA.y];
        gridItems[gridA.x, gridA.y] = gridB;
        gridItems[gridB.x, gridB.y] = tempA;
        int boldx = gridB.x;
        int boldy = gridB.y;

        gridB.OnItemPostionChanged(gridA.x, gridA.y);
        gridA.OnItemPostionChanged(boldx, boldy);
    }


    void changerGridBodyStatus(bool status)
    {
        foreach(GridItems item in gridItems)
        {
            if(item != null)
            {
                item.GetComponent<Rigidbody2D>().isKinematic = !status;
            }
        }
    }


}
