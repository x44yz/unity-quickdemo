// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class InteractObj : MonoBehaviour
// {
//     public bool handleInput;
//     public 

//     private Interactable currentInteractable;

//     // This function is called by the EventTrigger on an Interactable, the Interactable component is passed into it.
//     public void OnInteractableClick(Interactable interactable)
//     {
//         // If the handle input flag is set to false then do nothing.
//         if(!handleInput)
//             return;

//         // Store the interactble that was clicked on.
//         currentInteractable = interactable;

//         // // Set the destination to the interaction location of the interactable.
//         // destinationPosition = currentInteractable.interactionLocation.position;

//         // // Set the destination of the nav mesh agent to the found destination position and start the nav mesh agent going.
//         // agent.SetDestination(destinationPosition);
//         // agent.Resume ();
//     }
// }
