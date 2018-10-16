using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following : MonoBehaviour {

    [SerializeField]
    public Transform _unityChansTransform;

    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    public Animator _animator;

    /// <summary>
    /// 開始時
    /// </summary>
	void Start () {
        if (!_navMeshAgent) {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
        }

        if (!_animator) {
            _animator = this.GetComponent<Animator>();
        }
	}
	
    /// <summary>
    /// 更新時
    /// </summary>
	void Update () {
        if (!_unityChansTransform) {
            return;
        }

        // 目的地と自分の位置との距離
        Vector3 dir = _unityChansTransform.transform.position - this.transform.position;

        // 目的地の位置
        Vector3 pos = this.transform.position + dir * 1.5f;

        // 目的地の方を向く
        this.transform.rotation = Quaternion.LookRotation(dir);

        // 目的地を指定する
        _navMeshAgent.destination = pos;

        // 目的地からどれくらい離れて停止するか
        _navMeshAgent.stoppingDistance = 3f;

        // Agentの速度の二乗の数値でアニメーションを切り替える
        _animator.SetFloat("Speed", _navMeshAgent.velocity.sqrMagnitude);
    }
}
