//------------------------------------------------------------------------
// Simple Tweener UGUI
//
//
//                          Lovatto Studio 2015
//------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class bl_Tweener : MonoBehaviour, IEventSystemHandler
{


    protected bool isCached = false;
    protected Image mImage = null;
    protected Text mText = null;
    protected RawImage mRaw = null;
    protected Material mMat = null;

    protected Transform m_Tranform = null;
    public Transform Transform
    {
        get
        {
            if (m_Tranform == null)
                m_Tranform = transform;

            return m_Tranform;
        }
    }

    protected bl_Tweener m_Tweener = null;
    public bl_Tweener current
    {
        get
        {
            if (m_Tweener == null)
                m_Tweener = GetComponent<bl_Tweener>();

            return m_Tweener;
        }
    }

    protected RectTransform m_RecTransform = null;
    public RectTransform RecTransform
    {
        get
        {
            if (m_RecTransform == null)
                m_RecTransform = GetComponent<RectTransform>();

                    return m_RecTransform;
        }
    }

    public Vector2 RectPosition
    {
        get
        {
            return RecTransform.anchoredPosition;
        }
        set
        {
            RecTransform.anchoredPosition = value;
        }
    }

    public Vector2 RectSize
    {
        get
        {
            return RecTransform.sizeDelta;
        }
        set
        {
            RecTransform.sizeDelta = value;
        }
    }

    public Vector3 Lerp(Vector3 from, Vector3 to, float t)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(from.x + ((to.x - from.x) * t), from.y + ((to.y - from.y) * t), from.z + ((to.z - from.z) * t));
    }

    public Vector2 Lerp(Vector2 from, Vector2 to, float t)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(from.x + ((to.x - from.x) * t), from.y + ((to.y - from.y) * t));
    }

    public Color Lerp(Color a, Color b, float t)
    {
        t = Mathf.Clamp01(t);
        return new Color(a.r + ((b.r - a.r) * t), a.g + ((b.g - a.g) * t), a.b + ((b.b - a.b) * t), a.a + ((b.a - a.a) * t));
    }
    public  float Lerp(float from, float to, float t)
    {
        return (from + ((to - from) * Mathf.Clamp01(t)));
    }
    public float Sinus(float rate, float amp, float offset = 0.0f)
    {
        return (Mathf.Cos((Time.time + offset) * rate) * amp);
    }

    public float LerpLogic(float val)
    {
        if (val < (1 / 2.75))
        {
            val = 7.5685f * val * val;
        }
        else if (val < (2 / 2.75))
        {
            val = 7.5625f * (val -= (1.5f / 2.75f)) * val + 0.75f; 
        }
        else if (val < (2.5 / 2.75))
        {
            val = 7.5625f * (val -= (2.25f / 2.75f)) * val + 0.9375f; 
        }
        else
        {
            val = 7.5625f * (val -= (2.625f / 2.75f)) * val + 0.984375f; 
        }
        return val;
    }

    /// <summary>
    /// Get the target grpahic of current component
    /// </summary>
    public void GetTargetGraphic()
    {
        isCached = true;

         mImage = GetComponent<Image>();
         if (mImage != null)
            return;
         mText = GetComponent<Text>();
         if (mText != null)
             return;
         mRaw = GetComponent<RawImage>();
         if (mRaw != null) 
             return;
         mMat = GetComponent<Renderer>().material;
         if (mMat != null)
             return;
         mImage = GetComponentInChildren<Image>();
 
    }

    /// <summary>
    /// Get or Set color of current target graphic
    /// </summary>
    public Color ColorValue
    {
        get
        {
            if (!isCached) GetTargetGraphic();
            if (mImage != null) return mImage.color;
            if (mText != null) return mText.color;
            if (mRaw != null) return mRaw.color;
            if (mMat != null) return mMat.color;
            return Color.white;
        }
        set
        {
            if (!isCached) GetTargetGraphic();
            if (mImage != null)  mImage.color = value;
            if (mText != null) mText.color = value;
            if (mRaw != null) mRaw.color = value;
            if (mMat != null) mMat.color = value;
        }
    }

    public Sprite SpriteValue
    {
        get
        {
           if(GetComponent<Image>() != null)
            {
                return GetComponent<Image>().sprite;
            }
            return null;
        }
        set
        {
            GetComponent<Image>().sprite = value;
        }
    }
}