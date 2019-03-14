using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_DragonGroundToFly : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//animator.gameObject.GetComponent<SFB_DragonHeight> ().GroundToFlying ();
        
		Vector3 newHeight = new Vector3 (animator.gameObject.transform.position.x, 5.0f, animator.gameObject.transform.position.z);
		animator.gameObject.transform.position = newHeight;
		Debug.Log ("New Height: " + animator.gameObject.transform.position);
		animator.SetTrigger ("goAir");
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
