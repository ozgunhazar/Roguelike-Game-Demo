using System;
using UnityEngine;

public class Enemy : CellObject
{
   public int Health = 3;
   private int m_CurrentHealth;
   
   private bool m_IsMoving;
   private Vector3 m_MoveTarget;
   public float moveSpeed = 5.0f;
   
   private Animator m_Animator;

   private void Awake()
   {
      m_Animator = GetComponent<Animator>();
      GameManager.Instance.TurnManager.OnTick += TurnHappened;
   }

   private void OnDestroy()
   {
       GameManager.Instance.TurnManager.OnTick -= TurnHappened;
   }

   public override void Init(Vector2Int coord)
   {
      base.Init(coord);
      m_CurrentHealth = Health;
   }

   public override bool PlayerWantsToEnter()
   {
       m_CurrentHealth -= 1;
       m_Animator.SetTrigger("Hurt");

       if (m_CurrentHealth <= 0)
       {
          Destroy(gameObject);
       }

       return false;
   }

   private void Update() {
       
       if (m_IsMoving) {
           
           transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, moveSpeed * Time.deltaTime);
           
           if (transform.position == m_MoveTarget) {
               m_IsMoving = false;
               m_Animator.SetBool("Moving", false);
           }
       }
   }

   bool MoveTo(Vector2Int coord)
   {
       var board = GameManager.Instance.BoardManager;
       var targetCell =  board.GetCellData(coord);

      if (targetCell == null || !targetCell.Passable || targetCell.ContainedObject != null)
      {
          return false;
      }
    
      var currentCell = board.GetCellData(m_Cell);
      currentCell.ContainedObject = null;
    
      targetCell.ContainedObject = this;
      m_Cell = coord;

      m_IsMoving = true;
      m_MoveTarget = board.CellToWorld(coord);

      return true;
   }

   void TurnHappened()
   {
      var playerCell = GameManager.Instance.PlayerController.Cell;

      int xDist = playerCell.x - m_Cell.x;
      int yDist = playerCell.y - m_Cell.y;

      int absXDist = Mathf.Abs(xDist);
      int absYDist = Mathf.Abs(yDist);

      bool didMove = false;
      
      int enemyDamage = 1 + (GameManager.Instance.m_CurrentLevel / 4);


      if ((xDist == 0 && absYDist == 1)
          || (yDist == 0 && absXDist == 1))
      {
          GameManager.Instance.ChangeFood(-enemyDamage);
          m_Animator.SetTrigger("Attack");
      }
      else
      {
          if (absXDist > absYDist)
          {
              if (!TryMoveInX(xDist))
              {
                  didMove = TryMoveInY(yDist);
              }
              else
              {
                  didMove = true;
              }
          }
          else
          {
              if (!TryMoveInY(yDist))
              {
                  didMove = TryMoveInX(xDist);
              }
              else
              {
                  didMove = true;
              }
          }
      }
      
      if (didMove)
      {
          m_Animator.SetBool("Moving", true);
      }

   }

   bool TryMoveInX(int xDist)
   {
      
      if (xDist > 0)
      {
          return MoveTo(m_Cell + Vector2Int.right);
      }
    
      return MoveTo(m_Cell + Vector2Int.left);
   }

   bool TryMoveInY(int yDist)
   {
      
      if (yDist > 0)
      {
          return MoveTo(m_Cell + Vector2Int.up);
      }

      return MoveTo(m_Cell + Vector2Int.down);
   }
}
