using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Carrot
{
    public class SlideCanCoverScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private Vector2 cellSize;

        [SerializeField]
        private int spacing;//间隙

        [SerializeField]
        private int leftOffset;//左偏移量


        [SerializeField]
        private TMP_Text pageText;
        private RectTransform contentRectTransform;
        private float contentLength; //容器长度
        private int totalItemNum;//共有几个单元格
        private float beginMousePositionX;
        private float endMousePositionX;        
        private float lastProportion;//上一个位置比例           
        private float upperLimit;//上限值
        private float lowerLimit;//下限值
        private float firstItemLength;//移动第一个单元格的距离
        private float oneItemLength;//滑动一个单元格需要的距离
        private float oneItemProportion;//滑动一个单元格所占比例      
        private int currentIndex;//当前单元格索引


        private void Awake()
        {
            contentRectTransform = scrollRect.content;
            totalItemNum = contentRectTransform.childCount;
            contentLength = (totalItemNum-1) * (cellSize.x + spacing);
            contentRectTransform.sizeDelta = new Vector2(contentLength, cellSize.y);

            firstItemLength = cellSize.x / 2 + leftOffset;
            oneItemLength = cellSize.x + spacing;
            oneItemProportion = oneItemLength / contentLength;

            upperLimit = 1 - firstItemLength / contentLength;
            lowerLimit = firstItemLength / contentLength;
            currentIndex = 0;
            scrollRect.horizontalNormalizedPosition = 0f;
            UpdatePageNum();
        }

        public void Init()
        {
            lastProportion = 0;
            currentIndex = 0;
            if (scrollRect != null)
            {
                scrollRect.horizontalNormalizedPosition = 0;
                UpdatePageNum();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float offSetX = 0;
            endMousePositionX = Input.mousePosition.x;
            offSetX = (beginMousePositionX - endMousePositionX) * 2;           
            if (Mathf.Abs(offSetX) > firstItemLength)//执行滑动动作的前提是要大于第一个需要滑动的距离
            {
                if (offSetX > 0)//右滑
                {
                    if (currentIndex >= totalItemNum-1)
                    {
                        return;
                    }
                    int moveCount =
                        (int)((offSetX - firstItemLength) / oneItemLength) + 1;//当次可以移动的格子数目
                    currentIndex += moveCount;
                    if (currentIndex >= totalItemNum-1)
                    {
                        currentIndex = totalItemNum-1;
                    }
                    //当次需要移动的比例:上一次已经存在的单元格位置
                    //的比例加上这一次需要去移动的比例
                    lastProportion += oneItemProportion * moveCount;
                    if (lastProportion >= upperLimit)
                    {
                        lastProportion = 1;
                    }
                }
                else //左滑
                {
                    if (currentIndex <= 0)
                    {
                        return;
                    }
                    int moveCount =
                        (int)((offSetX + firstItemLength) / oneItemLength) - 1;//当次可以移动的格子数目
                    currentIndex += moveCount;
                    if (currentIndex <= 0)
                    {
                        currentIndex = 0;
                    }
                    //当次需要移动的比例:上一次已经存在的单元格位置
                    //的比例加上这一次需要去移动的比例
                    lastProportion += oneItemProportion * moveCount;
                    if (lastProportion <= lowerLimit)
                    {
                        lastProportion = 0;
                    }
                }
                UpdatePageNum();

            }

            DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.OutQuint);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            beginMousePositionX = Input.mousePosition.x;
        }
        private void UpdatePageNum()
        {
            if (pageText != null)
            {
                int index = currentIndex + 1;
                pageText.text = index.ToString() + "/" + totalItemNum;
            }
        }
    }
}
