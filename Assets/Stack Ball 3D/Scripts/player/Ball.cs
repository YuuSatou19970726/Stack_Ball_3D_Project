using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("Elements")]
    private new Rigidbody rigidbody;

    [Header("Setting")]
    private bool smash, invincible;
    private int currentBrokenStacks, totalStacks;
    private float currentTime;

    [Header("In Game")]
    [SerializeField] GameObject _invincibleObj;
    [SerializeField] Image _invincibleFill;
    [SerializeField] GameObject _fireEffect;

    public enum BallState
    {
        Prepare, Playing, Died, Finish
    }
    [HideInInspector]
    public BallState ballState = BallState.Prepare;

    [SerializeField]
    private AudioClip _bounceOffClip, _deadClip, _winClip, _destroyClip, _iDestroyClip;

    #region UnityLifecycle
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }

    void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
                smash = true;

            if (Input.GetMouseButtonUp(0))
                smash = false;

            EventInvincible();
        }

        if (ballState == BallState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindAnyObjectByType<LevelSpawner>().NextLevel();
        }
    }

    void FixedUpdate()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rigidbody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (rigidbody.velocity.y > 5)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5, rigidbody.velocity.z);
    }
    #endregion

    #region FunctionPrivate
    private void EventInvincible()
    {
        if (invincible)
        {
            currentTime -= Time.deltaTime * .35f;
            if (!_fireEffect.activeInHierarchy)
                _fireEffect.SetActive(true);
        }
        else
        {
            if (_fireEffect.activeInHierarchy)
                _fireEffect.SetActive(false);

            if (smash)
                currentTime += Time.deltaTime * .8f;
            else
                currentTime -= Time.deltaTime * .5f;
        }

        if (currentTime >= 0.3f || _invincibleFill.color == Color.red)
            _invincibleObj.SetActive(true);
        else
            _invincibleObj.SetActive(false);

        if (currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;
            _invincibleFill.color = Color.red;
        }
        else
        {
            currentTime = 0;
            invincible = false;
            _invincibleFill.color = Color.white;
        }

        if (_invincibleObj.activeInHierarchy)
            _invincibleFill.fillAmount = currentTime / 1;
    }
    #endregion

    #region FunctionPublic
    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;

        if (!invincible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(_destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(_iDestroyClip, 0.5f);
        }
    }
    #endregion

    #region EventCollision
    void OnCollisionEnter(Collision other)
    {
        if (!smash)
        {
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            SoundManager.instance.PlaySoundFX(_bounceOffClip, 0.5f);
        }
        else
        {
            if (invincible)
            {
                if (other.gameObject.CompareTag(Tags.ENEMY) || other.gameObject.CompareTag(Tags.PLANE))
                    other.transform.parent.GetComponent<StackController>().ShatterAllParts();
            }
            else
            {
                if (other.gameObject.CompareTag(Tags.ENEMY))
                    other.transform.parent.GetComponent<StackController>().ShatterAllParts();

                if (other.gameObject.CompareTag(Tags.PLANE))
                {
                    Debug.Log("Over");
                    ScoreManager.instance.ResetScore();
                    SoundManager.instance.PlaySoundFX(_deadClip, 0.5f);
                }
            }
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if (other.gameObject.CompareTag(Tags.FINISH) && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.instance.PlaySoundFX(_winClip, 0.7f);
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (!smash || other.gameObject.CompareTag(Tags.FINISH))
            rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
    }
    #endregion

}
