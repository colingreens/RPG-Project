using RPG.Attributes;
using RPG.UI.Inventories;
using System.Text;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : InventoryItem
    {
        [Header("Consumable Data")]
        [SerializeField] private string useText = "Weapon Description";
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Weapon equippedPrefab = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private float range = 2f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float percentageBonus = 0;
        [SerializeField] private bool isRightHanded = true;

        const string weaponName = "Weapon";

        public float Damage => damage;

        public float Range => range;

        public float PercentageBonus => percentageBonus;

        public override string GetInfoDisplayText()
        {
            var builder = new StringBuilder();
            builder.Append(Rarity.Name).AppendLine();
            builder.Append("<color=red>Use: ").Append(useText).Append("</color>").AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
            else if (overrideController != null)
                animator.runtimeAnimatorController = animatorOverride.runtimeAnimatorController;

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null)
                return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
                handTransform = rightHand;
            else
                handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            var projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,instigator, calculatedDamage);
        }
    }
}