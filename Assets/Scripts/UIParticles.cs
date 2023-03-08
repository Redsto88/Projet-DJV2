using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class UIParticles : MonoBehaviour
{
    [System.Serializable]
    public class ParticleUIWeight
    {
        public float weight;
        public Sprite sprite;
    }
    [Header("General")]
    [SerializeField] private Transform particlesParent;
    [SerializeField] private List<ParticleUIWeight> particles;
    [SerializeField] private bool isDestroyed;
    [Header("Spawn conditions")]
    [SerializeField] private float spawnDuration;
    [SerializeField] private float spawnRate;
    [NaughtyAttributes.MinMaxSlider(0,1)]
    [SerializeField] private Vector2 XSpawnRange;
    [NaughtyAttributes.MinMaxSlider(0,1)]
    [SerializeField] private Vector2 YSpawnRange;
    [NaughtyAttributes.MinMaxSlider(-180,180)]
    [SerializeField] private Vector2 SpawnAngle;
    [Header("LifeTime")]
    [SerializeField] private float LifeTime;
    [Header("")]
    [SerializeField] private bool Movement;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float XStartSpeed;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float YStartSpeed;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float XAcceleration;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float YAcceleration;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private Vector2 RotSpeed;
    [NaughtyAttributes.ShowIf("Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float RotAcceleration;
    [SerializeField] private bool radialPulse;
    [NaughtyAttributes.ShowIf(NaughtyAttributes.EConditionOperator.And, "radialPulse", "Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private Transform center;
    [NaughtyAttributes.ShowIf(NaughtyAttributes.EConditionOperator.And, "radialPulse", "Movement")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float strength;
    [Header("")]
    [SerializeField] private bool Size;
    [NaughtyAttributes.ShowIf("Size")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float XStartSize;
    [NaughtyAttributes.ShowIf("Size")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float YStartSize;
    [NaughtyAttributes.ShowIf("Size")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float XSizeVariation;
    [NaughtyAttributes.ShowIf("Size")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private float YSizeVariation;

    private float timeEllapsed;
    private GameObject particlePrefab;
    private float totalWeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        particlePrefab = Addressables.LoadAssetAsync<GameObject>("UIParticlePrefab").WaitForCompletion();
        foreach (ParticleUIWeight puiw in particles)
        {
            totalWeight += puiw.weight;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeEllapsed > spawnRate)
        {
            timeEllapsed = 0;
            var particle = Instantiate(particlePrefab);
            particle.transform.SetParent(particlesParent);
            var rt = particle.GetComponent<RectTransform>();
            var x = Random.Range(XSpawnRange.x, XSpawnRange.y);
            var y = Random.Range(YSpawnRange.x, YSpawnRange.y);
            var angle = Random.Range(SpawnAngle.x, SpawnAngle.y);
            var rotSpeed = Random.Range(RotSpeed.x, RotSpeed.y);
            rt.anchorMin = new Vector2(x,y);
            rt.anchorMax = new Vector2(x,y);
            rt.anchoredPosition3D = Vector3.zero;
            rt.localRotation = Quaternion.identity;
            particle.transform.localScale = new Vector3(XStartSize, YStartSize, 1);
            particle.GetComponent<Image>().sprite = chooseSprite();
            particle.GetComponent<UIParticle>().SetParticle(LifeTime, new Vector2(XStartSpeed, YStartSpeed), new Vector2(XAcceleration, YAcceleration), new Vector2(XSizeVariation, YSizeVariation), angle, rotSpeed, RotAcceleration, radialPulse, center, strength);
        }
        timeEllapsed += Time.deltaTime;
    }

    Sprite chooseSprite()
    {
        var rd = Random.Range(0f,totalWeight);
        foreach (ParticleUIWeight puiw in particles)
        {
            rd -= puiw.weight;
            if (rd < 0) return puiw.sprite;
        }
        return null;
    }
}
