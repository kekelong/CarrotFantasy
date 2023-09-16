
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DoTweenTest : MonoBehaviour
{
    [SerializeField]
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        /*
         * DOTween.To：这是DOTween插件提供的静态方法，用于创建一个补间动画。
         *()=>image.color：这是一个Lambda表达式，表示获取图片（Image）的当前颜色作为初始值。
         *toColor => image.color = toColor：这也是一个Lambda表达式，表示将目标颜色（toColor）赋值给图片的颜色属性，实现颜色渐变效果。
         *new Color(0, 0, 0, 0)：这是目标颜色，表示 RGBA 颜色空间中的一个透明黑色（全透明），即没有颜色。
         *5f：表示动画的持续时间，这里是5秒。
         */
        DOTween.To(()=>image.color, toColor=>image.color = toColor, new Color(0,0,0,0),20f);


        Tween t = transform.DOLocalMoveX(100, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
