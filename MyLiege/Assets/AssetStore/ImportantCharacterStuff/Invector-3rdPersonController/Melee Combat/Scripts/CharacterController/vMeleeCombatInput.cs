using Unity.VisualScripting;
using UnityEngine;

namespace Invector.vCharacterController
{
    using vEventSystems;
    using vMelee;

    // here you can modify the Melee Combat inputs
    // if you want to modify the Basic Locomotion inputs, go to the vThirdPersonInput
    [vClassHeader("MELEE INPUT MANAGER", iconName = "inputIcon")]
    public class vMeleeCombatInput : vThirdPersonInput, vIMeleeFighter
    {
        #region Variables

        //Reference to building controller
        private Character_BuildingController CBC;


        [vEditorToolbar("Inputs")] [Header("General Inputs")]
        public GenericInput ChangeCharactersCameraViewInput = new GenericInput("Mouse1", "LT", "LT");

        [Header("Building Inputs")] public GenericInput BuildModeToggle = new GenericInput("Q", "B", "B");
        public GenericInput EditModeToggle = new GenericInput("Q", "B", "B");
        public GenericInput RotateBuildingRight = new GenericInput("Q", "B", "B");
        public GenericInput RotateBuildingLeft = new GenericInput("Q", "B", "B");
        public GenericInput PlaceBuilding = new GenericInput("Q", "B", "B");
        public GenericInput RemoveBuilding = new GenericInput("Q", "B", "B");
        public GenericInput NextBuilding = new GenericInput("Q", "B", "B");
        public GenericInput PreviousBuilding = new GenericInput("Q", "B", "B");

        public GenericInput MoveBuilding = new GenericInput("Q", "B", "B");

        [Header("Melee Inputs")] public GenericInput weakAttackInput = new GenericInput("Mouse0", "RB", "RB");
        public GenericInput strongAttackInput = new GenericInput("Alpha1", false, "RT", true, "RT", false);
        public GenericInput blockInput = new GenericInput("Mouse1", "LB", "LB");

        internal vMeleeManager meleeManager;
        protected virtual bool _isAttacking { get; set; }

        public virtual bool isAttacking
        {
            get => _isAttacking || cc.IsAnimatorTag("Attack");
            protected set { _isAttacking = value; }
        }

        public virtual bool isBlocking { get; protected set; }

        public virtual bool isArmed
        {
            get
            {
                return meleeManager != null && (meleeManager.rightWeapon != null || (meleeManager.leftWeapon != null &&
                    meleeManager.leftWeapon.meleeType != vMeleeType.OnlyDefense));
            }
        }

        public virtual bool isEquipping { get; protected set; }
        public virtual bool isFirstPerson { get; protected set; } = true;

        [HideInInspector] public bool lockMeleeInput;

        public void SetLockMeleeInput(bool value)
        {
            lockMeleeInput = value;

            if (value)
            {
                isAttacking = false;
                isBlocking = false;
            }
        }

        public override void SetLockAllInput(bool value)
        {
            base.SetLockAllInput(value);
            SetLockMeleeInput(value);
        }

        #endregion

        public virtual bool lockInventory
        {
            get { return isAttacking || cc.isDead || cc.customAction || cc.isRolling; }
        }

        protected override void Start()
        {
            CBC = gameObject.GetComponent<Character_BuildingController>();
            base.Start();
        }

