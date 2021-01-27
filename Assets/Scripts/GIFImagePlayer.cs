using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GIFImagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Sprite[] _animationFrames;
    [SerializeField] private float _fps;
    private SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int frameCount = (int)(Time.time * _fps);
        frameCount %= _animationFrames.Length;
        _spriteRenderer.sprite = _animationFrames[frameCount];
    }
}
