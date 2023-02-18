using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Part 
{
    public class PlayerControl : MonoBehaviour 
    {
        public Collider2D[] ExtractCollider; //Determine whether to interact with the block
        public Collider2D[] KeepDirectionCollider; //Determine whether to filp horizontally 
        public Transform[] GroundCollider;

        public GameObject[] playerCharacter;

        public float speed = 1;
        [Header("Jump Setting")]
        public float Jump = 15;
        public float jumpDuration = 1f;
        
        public float power = 2;

        public Animator[] animator;

        [Header("Root State")] //These three parameters determine the state of the root/player
        public bool isRotating = false;
        public bool isRotated = false;
        [SerializeField] private bool isRight = true;
        [Space]

        private bool overlappedGround = false; //For determining whether the root is covering something later 
        private float lastTimeOnGround;
        private Box overlapedBox;
        private float direction;

        [Header("Audio Setting")]
        public AudioSource jumpAudio;

        [Header("Rotate Setting")]
        public float rotateDuration = 1;

        //When the root faces to the left and starts rotating, its position.x needs to add or minus this value
        [SerializeField] private float RotateOffset = 0.923f; 
        private float rotateStart;
        [Space]

        public float lastTimeJump = 0;

        public GameObject diecanvas;
        ContactFilter2D filter;

        public bool IsRight { get { return isRight; } set { if (isRight == value) return; if (!overlappedGround) { isRight = value; UpdateState(); } } }
        public float LastTimeOnGround { get { return lastTimeOnGround; } set { lastTimeOnGround = value; } }

        
        private void Start()
        {
            filter.layerMask = LayerMask.GetMask("Ground"); // For collider detection later
            UpdateState();
        }

        private void Update()
        {
            CheckDie();

            overlapedBox = null;
            overlappedGround = false;

            CheckOverlap();
            CheckOnGround();
            MoveControl();
            ExtendBox();
        }

        public void ExtendBox()
        {
            if (!isRotating)
            {
                var triggerResult = new List<Collider2D>();
                Physics2D.OverlapCollider(ExtractCollider[GetIndex()], filter, triggerResult);

                foreach (var c in triggerResult)
                {
                    if (c.gameObject.TryGetComponent(out Box box))
                    {
                        box.Extend(GetDirection(), power); //Change interactive block (Box)
                    }
                }
            }
        }

        public bool IsRotated 
        {
            get 
            {
                return isRotated;
            }
            set 
            {
                if (isRotated == value) 
                {
                    return;
                }
                if (!overlappedGround) 
                {
                    isRotated = value;
                    if (value) 
                    {
                        animator[0].Play("PlayerRotate");
                        //Debug.Log("Player is rotating.");
                    } 
                    else 
                    {
                        if (IsRight)
                            playerCharacter[0].transform.position = playerCharacter[1].transform.position - new Vector3(RotateOffset, 0, 0);
                        else
                            playerCharacter[0].transform.position = playerCharacter[1].transform.position + new Vector3(RotateOffset, 0, 0);

                        playerCharacter[0].SetActive(true);
                        playerCharacter[1].SetActive(false);
                        animator[0].Play("RotateBack");
                    }
                    isRotating = true;
                    rotateStart = Time.time;

                    Coroutine coroutine = StartCoroutine(Rotate());
                    UpdateState();
                }
            }
        }

        //Determine the direction of the block change according to the state of the root sign
        private Vector2 GetDirection() 
        {
            Vector2 ans;
            if (isRotated)
            {
                if (isRight) 
                {
                    ans = new Vector2(-1, 0);
                } else {
                    ans = new Vector2(1, 0);
                }
            } 
            else 
            {
                ans = new Vector2(0, 1);
            }
            return ans;
        }

        public void MoveControl() 
        {
            if (isRotating) 
            {
                return;
            }

            direction = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            if (direction < 0) // To avoid errors, in some situations, the root cannot flip itself
            {
                if (IsRotated && playerCharacter[GetIndex()].GetComponent<CheckWall>().CheckWallObstacle(1)) { }
                else if (overlapedBox || overlappedGround) { }
                else if (!IsRotated && IsRight && playerCharacter[GetIndex()].GetComponent<CheckWall>().CheckWallObstacle(-1)) { }
                //direction = 0;
                else
                    IsRight = false;
            } 
            else if (direction > 0) 
            {
                if (IsRotated && playerCharacter[GetIndex()].GetComponent<CheckWall>().CheckWallObstacle(-1)) { }
                else if (overlapedBox || overlappedGround) { }
                else if (!IsRotated && !IsRight && playerCharacter[GetIndex()].GetComponent<CheckWall>().CheckWallObstacle(1)) { }
                //direction = 0;
                else
                    IsRight = true;                                  
            }

            Vector3 v = GetComponent<Rigidbody2D>().velocity;
            v.x = speed * direction;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) 
            {      
                if (Time.time > lastTimeJump + 0.5f && Time.time < lastTimeOnGround + 0.01f) 
                {
                    v.y = Jump;
                    lastTimeJump = Time.time;
                    jumpAudio.Play();
                }
            }
            GetComponent<Rigidbody2D>().velocity = v;
            
            animator[GetIndex()].SetFloat("speed", v.magnitude);

            if (Input.GetKeyDown(KeyCode.R)) 
            {
                IsRotated = !IsRotated;
            }
        }

        public void UpdateState() 
        {
            int s = (IsRight ? 0 : 1) | (IsRotated ? 2 : 0);
            ExtractCollider[GetIndex()].transform.localScale = new Vector3(1f, 1f, 1);
            Vector3 flip = new Vector3(isRight ? 1 : -1, 1, 1);
            
            playerCharacter[GetIndex()].transform.localScale = flip; //Flip the root horizontally
        }

        // Determine whether the root is covering something
        public void CheckOverlap() 
        {
            if (KeepDirectionCollider[GetIndex()].isTrigger)
            {
                List<Collider2D> result = new List<Collider2D>();
                Physics2D.OverlapCollider(KeepDirectionCollider[GetIndex()], filter, result);
                foreach (var c in result)
                {
                    if (c.gameObject.layer == 6)
                    {
                        overlappedGround = true;
                    }
                    if (c.gameObject.TryGetComponent(out Box box))
                    {
                        overlapedBox = box;
                        if (box.state == Box.BoxState.Negative)
                        {
                            Die();
                        }
                    }
                }
            }
        }

        public void CheckDie() 
        {
            if (transform.position.y < -17) 
            {
                Die();
            }
        }

        public void CheckOnGround() 
        {
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = LayerMask.GetMask("Ground");
            var groundCollider = GroundCollider[GetIndex()].GetComponents(typeof(Collider2D));
            foreach (var component in groundCollider) 
            {
                var c = (Collider2D) component;
                List<Collider2D> result = new List<Collider2D>();
                Physics2D.OverlapCollider(c, contactFilter2D, result);
                foreach (var res in result) 
                {
                    if (res.transform.IsChildOf(transform)) 
                    {
                        continue;
                    }
                    lastTimeOnGround = Time.time;
                    return;
                }
            }
        }

        IEnumerator Rotate() 
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            while (Time.time < rotateStart + rotateDuration) 
            {
                yield return null;
            }
            isRotating = false;
            if (IsRotated)
            {
                if (IsRight)
                    playerCharacter[1].transform.position = playerCharacter[0].transform.position + new Vector3(RotateOffset, 0, 0);
                else
                    playerCharacter[1].transform.position = playerCharacter[0].transform.position - new Vector3(RotateOffset, 0, 0);

                playerCharacter[1].SetActive(true);
                playerCharacter[0].SetActive(false);
            }
            /*else
            {
                //Since the reverse animation is attached on the PlayerH, offsetting & setting activity were conducted before this fuction
                playerCharacter[1].SetActive(false);
                playerCharacter[0].SetActive(true); 
            }*/

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        // Return the index of player game object which is active 
        int GetIndex()
        {
            if (IsRotated) return 1;
            else return 0;
        }

        public void Die() 
        {
            var Canvas = diecanvas;
            Canvas.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}