        protected override void LateUpdate()
        {
            UpdateMeleeAnimations();
            base.LateUpdate();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void InputHandle()
        {
            if (cc == null || cc.isDead)
            {
                return;
            }

            base.InputHandle();

            //Handle the input for change camera.
            ChangeCameraViewInput();

            //Handle the building ui toggle
            ToggleBuildModeInput();
            ToggleEditModeInput();

            PlaceBuildingInput();

            if (CBC.GetBuildMode() && CBC.GetEditMode() && CBC.CurrentBuildingSelected != null &&
                !CBC.GetCurrentlyMoving())
            {
                DeleteBuildingInput();
            }

            if (CBC.GetEditMode() && CBC.GetBuildMode() && CBC.CurrentBuildingSelected != null)
            {
                MoveBuildingInput();
            }

            if (!CBC.GetRotateModeLeft())
            {
                RotateBuildingRightInput();
            }

            if (!CBC.GetRotateModeRight())
            {
                RotateBuildingLeftInput();
            }

            if (MeleeAttackConditions() && !lockMeleeInput && !CBC.GetBuildMode())
            {
                MeleeWeakAttackInput();
                MeleeStrongAttackInput();
                BlockingInput();
            }
            else
            {
                ResetAttackTriggers();
                isBlocking = false;
            }
        }

        public void RotateBuildingRightInput()
        {
            if (!CBC.GetBuildMode())
            {
                return;
            }

            if (RotateBuildingRight.GetButton() && CBC.GetBuildMode())
            {
                CBC.RotateBuilding(false);
                CBC.SetRotationRight(true);
            }
            else
            {
                CBC.SetRotationRight(false);
            }
        }

        public void RotateBuildingLeftInput()
        {
            if (!CBC.GetBuildMode())
            {
                return;
            }

            if (RotateBuildingLeft.GetButton() && CBC.GetBuildMode())
            {
                CBC.SetRotationLeft(true);
                CBC.RotateBuilding(true);
            }
            else
            {
                CBC.SetRotationLeft(false);
            }
        }

        private void MoveBuildingInput()
        {
            if (MoveBuilding.GetButtonDown() && !CBC.GetCurrentlyMoving())
            {
                CBC.SetCurrentlyMoving(true);
                CBC.CurrentBuildingSelected.gameObject.layer = LayerMask.NameToLayer("Moving");
                CBC.CurrentBuildingSelected.GetComponent<BoxCollider>().enabled = false;
                Debug.Log("moving this building, and reset the layer to moving");
            }
            else if (MoveBuilding.GetButtonDown() && CBC.GetCurrentlyMoving() && CBC.GetAllowedNewPosition())
            {
                Debug.Log("Stopped moving this building, and reset the layer to building");
                CBC.SetCurrentlyMoving(false);
                CBC.CurrentBuildingSelected.gameObject.layer = LayerMask.NameToLayer("Building");
                CBC.CurrentBuildingSelected.GetComponent<BoxCollider>().enabled = true;
            }
            else if (MoveBuilding.GetButtonDown() && CBC.GetCurrentlyMoving() && !CBC.GetAllowedNewPosition())
            {
                Debug.Log("Sorry but that is not a valid location for the new position");
            }
        }

        public void DeleteBuildingInput()
        {
            if (RemoveBuilding.GetButtonDown() && CBC.CurrentBuildingSelected != null && !CBC.GetCurrentlyMoving())
            {
                CBC.DeleteBuilding();
            }
        }

        public void ToggleEditModeInput()
        {
            if (!CBC.GetBuildMode())
            {
                return;
            }

            if (EditModeToggle.GetButtonDown() && !CBC.GetEditMode())
            {
                Debug.Log("Enabling Edit Mode");
                CBC.SetEditMode(true);
                CBC.PreviewBuildingPoint.SetActive(false);
                CBC.SetCurrentlyMoving(false);
                CBC.SetAllowedNewPosition(false);
            }
            else if (EditModeToggle.GetButtonDown() && CBC.GetEditMode() && CBC.GetAllowedNewPosition())
            {
                Debug.Log("Disabling Edit Mode");
                CBC.SetEditMode(false);
                CBC.PreviewBuildingPoint.SetActive(true);
                CBC.SetCurrentlyMoving(false);
                CBC.SetAllowedNewPosition(false);
            }
        }

        public void ToggleBuildModeInput()
        {
            if (BuildModeToggle.GetButtonDown() && !CBC.GetBuildMode())
            {
                Debug.Log("Enabling Build Mode");
                CBC.SetBuildMode(true);
                CBC.PreviewBuildingPoint.SetActive(true);

                //if we forgot to switch the layer back this does it for us
                if (CBC.CurrentBuildingSelected != null)
                {
                    CBC.CurrentBuildingSelected.gameObject.layer = LayerMask.NameToLayer("Building");
                }
            }
            else if (BuildModeToggle.GetButtonDown() && CBC.GetBuildMode() && CBC.GetAllowedNewPosition())
            {
                Debug.Log("Disabling Build Mode");
                CBC.SetBuildMode(false);
                //We also want to reset edit mode when we are not in build mode anymore.
                CBC.SetEditMode(false);
                CBC.PreviewBuildingPoint.SetActive(false);
                //if we forgot to switch the layer back this does it for us
                if (CBC.CurrentBuildingSelected != null)
                {
                    CBC.CurrentBuildingSelected.gameObject.layer = LayerMask.NameToLayer("Building");
                }
            }
        }

        //when we enable the edit mode we need to disable the preview building point

        public void PlaceBuildingInput()
        {
            if (!CBC.GetBuildMode())
            {
                return;
            }

            if ((CBC.GetBuildMode() && !CBC.GetEditMode()) && PlaceBuilding.GetButtonDown())
            {
                CBC.PlaceDownBuilding();
            }
        }


        #region MeleeCombat Input Methods

        /// <summary>
        /// WEAK ATK INPUT
        /// </summary>
        public virtual void MeleeWeakAttackInput()
        {
            if (animator == null)
            {
                return;
            }

            if (weakAttackInput.GetButtonDown() && MeleeAttackStaminaConditions())
            {
                TriggerWeakAttack();
            }
        }

        public virtual void TriggerWeakAttack()
        {
            animator.SetInteger(vAnimatorParameters.AttackID, AttackID);
            animator.SetTrigger(vAnimatorParameters.WeakAttack);
        }

        /// <summary>
        /// STRONG ATK INPUT
        /// </summary>
        public virtual void MeleeStrongAttackInput()
        {
            if (animator == null)
            {
                return;
            }

            if (strongAttackInput.GetButtonDown() &&
                (!meleeManager.CurrentActiveAttackWeapon || meleeManager.CurrentActiveAttackWeapon.useStrongAttack) &&
                MeleeAttackStaminaConditions())
            {
                TriggerStrongAttack();
            }
        }

        public virtual void TriggerStrongAttack()
        {
            animator.SetInteger(vAnimatorParameters.AttackID, AttackID);
            animator.SetTrigger(vAnimatorParameters.StrongAttack);
        }

        /// <summary>
        /// BLOCK INPUT
        /// </summary>
        public virtual void BlockingInput()
        {
            if (animator == null)
            {
                return;
            }

            isBlocking = blockInput.GetButton() && cc.currentStamina > 0 && !cc.customAction && !isAttacking;
        }

        /// <summary>
        /// Change Camera View Input - this checks to see if the conditions for the input are true before actually triggering the change.
        /// </summary>
        public virtual void ChangeCameraViewInput()
        {
            if (animator == null)
            {
                return;
            }

            if (tpCamera.currentState != tpCamera.lerpState)
            {
            }
            else if (tpCamera.currentState == tpCamera.currentState)
            {
            }

            if (ChangeCharactersCameraViewInput.GetButtonDown())
            {
                TriggersCharactersCameraViewChange();
            }
        }

        //Change the camera state from the camera managers to use the different camera, if the camera is already in top down, swap back to the normal fps cam.
        public virtual void TriggersCharactersCameraViewChange()
        {
            if (isFirstPerson)
            {
                Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                isFirstPerson = false;
                Debug.Log("Changing the camera to top down");
                ChangeCameraState("Soldier_TopDown", false);
            }
            else
            {
                Debug.Log("changing the camera to first person");
                ChangeCameraState("Soldier_FirstPerson", false);
                isFirstPerson = true;
            }
        }

        /// <summary>
        /// Override the Sprint method to cancel Sprinting when Attacking
        /// </summary>
        public override void SprintInput()
        {
            if (sprintInput.useInput)
            {
                bool canSprint = cc.useContinuousSprint ? sprintInput.GetButtonDown() : sprintInput.GetButton();
                cc.Sprint(canSprint && !isAttacking);
            }
        }

        #endregion

        #region Conditions

        protected virtual bool MeleeAttackStaminaConditions()
        {
            var result = cc.currentStamina - meleeManager.GetAttackStaminaCost();
            return result >= 0;
        }

        protected virtual bool MeleeAttackConditions()
        {
            if (meleeManager == null)
            {
                meleeManager = GetComponent<vMeleeManager>();
            }

            return meleeManager != null && cc.isGrounded && !cc.customAction && !cc.isJumping && !cc.isCrouching &&
                   !cc.isRolling && !isEquipping && !animator.IsInTransition(cc.baseLayer);
        }

        public override bool JumpConditions()
        {
            return !isAttacking && base.JumpConditions();
        }

        public override bool RollConditions()
        {
            return base.RollConditions() && !isAttacking && !animator.IsInTransition(cc.upperBodyLayer) &&
                   !animator.IsInTransition(cc.fullbodyLayer);
        }

        #endregion

        #region Update Animations

        protected virtual void UpdateMeleeAnimations()
        {
            if (animator == null || meleeManager == null)
            {
                return;
            }

            animator.SetInteger(vAnimatorParameters.AttackID, AttackID);
            animator.SetInteger(vAnimatorParameters.DefenseID, DefenseID);
            animator.SetBool(vAnimatorParameters.IsBlocking, isBlocking);
            animator.SetFloat(vAnimatorParameters.MoveSet_ID, meleeMoveSetID, .2f, vTime.fixedDeltaTime);
            isEquipping = cc.IsAnimatorTag("IsEquipping");
        }

        /// <summary>
        /// Default moveset id used when is without weapon
        /// </summary>
        public virtual int defaultMoveSetID { get; set; }

        /// <summary>
        /// Used to ignore the Weapon moveset id and use the <seealso cref="defaultMoveSetID"/>
        /// </summary>
        public virtual bool overrideWeaponMoveSetID { get; set; }

        /// <summary>
        /// Current moveset id based if is using weapon or not
        /// </summary>
        public virtual int meleeMoveSetID
        {
            get
            {
                int id = meleeManager.GetMoveSetID();
                if (id == 0 || overrideWeaponMoveSetID)
                {
                    id = defaultMoveSetID;
                }

                return id;
            }
        }

        public virtual void ResetMeleeAnimations()
        {
            if (meleeManager == null || !animator)
            {
                return;
            }

            animator.SetBool(vAnimatorParameters.IsBlocking, false);
        }

        public virtual int AttackID
        {
            get { return meleeManager ? meleeManager.GetAttackID() : 0; }
        }

        public virtual int DefenseID
        {
            get { return meleeManager ? meleeManager.GetDefenseID() : 0; }
        }

        #endregion

        #region Melee Methods

        public virtual void OnEnableAttack()
        {
            if (meleeManager == null)
            {
                meleeManager = GetComponent<vMeleeManager>();
            }

            if (meleeManager == null)
            {
                return;
            }

            cc.currentStaminaRecoveryDelay = meleeManager.GetAttackStaminaRecoveryDelay();
            cc.currentStamina -= meleeManager.GetAttackStaminaCost();
            isAttacking = true;
            cc.isSprinting = false;
        }

        public virtual void OnDisableAttack()
        {
            isAttacking = false;
        }

        public virtual void ResetAttackTriggers()
        {
            animator.ResetTrigger(vAnimatorParameters.WeakAttack);
            animator.ResetTrigger(vAnimatorParameters.StrongAttack);
        }

        public virtual void BreakAttack(int breakAtkID)
        {
            ResetAttackTriggers();
            OnRecoil(breakAtkID);
        }

        public virtual void OnRecoil(int recoilID)
        {
            animator.SetInteger(vAnimatorParameters.RecoilID, recoilID);
            animator.SetTrigger(vAnimatorParameters.TriggerRecoil);
            animator.SetTrigger(vAnimatorParameters.ResetState);
            animator.ResetTrigger(vAnimatorParameters.WeakAttack);
            animator.ResetTrigger(vAnimatorParameters.StrongAttack);
        }

        public virtual void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
        {
            // character is blocking
            if (!damage.ignoreDefense && isBlocking && meleeManager != null &&
                meleeManager.CanBlockAttack(damage.sender.position))
            {
                var damageReduction = meleeManager.GetDefenseRate();
                if (damageReduction > 0)
                {
                    damage.ReduceDamage(damageReduction);
                }

                if (attacker != null && meleeManager != null && meleeManager.CanBreakAttack())
                {
                    attacker.BreakAttack(meleeManager.GetDefenseRecoilID());
                }

                meleeManager.OnDefense();
                cc.currentStaminaRecoveryDelay = damage.staminaRecoveryDelay;
                cc.currentStamina -= damage.staminaBlockCost;
            }

            // apply damage
            damage.hitReaction = !isBlocking || damage.ignoreDefense;
            cc.TakeDamage(damage);
        }

        public virtual vICharacter character
        {
            get { return cc; }
        }

        #endregion
    }

    public static partial class vAnimatorParameters
    {
        public static int AttackID = Animator.StringToHash("AttackID");
        public static int DefenseID = Animator.StringToHash("DefenseID");
        public static int IsBlocking = Animator.StringToHash("IsBlocking");
        public static int MoveSet_ID = Animator.StringToHash("MoveSet_ID");
        public static int RecoilID = Animator.StringToHash("RecoilID");
        public static int TriggerRecoil = Animator.StringToHash("TriggerRecoil");
        public static int WeakAttack = Animator.StringToHash("WeakAttack");
        public static int StrongAttack = Animator.StringToHash("StrongAttack");
    }
}