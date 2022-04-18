using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationAction
{
    public string animationName;
    public PVGamesAnimationSheet animationSheet;
}



public class PVGamesAnimator : MonoBehaviour
{
    [SerializeField] private float frameRate;
    public PVGamesAnimationSheet anim;
    public Sprite[] sheet;
    public int frameIndex;
    public int directionOffset;
    public int startOffset;
    public int frameCount;

    private int _currentDirection;
    private DirectionFinder dir;
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    
    private void Start()
    {
        dir = GetComponent<DirectionFinder>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LoadAnimation(anim);
    }

    private void Update()
    {
        if (_currentDirection != (int) dir.facingDirection)
        {
            _currentDirection = (int) dir.facingDirection;
            GetDirectionOffset();
        }
        TryAnimate();
    }

    void SetSprite()
    {
        int index = startOffset+(directionOffset * frameCount) + frameIndex;
        _spriteRenderer.sprite = sheet[index];
    }

    void TryAnimate()
    {
        if (anim == null)
            return;
        if (_timer > 0)
            _timer -= Time.deltaTime;
        else Animate();
    }
    
    void Animate()
    {
        _timer = frameRate;
        frameIndex++;
        if (frameIndex > frameCount-1)
            frameIndex = 0;
        SetSprite();
    }
    
    void GetDirectionOffset()
    {
        
        #region IF ELSE CHAIN
        
        //if (dir.facingDirection         == DirectionFinder.PossibleDirections.WEST)
        //    directionOffset = 1;
        //
        //else if (dir.facingDirection    == DirectionFinder.PossibleDirections.SOUTH)
        //    directionOffset = 0;
        //
        //else if (dir.facingDirection    == DirectionFinder.PossibleDirections.EAST ||
        //         dir.facingDirection    == DirectionFinder.PossibleDirections.NORTH)
        //    directionOffset = (int) dir.facingDirection / 2;
        //
        //else if (dir.facingDirection    == DirectionFinder.PossibleDirections.SOUTH_WEST ||
        //         dir.facingDirection    == DirectionFinder.PossibleDirections.SOUTH_EAST)
        //    directionOffset = (int) dir.facingDirection + 3;
        //
        //else if(dir.facingDirection     == DirectionFinder.PossibleDirections.NORTH_EAST)
        //    directionOffset = 7;
        //
        //else if(dir.facingDirection     == DirectionFinder.PossibleDirections.NORTH_WEST)
        //    directionOffset = 5;
        
        #endregion

        #region Switch Case

        //switch (dir.facingDirection)
        //{
        //    case DirectionFinder.PossibleDirections.WEST:
        //        directionOffset = 1;
        //        break;
        //    
        //    case DirectionFinder.PossibleDirections.SOUTH:
        //        directionOffset = 0;
        //        break;
        //    
        //    case DirectionFinder.PossibleDirections.EAST:
        //    case DirectionFinder.PossibleDirections.NORTH:
        //        directionOffset = (int) dir.facingDirection / 2;
        //        break;
        //    
        //    case DirectionFinder.PossibleDirections.SOUTH_WEST:
        //    case DirectionFinder.PossibleDirections.SOUTH_EAST:
        //        directionOffset = (int) dir.facingDirection + 3;
        //        break;
        //    
        //    case DirectionFinder.PossibleDirections.NORTH_EAST:
        //        directionOffset = 7;
        //        break;
        //    
        //    case DirectionFinder.PossibleDirections.NORTH_WEST:
        //        directionOffset = 5;
        //        break;
        //}

        #endregion

        #region Switch Expression

        directionOffset = ToOrientation(dir.facingDirection);

        #endregion

    }

    private static int ToOrientation(DirectionFinder.PossibleDirections direction) => direction switch
    {
        DirectionFinder.PossibleDirections.WEST         => 1,
        DirectionFinder.PossibleDirections.SOUTH        => 0,
        DirectionFinder.PossibleDirections.EAST         => (int) direction / 2,
        DirectionFinder.PossibleDirections.NORTH        => (int) direction / 2,
        DirectionFinder.PossibleDirections.SOUTH_WEST   => (int) direction + 3,
        DirectionFinder.PossibleDirections.SOUTH_EAST   => (int) direction + 3,
        DirectionFinder.PossibleDirections.NORTH_EAST   => 7,
        DirectionFinder.PossibleDirections.NORTH_WEST   => 5,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public void LoadAnimation(PVGamesAnimationSheet incomingAnimation)
    {
        anim = incomingAnimation;
        sheet = Resources.LoadAll<Sprite>("Mythos/"+anim.ID); // Need to add function to check if we already have it loaded
        startOffset = anim.startIndex;
        frameCount  = anim.frameCount;
        if (frameCount <= 0)
            frameCount = 8;
    }
}
