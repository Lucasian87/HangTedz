//////////////////////////////////////////////////////////////////////////////
// bl_TweenerColor.cs
// Simple Tweener UGUI
//
// - Tween the target's graphic color.
//
//
//                            Lovatto Studio
//////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.EventSystems;
namespace UnityEngine.UI
{
    public class bl_TweenerColor : bl_Tweener, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField]private Color ToColor;
        [SerializeField]private float Duration = 0.5f;

        private Graphic m_graphic;
        private Color defaultColor;

        void Awake()
        {
            m_graphic = GetComponent<Graphic>();
            defaultColor = ColorValue;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            DoTransition(ToColor);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            DoTransition(defaultColor);
        }

        void DoTransition(Color target)
        {
            m_graphic.CrossFadeColor(target, Duration, true, true);
        }
     
    }
}