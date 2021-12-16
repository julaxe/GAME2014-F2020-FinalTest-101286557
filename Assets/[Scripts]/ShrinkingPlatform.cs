using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On Collision with player, the platform will shrink, then it will appears again after a few seconds
/// </summary>
/// made by Julian Escobar Echeverri
/// version: 1.0
/// 2021-12-16
public class ShrinkingPlatform : MonoBehaviour
{
    [SerializeField] private float shrinkingTime;
    [SerializeField] private float expandingTime;
    [SerializeField] private float waitingTimeToRegrow;

    //components
    private BoxCollider2D _boxCollider2D;
    private AudioSource _audioSource;
    
    //shrinking and expanding variables
    private Vector3 _initialScale;
    private bool _isShrinking;
    private bool _isExpanding;
    private double _timer;
    
    //audio clips
    private AudioClip _shrinkingSound, _expandingSound;
    


    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        
        _initialScale = transform.localScale;

        _shrinkingSound = Resources.Load<AudioClip>("Sounds/shrinkSound");
        _expandingSound = Resources.Load<AudioClip>("Sounds/expandSound");

    }

    private void FixedUpdate()
    {
        if (_isShrinking)
        {
            if (_timer < shrinkingTime)
            {
                var newLocalScale = transform.localScale;
                newLocalScale.x = Mathf.Lerp(_initialScale.x, 0.0f, (float)(_timer / shrinkingTime));
                newLocalScale.y = Mathf.Lerp(_initialScale.y, 0.0f, (float)(_timer / shrinkingTime));
                newLocalScale.z = Mathf.Lerp(_initialScale.z, 0.0f, (float)(_timer / shrinkingTime));
                transform.localScale = newLocalScale;
            }
            else
            {
                _isShrinking = false;
                //start expanding after a few seconds
                StartCoroutine(ExpandPlatfrom());
            }

            _timer += Time.fixedDeltaTime;
        }
        else if (_isExpanding)
        {
            if (_timer < expandingTime)
            {
                var newLocalScale = transform.localScale;
                newLocalScale.x = Mathf.Lerp(0.0f, _initialScale.x, (float)(_timer / expandingTime));
                newLocalScale.y = Mathf.Lerp( 0.0f,_initialScale.y,  (float)(_timer / expandingTime));
                newLocalScale.z = Mathf.Lerp( 0.0f, _initialScale.z, (float)(_timer / expandingTime));
                transform.localScale = newLocalScale;
            }
            else
            {
                _isExpanding = false;
            }
            _timer += Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_isShrinking && !_isExpanding)
        {
            if (other.transform.CompareTag("Player"))
            {
                //starts shrinking
                _isShrinking = true;
                _timer = 0;
                _audioSource.PlayOneShot(_shrinkingSound);
            }
        }
    }

    IEnumerator ExpandPlatfrom()
    {
        yield return new WaitForSeconds(waitingTimeToRegrow);
        _isExpanding = true;
        _timer = 0.0f;
        _audioSource.PlayOneShot(_expandingSound);
    }

    private void MoveUpAndDown()
    {
        
    }
}
