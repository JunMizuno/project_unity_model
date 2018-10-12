using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private const string MOVE_HORIZONTAL = "Horizontal";
    private const string MOVE_VERTICAL = "Vertical";
    private const string ANIMATION_RUN = "Running";
    private const string ANIMATION_JUMP = "Jumping";
    private const string KEY_JUMP = "Jump";

    [SerializeField]
    Rigidbody _rb;

    [Range(1.0f, 9.0f)]
    public float _speed = 3.0f;

    [Range(10.0f, 600.0f)]
    public float _thrust = 200.0f;

    [SerializeField]
    private Animator _animator;

    private Vector3 _playerPos;

    private bool _isGround;

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () {
        if (!_rb) {
            _rb = this.GetComponent<Rigidbody>();
        }

        if (!_animator) {
            _animator = this.GetComponent<Animator>();
        }

        _playerPos = this.transform.position;
	}
	
    /// <summary>
    /// 更新時
    /// </summary>
	void Update () {
        if (_isGround) {
            UpdateAction();
        }
	}

    /// <summary>
    /// アクション判定と更新
    /// </summary>
    private void UpdateAction() {
        // 横移動(A・D及び左右キー)
        float x = Input.GetAxisRaw(MOVE_HORIZONTAL) * Time.deltaTime * _speed;

        // 前後移動(W・S及び上下キー)
        float z = Input.GetAxisRaw(MOVE_VERTICAL) * Time.deltaTime * _speed;

        // 現在の位置＋入力した数値の場所に移動する
        _rb.MovePosition(this.transform.position + new Vector3(x, 0, z));

        // 最新の位置から少し前の位置を引いて方向を割り出す
        Vector3 direction = this.transform.position - _playerPos;

        // 移動距離が少しでもあった場合に方向転換
        if (direction.magnitude > 0.01f) {
            // directionのX軸とZ軸の方向を向かせる
            this.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            // 走るアニメーションを再生
            _animator.SetBool(ANIMATION_RUN, true);
        }
        else {
            // ベクトルの長さがない＝移動していない時は走るアニメーションはオフ
            _animator.SetBool(ANIMATION_RUN, false);
        }

        // ユニティちゃんの位置を更新する
        _playerPos = this.transform.position;

        // スペースキーやゲームパッドの3ボタンでジャンプ
        if (Input.GetButton(KEY_JUMP)) {
            // thrustの分だけ上方に力がかかる
            _rb.AddForce(this.transform.up * _thrust);

            // 速度が出ていたら前方と上方に力がかかる
            if (_rb.velocity.magnitude > 0) {
                _rb.AddForce(this.transform.forward * _thrust + this.transform.up * _thrust);
            }
        }
    }

    /// <summary>
    /// 当たり判定があった場合
    /// </summary>
    /// <param name="col">Col.</param>
    void OnCollisionStay(Collision col) {
        _isGround = true;

        // ジャンプのアニメーションをオフにする
        _animator.SetBool(ANIMATION_JUMP, false);
    }

    /// <summary>
    /// 当たり判定がなくなった場合
    /// </summary>
    /// <param name="col">Col.</param>
    void OnCollisionExit(Collision col) {
        _isGround = false;

        // ジャンプのアニメーションをオンにする
        _animator.SetBool(ANIMATION_JUMP, true);
    }
}
