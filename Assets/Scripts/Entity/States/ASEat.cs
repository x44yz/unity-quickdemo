// using UnityEngine;
// using AI.FSM;

// public class ASEat : ActorState
// {
//     private float eatTick = 0f;
//     private ItemId eatItemId;
//     private int eatCount;

//     public ASEat(Actor actor) : base(actor)
//     {
        
//     }

//     public override void OnEnter(State from)
//     {
//         base.OnEnter(from);

//         eatTick = 0f;
//         eatItemId = ItemId.NONE;

//         // check food enough 
//         var foods = shelter.storage.GetItems(ItemType.FOOD);
//         if (foods == null || foods.Count == 0)
//         {
//             Debug.LogWarning("not food");
//             ai.ChangeToState<ASIdle>();
//             return;
//         }

//         // 优先级
//         for (int i = 0; i < foods.Count; ++i)
//         {
//             if (foods[i].count > 0)
//             {
//                 eatItemId = foods[i].id;
//                 eatCount = 1;
//                 shelter.storage.RemoveItem(eatItemId, 1);
//                 break;
//             }
//         }

//         if (eatItemId == ItemId.NONE)
//         {
//             Debug.LogWarning("cant find any food");
//             return;
//         }

//         ani.PlayANI(AnimId.EAT);
//     }

//     public override void OnUpdate(float dt)
//     {
//         base.OnUpdate(dt);

//         eatTick += dt;
//         if (eatTick >= GameDef.EAT_TIME)
//         {
//             var cfg = sRes.GetItemCfg(eatItemId);
//             for (int i = 0; i < eatCount; ++i)
//                 actor.ApplyEffects(cfg.useEffects, actor);

//             ai.ChangeToState<ASIdle>();
//         }
//     }
// }