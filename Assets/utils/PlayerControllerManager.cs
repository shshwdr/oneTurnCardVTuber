using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerControllerManager : Singleton<PlayerControllerManager>
{
    private GameObject currentDragging; // 当前正在拖动的 Building
    public Transform uiDropArea; // 取消UI 放置区域
    public GameObject uiDropAreaText;
    [HideInInspector]
    [FormerlySerializedAs("cell")] public CardVisualize currentDraggingCell;
    // Start is called before the first frame update
    public void StartDragging(GameObject building,CardVisualize cell)
    {
        currentDragging = building;
        this.currentDraggingCell = cell;
        uiDropArea.gameObject.SetActive(true);
    }

    void StopDragging()
    {
        
        uiDropArea.gameObject.SetActive(false);
    }

    //public HoveredObject hoveredObject;
    public void Update()
    {
        // 检查当前是否有 Building 在拖动
        if (currentDragging != null)
        {
            DragBuilding();
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 如果鼠标在 UI 上，不显示弹框
                //HoverOverMenu.FindFirstInstance<HoverOverMenu>().Hide();
                //hoveredObject = null;
                return;
            }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取鼠标位置
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 使用射线检测鼠标指向的物体
            
            
            // if (hit.collider != null && hit.collider.GetComponent<ClickableObject>() ) // 检查是否碰到当前物体
            // {
            //     if( Input.GetMouseButtonDown(0))
            //     {
            //         hit.collider.GetComponent<ClickableObject>().Click();
            //     }
            // }
            
            
            
            
            
            
            bool isMouseOverSprite;
            // if (hit.collider != null && hit.collider.GetComponent<HoveredObject>() ) // 检查是否碰到当前物体
            // {
            //     if( hit.collider.GetComponent<HoveredObject>() !=  hoveredObject)
            //     {
            //         hoveredObject = hit.collider.GetComponent<HoveredObject>();
            //
            //         hoveredObject.Show();
            //     }
            // }
            // else
            // {
            //     hoveredObject = null;
            //     HoverOverMenu.FindFirstInstance<HoverOverMenu>().Hide();
            // }
        }
    }
    private void DragBuilding()
    {
       //  // 获取鼠标位置并转换为世界坐标
       Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       mousePosition.z = 0; // 确保 z 轴为 0
       
       //  
       //  // 自动对齐到网格位置
       //  // Vector2Int gridPosition = GridManager.Instance.WorldToGridPosition(mousePosition);
       //  // Vector3 snapPosition =  GridManager.Instance.GridToWorldPosition(gridPosition);
       //  Vector2Int buildingGridPosition = GridManager.Instance.GetBuildingGridPosition(mousePosition, currentDragging);
       //  var offset = GridManager.Instance.GetBuildingOffset(currentDragging);
       //  Vector3 snapPosition = GridManager.Instance.GridToWorldPosition(buildingGridPosition, currentDragging);
       //
       //  // 更新当前 Building 的位置
         currentDragging.transform.position = mousePosition;

         
       //  var canPlace = currentDraggingCell.CanPlace();;
         
         
         
         
         
         {
             
             if (IsInDropArea(mousePosition))
             {
                 uiDropAreaText.SetActive(true);
             }
             else
             {
                     
                 uiDropAreaText.SetActive(false);
                 //uiDropArea.GetComponent<CanvasGroup>().alpha = 0.5f;
             }
             var canPlace = currentDraggingCell.OnDrag();
             
             if (Input.GetMouseButtonUp(0)) // 左键放下
                 {
                     if (IsInDropArea(mousePosition) ||!canPlace)
                     {
                        
                         currentDraggingCell.Cancel();
                         StopDragging();
                         currentDraggingCell = null;
                         currentDragging = null; // 取消选择
                     }
                     else
                     {
                         //Destroy(currentBuilding);
                         currentDraggingCell.OnPlace();
                         StopDragging();
                         currentDraggingCell = null;
                         currentDragging = null; // 取消选择
                         //Destroy(cell);
                     }
                 }
             else
             {
             }
         }
         //  // 更新当前 Building 的位置
         // // currentBuilding.transform.position = mousePosition;
         //
         //  // 检查网格状态
         //  bool canPlace = GridManager.Instance.CanPlace(currentDragging.GetComponent<Building>(),buildingGridPosition);
         //
         //  if (canPlace)
         //  {
         //      currentDragging.GetComponent<Building>().SetWhite();
         //  }
         //  else
         //  {
         //      currentDragging.GetComponent<Building>().SetRed();
         //  }
         //
         //  // 检查放置
         //  if (Input.GetMouseButtonDown(0)) // 左键放下
         //  {
         //      if (/*IsInDropArea(mousePosition) ||*/ !canPlace)
         //      {
         //         // // currentBuilding.GetComponent<Building>().LockBuilding();
         //         //  Destroy(currentBuilding);
         //         //  currentBuilding = null; // 取消选择
         //      }
         //      else
         //      {
         //          //Destroy(currentBuilding);
         //          GridManager.Instance.PlaceBuilding(currentDragging.GetComponent<Building>(),buildingGridPosition);
         //          currentDragging = null; // 取消选择
         //          Destroy(cell);
         //      }
         //  }


    }
    
    private bool IsInDropArea(Vector3 position)
    {
        // 将鼠标屏幕坐标转换为本地坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiDropArea.GetComponent<RectTransform>(), 
            Camera.main.WorldToScreenPoint(position), 
            null, // 使用 null 来表示没有特殊的事件系统
            out localPoint);

        // 获取 uiDropArea 的 RectTransform
        RectTransform dropAreaRect = uiDropArea.GetComponent<RectTransform>();

        // 检查鼠标坐标是否在 uiDropArea 的范围内
        return dropAreaRect.rect.Contains(localPoint);
    }
}
