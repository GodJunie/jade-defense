using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace B409.Jade.Battle {
    using Data;

    public class UnitController : MonoBehaviour {
        // 애니메이션 세팅
        [SerializeField]
        private Direction defaultDirection;
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

        // Events
        public Action OnDead;

        public State State { get; private set; }
        public UnitData Data { get; private set; }
        public float Hp { get; private set; }
        public bool IsPlayer { get; private set; }

        // about slow
        private float moveSpeed;
        private List<Slow> slows = new List<Slow>();
        // about dot
        private List<DamageOverTime> dots = new List<DamageOverTime>();
     
        private List<UnitController> targets = new List<UnitController>();
        private List<UnitController> attackTargets = new List<UnitController>();


        #region Mono
        private void Awake() {
            detector.OnEnter = () => {
                this.targets = detector.Targets.Where(e => e != null && this.Data.Status.TargetEnemy == (e.IsPlayer ^ this.IsPlayer) && e.State != State.Die && e != this).ToList();

                if(targets.Count > 0) {
                    if(this.State == State.Move)
                        ChangeState(State.Attack);
                }
            };

            detector.OnExit = () => {
                this.targets = detector.Targets.Where(e => e != null && this.Data.Status.TargetEnemy == (e.IsPlayer ^ this.IsPlayer) && e.State != State.Die && e != this).ToList();

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
                        if(target == null) continue;

                        switch(Data.Status.AttackMode) {
                        case AttackMode.Attack:
                            target.OnDamage(this.Data.Status.Atk);
                            break;
                        case AttackMode.DamageOverTime:
                            target.OnDamageOverTime(this.Data.Status.Atk, this.Data.Status.Duration, this.Data.Status.Interval);
                            break;
                        case AttackMode.Heal:
                            target.OnHeal(this.Data.Status.Heal);
                            break;
                        case AttackMode.Stun:
                            target.OnStun(this.Data.Status.Atk, this.Data.Status.Duration);
                            break;
                        case AttackMode.Slow:
                            target.OnSlow(this.Data.Status.Atk, this.Data.Status.Duration, this.Data.Status.SlowRate);
                            break;
                        default:
                            break;
                        }
                    }
                }
            };

            anim.AnimationState.End += (entry) => {
                if(entry.Animation.Name == dieAnimation) {
                    Destroy(gameObject);
                }
            };

            anim.AnimationState.Complete += (entry) => {
                Debug.Log(entry.Animation.Name);
                if(entry.Animation.Name == attackAnimation) {
                    attackEnd = true;
                    anim.AnimationState.SetAnimation(0, "Idle", true);
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
            case State.Stun:
                Stun();
                break;
            case State.Die:
                Die();
                break;
            default:
                break;
            }

            if(this.State != State.Die) {
                if(slows.Count > 0) {
                    foreach(var slow in slows) {
                        slow.Tick();
                    }

                    slows = slows.Where(e => e.Duration > 0).ToList();
                    var maxSlowRate = slows.Select(e => e.SlowRate).Max();
                    this.moveSpeed = this.Data.Status.MoveSpeed * (1 - maxSlowRate);
                } else {
                    this.moveSpeed = this.Data.Status.MoveSpeed;
                }

                if(dots.Count > 0) {
                    foreach(var dot in dots) {
                        if(dot.Tick()) {
                            OnDamage(dot.Damage);
                        }
                    }

                    dots = dots.Where(e => e.Duration > 0).ToList();
                }
            }
        }
        #endregion


        #region Data
        public void Init(UnitData data, bool isPlayer) {
            this.Data = data;
            this.IsPlayer = isPlayer;

            this.detector.transform.localPosition = new Vector3(this.hitbox.offset.x * this.hitbox.transform.localScale.x, 0, 0f);
            this.detector.SetRange(this.hitbox.size.x * this.hitbox.transform.localScale.x + data.Status.Range);

            this.Hp = data.Status.Hp;
            this.moveSpeed = data.Status.MoveSpeed;
            this.hpBar.Init(data, isPlayer);

            if(isPlayer == (defaultDirection == Direction.Left)) {
                this.anim.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }

            this.ChangeState(State.Move);
        }

        public void OnDamage(float damage) {
            if(this.State == State.Die) 
                return;

            this.Hp = Mathf.Clamp(this.Hp - damage, 0f, this.Data.Status.Hp);
            this.hpBar.SetHp(this.Hp);
            if(this.Hp == 0)
                ChangeState(State.Die);
        }

        public void OnHeal(float heal) {
            if(this.State == State.Die)
                return;

            this.Hp = Mathf.Clamp(this.Hp + heal, 0f, this.Data.Status.Hp);
            this.hpBar.SetHp(this.Hp);
        }

        public void OnStun(float damage, float duration) {
            if(this.State == State.Die)
                return;

            this.Hp = Mathf.Clamp(this.Hp - damage, 0f, this.Data.Status.Hp);
            this.hpBar.SetHp(this.Hp);
            if(this.Hp == 0) {
                ChangeState(State.Die);
                return;
            }

            this.stunDuration = duration;

            ChangeState(State.Stun);
        }

        public void OnSlow(float damage, float duration, float slowRate) {
            if(this.State == State.Die)
                return;

            this.Hp = Mathf.Clamp(this.Hp - damage, 0f, this.Data.Status.Hp);
            this.hpBar.SetHp(this.Hp);
            if(this.Hp == 0) {
                ChangeState(State.Die);
                return;
            }

            this.slows.Add(new Slow(duration, slowRate));
        }

        public void OnDamageOverTime(float damage, float duration, float interval) {
            this.dots.Add(new DamageOverTime(damage, duration, interval));
        }
        #endregion

        #region State
        private void CheckState() {
            if(this.targets.Count > 0) {
                ChangeState(State.Attack);
            } else {
                ChangeState(State.Move);
            }
        }

        private void ChangeState(State state) {
            if(this.State == State.Die)
                return;

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
                this.transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * this.moveSpeed;
            } else {
                this.transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * this.moveSpeed;
            }
        }

        private void MoveExit() {

        }
        #endregion

        #region Attack
        private float attackTimer;
        private bool attackEnd;

        private void AttackEnter() {
            attackTimer = this.Data.Status.AttackSpeed;
            attackEnd = false;

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
            if(Data.Status.TargetCount > 0) attackTargets = attackTargets.GetRange(0, Mathf.Min(attackTargets.Count, Data.Status.TargetCount));

            float duration = anim.skeleton.Data.FindAnimation(attackAnimation).Duration;
            float timeScale = Mathf.Max(1f, duration / this.Data.Status.AttackSpeed);

            anim.AnimationState.SetAnimation(0, attackAnimation, false).TimeScale = timeScale;
        }

        private void Attack() {
            attackTimer -= Time.deltaTime;

            if(attackTimer < 0f && attackEnd) {
                // 공격 끝
                CheckState();
            }
        }

        private void AttackExit() {

        }
        #endregion

        #region Stun
        private float stunDuration;
        private float knockBackSpeed;

        private void StunEnter() {

        }

        private void Stun() {
            stunDuration -= Time.deltaTime;

            if(IsPlayer) {
                this.transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * this.knockBackSpeed;
            } else {
                this.transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * this.knockBackSpeed;
            }

            if(stunDuration < 0) {
                CheckState();
            }
        }

        private void StunExit() {

        }
        #endregion

        #region Die
        private void DieEnter() {
            anim.AnimationState.SetAnimation(0, dieAnimation, false);
            this.hitbox.enabled = false;
            this.OnDead?.Invoke();
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