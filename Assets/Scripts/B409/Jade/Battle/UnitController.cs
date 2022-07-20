using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    using Data;

    public class UnitController : MonoBehaviour {
        // 설정
        //[SerializeField]
        //private TargetFilterMode targetFilter;
        //[SerializeField]
        //private bool descending;
        //[SerializeField]
        //private int targetCount;
        //[SerializeField]
        //private bool targetEnemy;

        // 애니메이션 세팅
        [ValueDropdown("animations")]
        [SerializeField]
        private string moveAnimation;
        [ValueDropdown("animations")]
        [SerializeField]
        private string attackAnimation;
        [ValueDropdown("animations")]
        [SerializeField]
        private string dieAnimation;

        // 오브젝트 구성
        [SerializeField]
        private BoxCollider2D hitbox;
        [SerializeField]
        private Detector detector;
        [SerializeField]
        private SkeletonAnimation anim;
        [SerializeField]
        private HpBar hpBar;

        public State State { get; private set; }
        public UnitData Data { get; private set; }
        public float Hp { get; private set; }
        public bool IsPlayer { get; private set; }

        private List<UnitController> targets = new List<UnitController>();
        private List<UnitController> attackTargets = new List<UnitController>();

        #region Data
        public void Init(UnitData data, bool isPlayer) {
            this.Data = data;
            this.IsPlayer = isPlayer;

            this.detector.transform.localPosition = new Vector3(this.hitbox.offset.x * this.hitbox.transform.localScale.x, 0, 0f);
            this.detector.SetRange(this.hitbox.size.x * this.hitbox.transform.localScale.x + data.Status.Range);

            this.Hp = data.Status.Hp;
            this.hpBar.Init(data, isPlayer);
            this.ChangeState(State.Move);
        }

        public void OnDamage(float damage) {
            this.Hp = Mathf.Clamp(this.Hp - damage, 0f, this.Data.Status.Hp);
            if(this.Hp == 0)
                ChangeState(State.Die);
        }
        #endregion

        #region Mono
        private void Awake() {
            detector.OnEnter = () => {
                this.targets = detector.Targets.Where(e => this.Data.Status.TargetEnemy == (e.IsPlayer ^ this.IsPlayer) && e.State != State.Die && e != this).ToList();
                
                if(targets.Count > 0) {
                    if(this.State == State.Move)
                        ChangeState(State.Attack);
                }
            };

            anim.AnimationState.Start += (entry) => {
                
            };

            anim.AnimationState.Event += (entry, e) => {
                if(e.Data.Name == "attackPoint" && entry.Animation.Name == attackAnimation) {
                    // 공격!
                    foreach(var target in attackTargets) {
                        target.OnDamage(this.Data.Status.Atk);
                    }
                }
            };
            
            anim.AnimationState.Complete += (entry) => {
                Debug.Log("A");
                Debug.Log(entry.Animation.Name);
                if(entry.Animation.Name == attackAnimation) {
                    Debug.Log("B");
                    // 공격 끝
                    if(this.targets.Count > 0) {
                        ChangeState(State.Attack);
                        Debug.Log("C");
                    } else {
                        ChangeState(State.Move);
                    }
                }
                if(entry.Animation.Name == dieAnimation) {
                    // 인생 끝!
                    Destroy(this.gameObject);
                }
            };
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            switch(State) {
            case State.Move:
                Move();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                Die();
                break;
            default:
                break;
            }
        }
        #endregion

        #region State
        private void ChangeState(State state) {
            switch(state) {
            case State.Move:
                MoveExit();
                break;
            case State.Attack:
                AttackExit();
                break;
            case State.Die:
                DieExit();
                break;
            default:
                break;
            }

            this.State = state;

            switch(state) {
            case State.Move:
                MoveEnter();
                break;
            case State.Attack:
                AttackEnter();
                break;
            case State.Die:
                DieEnter();
                break;
            default:
                break;
            }
        }

        #region Move
        private void MoveEnter() {
            anim.AnimationState.SetAnimation(0, moveAnimation, true);
        }

        private void Move() {
            if(IsPlayer) {
                this.transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * this.Data.Status.MoveSpeed;
            } else {
                this.transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * this.Data.Status.MoveSpeed;
            }
        }

        private void MoveExit() {

        }
        #endregion

        #region Attack
        private void AttackEnter() {
            switch(Data.Status.TargetFilterMode) {
            case TargetFilterMode.Hp:
                attackTargets = targets.OrderBy(e => e.Hp).ToList();
                break;
            case TargetFilterMode.MaxHp:
                attackTargets = targets.OrderBy(e => e.Data.Status.Hp).ToList();
                break;
            case TargetFilterMode.Distance:
                attackTargets = targets.OrderBy(e => Mathf.Abs(this.transform.position.x - e.transform.position.x)).ToList();
                break;
            case TargetFilterMode.Index:
                attackTargets = targets.ToList();
                break;
            default:
                break;
            }

            if(Data.Status.Descending) attackTargets.Reverse();
            if(Data.Status.TargetCount > 0) attackTargets = attackTargets.GetRange(0, Data.Status.TargetCount);

            anim.AnimationState.SetAnimation(0, attackAnimation, false);
        }

        private void Attack() {
            
        }

        private void AttackExit() {

        }
        #endregion

        #region Die
        private void DieEnter() {
            anim.AnimationState.SetAnimation(0, dieAnimation, false);
        }

        private void Die() {

        }

        private void DieExit() {

        }
        #endregion
        #endregion

        #region Editor
        #if UNITY_EDITOR
        public string[] animations {
            get {
                return this.anim.skeleton.Data.Animations.Select(e => e.Name).ToArray();
            }
        }

        [Button]
        public void InitObjects() {
            this.hitbox = GetComponentInChildren<BoxCollider2D>();
            this.detector = GetComponentInChildren<Detector>();
            this.anim = GetComponentInChildren<SkeletonAnimation>();
            this.hpBar = GetComponentInChildren<HpBar>();
        }
        #endif
        #endregion
    }
}