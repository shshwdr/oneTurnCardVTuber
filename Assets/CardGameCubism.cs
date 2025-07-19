using System;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Framework.Motion;
using UnityEngine;

public class CardGameCubism : MonoBehaviour
{
    
    [SerializeField]
    private AnimationClip _bodyAnimation;
    [SerializeField]
    private AnimationClip[] _tapBodyMotions;

    private CubismMotionController _motionController;
    
    private CubismExpressionController _expressionController;
    private void Awake()
    {
        _motionController = GetComponent<CubismMotionController>();
        _expressionController = GetComponent<CubismExpressionController>();
    }

    public void PlayAnim(int i )
    {
        _motionController.PlayAnimation(_tapBodyMotions[i], isLoop: false, priority:CubismMotionPriority.PriorityNormal);
    }
    public void PlayExpression(int i )
    {
        
        _expressionController.CurrentExpressionIndex = i;
    }
}
