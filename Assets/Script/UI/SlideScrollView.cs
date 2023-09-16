using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Carrot
{
    public class SlideScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
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
        private bool needSendMessage;


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
            Initialized();
        }
        private void Initialized()
        {
            contentRectTransform = scrollRect.content;
            totalItemNum = contentRectTransform.childCount;
            contentLength = (totalItemNum - 1) * (cellSize.x + spacing);
            contentRectTransform.sizeDelta = new Vector2(contentLength, cellSize.y);

            firstItemLength = cellSize.x / 2 + leftOffset;
            oneItemLength = cellSize.x + spacing;
            oneItemProportion = oneItemLength / contentLength;

            upperLimit = 1 - firstItemLength / contentLength;
            lowerLimit = firstItemLength / contentLength;
            currentIndex = 0;
            scrollRect.horizontalNormalizedPosition = 0f;
            if (pageText != null)
            {
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            }
        }
        public void Init()
        {
            Initialized();
            lastProportion = 0;
            currentIndex = 0;
            if (scrollRect != null)
            {
                scrollRect.horizontalNormalizedPosition = 0;
                UpdatePageNum();
            }
        }

        /// <summary>
        /// 通过拖拽与松开来达成翻页效果
        /// </summary>
        /// <param name="eventData"></param>

        public void OnEndDrag(PointerEventData eventData)
        {
            endMousePositionX = Input.mousePosition.x;
            float offSetX = 0;
            offSetX = beginMousePositionX - endMousePositionX;

            if (offSetX > 0)//右滑
            {
                if (currentIndex >= totalItemNum - 1)
                {
                    return;
                }
                if (needSendMessage)
                {
                    UpdatePanel(true);
                }
                currentIndex++;
                //当次需要移动的比例:上一次已经存在的单元格位置
                //的比例加上这一次需要去移动的比例
                lastProportion += oneItemProportion;
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
                if (needSendMessage)
                {
                    UpdatePanel(false);
                }
                currentIndex--;
        
                //当次需要移动的比例:上一次已经存在的单元格位置
                //的比例加上这一次需要去移动的比例
                lastProportion -= oneItemProportion;
                if (lastProportion <= lowerLimit)
                {
                    lastProportion = 0;
                }
            }
            UpdatePageNum();
            DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.OutQuint);
        }

        /// <summary>
        /// 按钮来控制翻书效果
        /// </summary>

        public void ToNextPage()
        {
            if (currentIndex >= totalItemNum - 1)
            {
                return;
            }
            currentIndex++;
            //当次需要移动的比例:上一次已经存在的单元格位置
            //的比例加上这一次需要去移动的比例
            lastProportion += oneItemProportion;
            if (lastProportion >= upperLimit)
            {
                lastProportion = 1;
            }
            if (needSendMessage)
            {
                UpdatePanel(true);
            }
            UpdatePageNum();
            DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.OutQuint);

        }

        public void ToLastPage()
        {
            if (currentIndex <= 0)
            {
                return;
            }
            currentIndex--;
            //当次需要移动的比例:上一次已经存在的单元格位置
            //的比例加上这一次需要去移动的比例
            lastProportion -= oneItemProportion;
            if (lastProportion <= lowerLimit)
            {
                lastProportion = 0;
            }
            UpdatePageNum();
            if (needSendMessage)
            {
                UpdatePanel(false);
            }
            DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.OutQuint);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
           beginMousePositionX = Input.mousePosition.x;
        }

        public void InitScrollLength()
        {
            totalItemNum = contentRectTransform.childCount;
            contentLength = (totalItemNum - 1) * (cellSize.x + spacing);
            contentRectTransform.sizeDelta = new Vector2(contentLength, cellSize.y);
        }

        //设置Content的大小
        public void SetContentLength(int itemNum)
        {           
            totalItemNum = itemNum;
            contentLength = (totalItemNum - 1) * (cellSize.x + spacing);
            contentRectTransform.sizeDelta = new Vector2(contentLength, cellSize.y);
        }

        private void UpdatePageNum()
        {
            if (pageText != null)
            {
                int index = currentIndex + 1;
                pageText.text = index.ToString() + "/" + totalItemNum;
            }
        }

        //发送翻页信息的方法
        public void UpdatePanel(bool toNext)
        {
            if (toNext)
            {
                gameObject.SendMessageUpwards("ToNextLevel");
            }
            else
            {
                gameObject.SendMessageUpwards("ToLastLevel");
            }
        }
    }
}
