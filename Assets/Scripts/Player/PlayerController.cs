using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up
    }
    private Direction dir;

    [Header("Score")]
    public int stepPoint;
    private int pointResult;

    [Header("Jump")]
    public float jumpDistance;
    private float moveDistance;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerInput playerInput;
    private BoxCollider2D coll;
    private Vector2 destination;
    private Vector2 touchPosition;

    private bool buttonHeld;    //若为true 代表按钮被长按了
    private bool isJump;
    private bool canJump;
    private bool isDead;

    //判断碰撞检测返回的物体
    private RaycastHit2D[] result = new RaycastHit2D[2];

    [Header("Direction Sign")]
    public SpriteRenderer signRenderer;
    public Sprite upSign;
    public Sprite leftSign;
    public Sprite rightSign;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        coll = GetComponent<BoxCollider2D>();
        signRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (destination.y - transform.position.y < 0.1f)
        //{
        //    canJump = false;
        //}

        if (isDead)
        {
            DisableInput();
            return;
        }

        if (canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            rb.position = Vector2.Lerp(transform.position, destination, 0.135f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && !isJump)
        {
            //发射射线检测碰撞到的物体
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.1f, Vector2.zero, result);
            bool inWater = true;
            foreach (var hit in result)
            {
                //Debug.Log(hit.collider.tag);
                if (hit.collider == null)
                {
                    continue;
                }
                if (hit.collider.CompareTag("Wood"))
                {
                    Debug.Log("在木板上");
                    //方法一，将玩家设置为木板的子物体，再次跳跃的时候要脱离木头
                    transform.parent = hit.collider.transform;
                    inWater = false;
                }
            }

            if (inWater && !isJump)
            {
                //没有木板，游戏结束
                Debug.Log("In Water GAME OVER! ");
                isDead = true;

            }
        }

        if (collision.CompareTag("Border") || collision.CompareTag("Car"))
        {
            Debug.Log("Game Over! ");
            isDead = true;
        }

        if (!isJump && collision.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! ");
            isDead = true;
        }

        if (isDead)
        {
            //广播通知游戏结束
            EventHandler.CallGameOverEvent();
            coll.enabled = false;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Wood"))
    //    {
    //        transform.parent = null;    //将父级设置为空，就可以回到最外层级
    //    }
    //}

    #region Input输入回调函数

    public void Jump(InputAction.CallbackContext context)
    {
        //执行跳跃，跳跃距离，记录分数，播放跳跃的音效

        if (context.performed && !isJump)  //InputActionPhase.Performed 在执行阶段
        {
            moveDistance = jumpDistance;
            Debug.Log("Jump! " + moveDistance);
            //执行跳跃
            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;

            AudioManager.instance.SetJumpClip(0);
        }

        if (dir == Direction.Up && context.performed && !isJump)
        {
            pointResult += stepPoint;
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump)
        {
            moveDistance = jumpDistance * 2;
            buttonHeld = true;
            //开启方向指示
            signRenderer.gameObject.SetActive(true);
        }

        if (context.canceled && buttonHeld && !isJump)
        {
            //执行跳跃
            Debug.Log("Long Jump! " + moveDistance);
            if (dir == Direction.Up)
            {
                pointResult += stepPoint;
            }
            buttonHeld = false;
            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;

            //手抬起时，播放音效
            AudioManager.instance.SetJumpClip(1);
            //关闭方向指示
            signRenderer.gameObject.SetActive(false);
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //读取终端上点按屏幕的坐标
            //Debug.Log(context.ReadValue<Vector2>());
            touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //Debug.Log(touchPosition);

            //选择方向
            var offset = ((Vector3)touchPosition - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.8f)    //HACK 触摸区域
            {
                dir = Direction.Up;
                signRenderer.sprite = upSign;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
                signRenderer.sprite = leftSign;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
                signRenderer.sprite = rightSign;
            }
        }
    }

    #endregion

    /// <summary>
    /// 触发执行跳跃动画
    /// </summary>
    private void TriggerJump()
    {
        //获得移动方向，播放动画
        canJump = false;

        switch (dir)
        {
            case Direction.Left:
                //触发切换动画
                anim.SetTrigger("JumpLeft");
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                break;
            case Direction.Right:
                anim.SetTrigger("JumpRight");
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                break;
            case Direction.Up:
                anim.SetTrigger("Jump");
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                break;
        }
    }

    #region Animation Event
    public void JumpAnimationEvent()
    {
        //播放跳跃音效
        AudioManager.instance.PlayJumpFX();

        //改变状态
        isJump = true;

        //修改排序图层
        sr.sortingLayerName = "Front";
        Debug.Log(dir);

        //改变父级
        transform.parent = null;
    }

    public void FinishJumpAnimationEvent()
    {
        isJump = false;
        sr.sortingLayerName = "Middle";

        if (dir == Direction.Up && !isDead)
        {
            //得分触发地图检测
            //terrainManager.CheckPosition();

            EventHandler.CallGetPointEvent(pointResult);

            Debug.Log(pointResult);
        }
    }
    #endregion

    private void DisableInput()
    {
        playerInput.enabled = false;
    }
}
