using UnityEngine;

using DG.Tweening;


public class FullScreenEffectManager : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature _feature;
    
    [SerializeField] Texture2D[] _ruleTextures;

    [SerializeField] Color color;

    [SerializeField] bool direction = true;
    float _progress;
    bool _onFade;
    private Tween _tween;
    

    [SerializeField] int textureIndex;

    void Start()
    {
        SetTexture();
        SetColor();
        _progress = 0;

    }

    private void SetColor()
    {
        _feature.passMaterial.SetColor("_Color", color);
    }

    private void SetTexture()
    {
         _feature.passMaterial.SetTexture("_TransitionTex", _ruleTextures[textureIndex]);
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.R))
        {
            FadeIn();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            FadeOut();
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            textureIndex += _ruleTextures.Length;
            textureIndex ++;
            textureIndex %= _ruleTextures.Length;

            SetTexture();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            textureIndex += _ruleTextures.Length;
            textureIndex --;
            textureIndex %= _ruleTextures.Length;

            SetTexture();
        }

    }

    public void FadeIn()
    {
        _progress = 0;
        _onFade = true;

        _tween?.Kill();
        _tween = DOTween.To(() => _progress, x => _progress = x, 1f, 1f)
                .OnUpdate(UpdateTransition)
                .OnComplete(() =>{
                    _onFade = false;
                    _feature.passMaterial.SetFloat("_Progress", 0f);
                });
                
    }

    public void FadeOut()
    {
        _progress = 1;
        _onFade = true;

        _tween?.Kill();
        _tween = DOTween.To(() => _progress, x => _progress = x, 0f, 1f)
                .OnUpdate(UpdateTransition)
                .OnComplete(() =>{
                    _onFade = false;
                    _feature.passMaterial.SetFloat("_Progress", 1.1f);
                });
    }

    private void UpdateTransition()
    {
        _feature.passMaterial.SetFloat("_Progress", 1f - _progress + (direction ? 0 : -1));
    }

    public bool IsFadeWhile()
    {
        return _onFade;
    }
}
