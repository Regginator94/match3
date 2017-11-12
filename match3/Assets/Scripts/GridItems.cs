﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItems : MonoBehaviour {

    public int x
    {
        get;
        private set;
    }

    public int y
    {
        get;
        private set;
    }

    [HideInInspector]
    public int id;

    public void OnItemPostionChanged(int newX, int newY)
    {
        x = newX;
        y = newY;

        gameObject.name = string.Format("bean_[{0}][{1}]", x, y);
    }

    private void OnMouseDown()
    {
        if(OnMouseOverItemEventHandler != null)
        {
            OnMouseOverItemEventHandler(this);
        }
    }

    public delegate void OnMouseOverItem(GridItems item);
    public static event OnMouseOverItem OnMouseOverItemEventHandler;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
