using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RoundedGradient : MonoBehaviour
{
    public Material mat;
    public bool UseGradient;
    public Color ColorA = Color.white;
    public Color ColorB = Color.white;
    public float borderRadius = 0;
    [Range(0.01f, 2.0f)]
    public float ColorMix = 0.6f;
    [Range(-3.60f, 3.60f)]
    public float GradientAngle = 0;

    void OnRectTransformDimensionsChange()
    {
        Refresh();
    }

    private void OnValidate()
    {
        Refresh();
    }
    public void Refresh()
    {

        RectTransform rect = GetComponent<RectTransform>();
        if (mat == null)
        {
            return;
        }

        Image image = GetComponent<Image>() ;
        RawImage rawImage = GetComponent<RawImage>();

        if (image != null)
            ProcessImage(image, rect);
        if (rawImage != null)
            ProcessImage(rawImage, rect);

    
    }

    private void ProcessImage(Image image, RectTransform rect)
    {
        image.material = mat;
        image.material.SetColor("_ColorA", ColorA);
        image.material.SetColor("_ColorB", ColorB);

        RectTransform parentCanvas = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
        if (parentCanvas != null)
        {
            if (rect.anchorMin.x == 0 && rect.anchorMin.y == 0 && rect.anchorMax.x == 1 & rect.anchorMax.y == 1)
            {
                //full screen rect
                image.material.SetVector("_WidthHeightRadius", new Vector4(parentCanvas.rect.width, parentCanvas.rect.height, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 1 && rect.anchorMin.y == 0 && rect.anchorMax.x == 1 & rect.anchorMax.y == 1)
            {
                //right stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, parentCanvas.rect.height, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 0.5f && rect.anchorMin.y == 0 && rect.anchorMax.x == 0.5f & rect.anchorMax.y == 1)
            {
                //center stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, parentCanvas.rect.height, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 0 && rect.anchorMin.y == 0 && rect.anchorMax.x == 0 & rect.anchorMax.y == 1)
            {
                //left stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, parentCanvas.rect.height, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 0 && rect.anchorMin.y == 1 && rect.anchorMax.x == 1 & rect.anchorMax.y == 1)
            {
                //top stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(parentCanvas.rect.width, rect.sizeDelta.y, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 0 && rect.anchorMin.y == 0.5 && rect.anchorMax.x == 1 & rect.anchorMax.y == 0.5)
            {
                //middle stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(parentCanvas.rect.width, rect.sizeDelta.y, borderRadius, 0));
            }
            else if (rect.anchorMin.x == 0 && rect.anchorMin.y == 0 && rect.anchorMax.x == 1 & rect.anchorMax.y == 0)
            {
                //middle stretch
                image.material.SetVector("_WidthHeightRadius", new Vector4(parentCanvas.rect.width, rect.sizeDelta.y, borderRadius, 0));
            }
            else
            {
                image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, rect.sizeDelta.y, borderRadius, 0));
            }
        }
        else
        {
            image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, rect.sizeDelta.y, borderRadius, 0));
        }



        image.material.SetFloat("_ColorMix", ColorMix);
        image.material.SetFloat("_GradientAngle", GradientAngle);
        image.material.SetFloat("_UseGradient", UseGradient == false ? 0 : 1);
    }
    private void ProcessImage(RawImage image, RectTransform rect)
    {
        image.material = mat;
        image.material.SetColor("_ColorA", ColorA);
        image.material.SetColor("_ColorB", ColorB);
        if (rect.sizeDelta != Vector2.zero)
            image.material.SetVector("_WidthHeightRadius", new Vector4(rect.sizeDelta.x, rect.sizeDelta.y, borderRadius, 0));
        image.material.SetFloat("_ColorMix", ColorMix);
        image.material.SetFloat("_GradientAngle", GradientAngle);
        image.material.SetFloat("_UseGradient", UseGradient == false ? 0 : 1);
    }
}
