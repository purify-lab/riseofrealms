using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMgr : MonoBehaviour 
{
    public bool isPlacing;
    public GameObject building;

    public Vector3Int buildingCursor;

    Plane m_Plane;
    // Start is called before the first frame update
    void Start()
    {
        m_Plane = new Plane(Vector3.up, 0);
    }
    
    void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    void HandleFingerTap(Lean.Touch.LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }
        
        if (BuildingMgr.Inst.isPlacing)
        {
            var cell = MapDrawer.inst.GetTileByPos(buildingCursor);
            cell.isWalkable = false;
            BuildingMgr.Inst.EndPlace();
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (BuildingMgr.Inst.isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 0.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                hitPoint.z *= -1;

                buildingCursor = MapDrawer.inst.SnapToHexGridCoord(hitPoint);
                BuildingMgr.Inst.building.transform.position = MapDrawer.inst.SnapToHexGrid(hitPoint);
            } 
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                hitPoint.z *= -1;

                var hitCell = MapDrawer.inst.SnapToHexGridCoord(hitPoint);
                
                Player.inst.MoveTo(hitCell);
            }
        }
    }
}