using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Advanced Mesh/Mesh Animator")]
public class MeshAnimator : MonoBehaviour
{
    public MeshAnimation meshAnimation;
    public float speed = 1;
    public float maxUpdateDistance = 100;
    public int currentFrame = 0;

    private float deltaFrame = 0;

    private MeshAnimation nextAnimation;
    private float transitionSpeed = 1;

    private MeshFilter mshf;

    public bool isVisible = false;

    void Awake () {
        mshf = GetComponent<MeshFilter>();
    }

    void Update () {
        if (!isVisible || meshAnimation == null) return;

        Transition ();

        deltaFrame += Time.deltaTime * meshAnimation.animationFPS * speed * transitionSpeed;
        if (deltaFrame >= 1) SwitchFrame ();
    }

    void Transition () {
        if (nextAnimation == null) return;

        if (currentFrame != 0) return;

        transitionSpeed = 1;
        meshAnimation = nextAnimation;
        nextAnimation = null;
    }

    void SwitchFrame () {
        currentFrame += Mathf.FloorToInt(deltaFrame);
        currentFrame = currentFrame % meshAnimation.framesCount;

        deltaFrame = deltaFrame % 1;

        mshf.mesh = meshAnimation.frames[currentFrame];

    }

    public void TransitionTo (MeshAnimation ma, float transitionTime = -1) {
        if (transitionTime == 0) {
            currentFrame = 0;
            meshAnimation = ma;
            return;
        }

        nextAnimation = ma;

        if (transitionTime != -1) {
            transitionSpeed = (meshAnimation.framesCount - currentFrame) / transitionTime;
        }
    }

    void OnBecameVisible () {
        isVisible = true;
    }

    void OnBecameInvisible () {
        isVisible = false;
    }
}
