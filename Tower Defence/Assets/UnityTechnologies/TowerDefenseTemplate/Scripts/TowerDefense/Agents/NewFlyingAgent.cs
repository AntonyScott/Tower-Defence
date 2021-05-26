using UnityEngine;
using UnityEngine.AI;

namespace TowerDefense.Agents
{

    public class NewFlyingAgent : Agent
    {
        protected float m_WaitTime = 0.5f;

        protected float m_CurrentWaitTime;

        protected override void OnPartialPathUpdate()
        {
            if (!isPathBlocked)
            {
                state = State.OnCompletePath;
                return;
            }
            if (!isAtDestination)
            {
                return;
            }
            m_NavMeshAgent.enabled = false;
            m_CurrentWaitTime = m_WaitTime;
            state = State.PushingThrough;
        }

        protected override void PathUpdate()
        {
            switch (state)
            {
                case State.OnCompletePath:
                    OnCompletePathUpdate();
                    break;
                case State.OnPartialPath:
                    OnPartialPathUpdate();
                    break;
                case State.PushingThrough:
                    PushingThrough();
                    break;
            }
        }
        protected void PushingThrough()
        {
            m_CurrentWaitTime -= Time.deltaTime;

            transform.LookAt(m_Destination, Vector3.up);
            transform.position += transform.forward * m_NavMeshAgent.speed * Time.deltaTime;
            if(m_CurrentWaitTime > 0)
            {
                return;
            }

            NavMeshHit hit;
            if (!NavMesh.Raycast(transform.position + Vector3.up, Vector3.down, out hit, navMeshMask))
            {
                m_CurrentWaitTime = m_WaitTime;
            }
            else
            {
                m_NavMeshAgent.enabled = true;
                NavigateTo(m_Destination);
                state = isPathBlocked ? State.OnPartialPath : State.OnCompletePath;
            }
        }
    }
}