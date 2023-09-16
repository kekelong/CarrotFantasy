
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
         * DOTween.To������DOTween����ṩ�ľ�̬���������ڴ���һ�����䶯����
         *()=>image.color������һ��Lambda���ʽ����ʾ��ȡͼƬ��Image���ĵ�ǰ��ɫ��Ϊ��ʼֵ��
         *toColor => image.color = toColor����Ҳ��һ��Lambda���ʽ����ʾ��Ŀ����ɫ��toColor����ֵ��ͼƬ����ɫ���ԣ�ʵ����ɫ����Ч����
         *new Color(0, 0, 0, 0)������Ŀ����ɫ����ʾ RGBA ��ɫ�ռ��е�һ��͸����ɫ��ȫ͸��������û����ɫ��
         *5f����ʾ�����ĳ���ʱ�䣬������5�롣
         */
        DOTween.To(()=>image.color, toColor=>image.color = toColor, new Color(0,0,0,0),20f);


        Tween t = transform.DOLocalMoveX(100, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
