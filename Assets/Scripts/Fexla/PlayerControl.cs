using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mark {
    public class PlayerControl : MonoBehaviour {
        public Collider2D[] extractCollider;
        public Collider2D[] dieColliders;
        public Collider2D[] KeepDirectionCollider;

        public Transform[] GroundCollider;
        public GameObject[] playerCharacter;

        public float speed = 1;
        [Header("Jump Setting")]
        public float Jump = 15;
        public float jumpDuration = 1f;
        public AnimationCurve JumpCurve;
        
        public float power = 2;

        public Animator[] animator;

        [Header("Root State")] //These two parameters determine the state of the root/player
        public bool isRotating = false;
        public bool isRotated = false;
        [Space]

        private bool overlappedGround = false;
        [SerializeField]private bool isRight = true;
        private float lastTimeOnGround;
        private Box overlapedBox;
        private bool overlapped = false;
        private float direction;

        [Header("Audio Setting")]
        public AudioSource footAudio;
        public AudioSource jumpAudio;

        [Header("Rotate Setting")]
        public float rotateDuration = 1;
        [SerializeField] private float RotateOffset = 0.923f;

        private float rotateStart;
        private bool hasOffsetV;
        private bool hasOffsetH;

        public GameObject diecanvas;


        public bool IsRight { get { return isRight; } set { if (isRight == value) return; if (!overlappedGround) { isRight = value; UpdateState(); } } }
        public float LastTimeOnGround { get { return lastTimeOnGround; } set { lastTimeOnGround = value; } }

        public float lastTimeJump = 0;

        ContactFilter2D filter;

        private void Start()
        {
            filter.layerMask = LayerMask.GetMask("Ground");
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
                Physics2D.OverlapCollider(extractCollider[GetIndex()], filter, triggerResult);

                foreach (var c in triggerResult)
                {
                    if (c.gameObject.TryGetComponent(out Box box))
                    {
                        box.Extend(GetDirection(), power); //改变box
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
                        Debug.Log("Player Rotate 1");
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
                    IEnumerator enumerator = Rotate();
                    Coroutine coroutine = StartCoroutine(enumerator);
                    UpdateState();
                }
            }
        }


        private Vector2 GetDirection() 
        {
            Vector2 ans;
            if (isRotated) {
                if (isRight) {
                    ans = new Vector2(-1, 0);
                } else {
                    ans = new Vector2(1, 0);
                }
            } else {
                ans = new Vector2(0, 1);
            }
            return ans;
        }

        public void MoveControl() {
            if (isRotating) 
            {
                return;
            }

            direction = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            if (direction < 0) 
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

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {      
                // v.y += Jump * JumpCurve.Evaluate((Time.time - lastTimeOnGround) / jumpDuration) * Time.deltaTime;
                if (Time.time > lastTimeJump + 0.5f && Time.time < lastTimeOnGround + 0.01f) {
                    v.y = Jump;
                    lastTimeJump = Time.time;
                    // AudioManager.Instance.PlaySE(AudioManager.SoundEffect.Jump);
                    jumpAudio.Play();
                }
            }
            GetComponent<Rigidbody2D>().velocity = v;
            
            animator[GetIndex()].SetFloat("speed", v.magnitude);

            if (Input.GetKeyDown(KeyCode.R)) {
                IsRotated = !IsRotated;
            }
        }

        //Fixme
        public void UpdateState() {
            int s = (IsRight ? 0 : 1) | (IsRotated ? 2 : 0);
            extractCollider[GetIndex()].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            Vector3 flip = new Vector3(isRight ? 1 : -1, 1, 1);
            
            playerCharacter[GetIndex()].transform.localScale = flip;

        }

        public void CheckOverlap() {
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

        public void CheckDie() {
            if (transform.position.y < -17) {
                Die();
            }
            // foreach (var c in dieColliders) {
            //     List<Collider2D> result = new List<Collider2D>();
            //     Physics2D.OverlapCollider(c, filter, result);
            //     foreach (var resC in result) {
            //         if (resC.gameObject.layer == 6) {
            //             Die();
            //             return;
            //         }
            //     }
            // }
        }

        public void CheckOnGround() {
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = LayerMask.GetMask("Ground");
            var groundCollider = GroundCollider[GetIndex()].GetComponents(typeof(Collider2D));
            foreach (var component in groundCollider) {
                var c = (Collider2D) component;
                List<Collider2D> result = new List<Collider2D>();
                Physics2D.OverlapCollider(c, contactFilter2D, result);
                foreach (var res in result) {
                    if (res.transform.IsChildOf(transform)) {
                        continue;
                    }
                    lastTimeOnGround = Time.time;
                    return;
                }
            }
        }

        IEnumerator Rotate() {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            while (Time.time < rotateStart + rotateDuration) {
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
            else
            {
                //Since the reverse animation is attached on the PlayerH, offsetting & setting activity were conducted before this fuction
                //playerCharacter[1].SetActive(false);
                //playerCharacter[0].SetActive(true); 
            }

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        int GetIndex()
        {
            if (IsRotated) return 1;
            else return 0;
        }

        public void Die() {
            var Canvas = diecanvas;
            Canvas.gameObject.SetActive(true);
            Destroy(gameObject);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}