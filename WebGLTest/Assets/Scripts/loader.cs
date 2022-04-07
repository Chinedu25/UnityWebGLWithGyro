using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loader : MonoBehaviour
{
   public int NumberOfDots = 4;
   public float minSize = 1;
   public float maxSize = 4;
   public float radius = 5;
   public float fadeTime = 5;
    public Color ColorA = Color.white;
    public Color ColorB = Color.white;
    public float borderRadius = 0;
    [Range(0.01f, 2.0f)]
    public float ColorMix = 0.6f;
    [Range(-3.60f, 3.60f)]
    public float GradientAngle = 0;
    private int _tempNumDots;
    public Material material;

    private List<GameObject> dots = new List<GameObject>();

    void GenerateDots()
    {
        _tempNumDots = NumberOfDots;
        for (int i = 0; i < NumberOfDots; i++)
        {
            GameObject d = new GameObject("Dot");
            d.AddComponent<RectTransform>();
            d.AddComponent<CanvasRenderer>();
            d.AddComponent<Image>();
            RoundedGradient roundedGradient =  d.AddComponent<RoundedGradient>();
            roundedGradient.borderRadius = borderRadius;
            roundedGradient.mat = material;
            roundedGradient.ColorA = ColorA;
            roundedGradient.ColorB = ColorB;
            roundedGradient.ColorMix = 0.6f;
            roundedGradient.UseGradient = true;
            roundedGradient.ColorMix = ColorMix;
            roundedGradient.GradientAngle = GradientAngle;
            //Instantiate(d, SpawnPosition(i, dots.Count, radius), Quaternion.identity);
            RectTransform rectTransform = d.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = SpawnPosition(i, NumberOfDots, radius);
            d.transform.parent = this.transform;
            dots.Add(d);
        }
    }


    private Vector3 SpawnPosition(int i, int numberOfObjects, float distanceFromCenter)
    {
        Vector3 _position;


        float circumferenceProgress = (float)i / numberOfObjects;

        float currentRadian = circumferenceProgress * Mathf.PI * 2;

        float xScaled = Mathf.Cos(currentRadian);
        float yScaled = Mathf.Sin(currentRadian);

        float x = xScaled * distanceFromCenter;
        float y = yScaled * distanceFromCenter;
        _position = new Vector3(x + GetComponent<RectTransform>().anchoredPosition.x, y + (GetComponent<RectTransform>().position.y - (GetComponent<RectTransform>().sizeDelta.y /Screen.height)));

        return _position;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateDots();
    }

    void UpdateDots()
    {
        if (NumberOfDots < 1)
            return;

        if (_tempNumDots != NumberOfDots)
        {
            if (_tempNumDots < NumberOfDots)
            {

                for (int i = 0; i < NumberOfDots - _tempNumDots; i++)
                {
                    GameObject d = new GameObject("Dot");
                    d.AddComponent<RectTransform>();
                    d.AddComponent<CanvasRenderer>();
                    d.AddComponent<Image>();
                    RoundedGradient roundedGradient = d.AddComponent<RoundedGradient>();
                    roundedGradient.borderRadius = borderRadius;
                    roundedGradient.mat = material;
                    roundedGradient.ColorA = ColorA;
                    roundedGradient.ColorB = ColorB;
                    roundedGradient.ColorMix = 0.6f;
                    roundedGradient.UseGradient = true;
                    roundedGradient.ColorMix = ColorMix;
                    roundedGradient.GradientAngle = GradientAngle;
                    //Instantiate(d, SpawnPosition(i, dots.Count, radius), Quaternion.identity);
                    RectTransform rectTransform = d.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = SpawnPosition(i, NumberOfDots, radius);
                    d.transform.parent = this.transform;
                    dots.Add(d);
                  
                }
                _tempNumDots = NumberOfDots;
            }
            else if (_tempNumDots > NumberOfDots)
            {
                for (int i = 0; i < _tempNumDots-NumberOfDots; i++)
                {
                   Destroy(dots[dots.Count-1]);
                    dots.Remove(dots[dots.Count-1]);
                    
                }
                _tempNumDots = NumberOfDots;
            }
        }
        for (int i = 0; i < dots.Count; i++)
        {
            RectTransform rectTransform = dots[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = SpawnPosition(i, NumberOfDots, radius);
            RoundedGradient roundedGradient = dots[i].AddComponent<RoundedGradient>();
            roundedGradient.borderRadius = borderRadius;
            roundedGradient.mat = material;
            roundedGradient.ColorA = ColorA;
            roundedGradient.ColorB = ColorB;
            roundedGradient.ColorMix = 0.6f;
            roundedGradient.UseGradient = true;
            roundedGradient.ColorMix = ColorMix;
            roundedGradient.GradientAngle = GradientAngle;
            roundedGradient.Refresh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDots();
    }
}
