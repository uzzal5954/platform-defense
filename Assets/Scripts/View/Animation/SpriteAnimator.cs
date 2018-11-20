using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class SpriteAnimator : MonoBehaviour {
    public List<SpriteAnimation> animations;

    public int frame {
        get;
        private set;
    }
    
    public string animationName {
        get {
            if(this.activeAnimation == null) {
                return null;
            }

            return this.activeAnimation.name;
        }
    }

    private SpriteRenderer rend;

    private SpriteAnimation activeAnimation;
    private string activeAnimationName;
    private float lastFrameChangeTime;

    private Dictionary<string, SpriteAnimation> animationsDict;

    public void Awake() {
        this.rend = this.gameObject.GetComponent<SpriteRenderer>();

        this.animationsDict = new Dictionary<string, SpriteAnimation>(this.animations.Count);
        foreach(var animation in this.animations) {
            this.animationsDict.Add(animation.name, animation);
        }
    }

    public void Update() {
        if(this.activeAnimation == null) {
            return;
        }

        if(Time.time - this.lastFrameChangeTime > this.activeAnimation.framerate) {
            this.frame = (this.frame + 1) % this.activeAnimation.frames.Count;
            this.lastFrameChangeTime = Time.time;

            this.rend.sprite = this.activeAnimation.frames[this.frame];
        }
    }

    protected void SetActive(string name, bool resetFrames = true, bool resetTime = true) {
        if(this.activeAnimation != null && this.activeAnimationName == name) {
            return;
        }

        this.activeAnimation = this.animationsDict[name];
        this.activeAnimationName = name;

        if(resetFrames) {
            this.frame = 0;
        }

        if(resetTime) {
            this.lastFrameChangeTime = -100;
        }
    }
}