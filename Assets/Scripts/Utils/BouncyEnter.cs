using UnityEngine;
using DG.Tweening;

public class BouncyEnter : MonoBehaviour
{
    [Header("Enter movement")]
    public Vector3 fromOffset = new Vector3(0f, 6f, 0f);
    public float enterDuration = 0.8f;
    public float jumpPower = 2.5f;
    public int numJumps = 1;
    public Ease moveEase = Ease.OutQuad;

    [SerializeField] private Transform target;

    private void Awake()
    {
        if (target == null) target = transform;
    }

    public void Play()
    {
        DOTween.Kill(target);
        MusicManager.Instance.PlayMusic("Game");
        Vector3 targetPos = target.position;
        Vector3 startPos = targetPos + fromOffset;

        target.position = startPos;
        target.DOJump(targetPos, jumpPower, numJumps, enterDuration).SetEase(moveEase);
    }
}
