using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Linq;

namespace MicroscopicContentExpansion.NewComponents;
[TypeId("a3477f19abb04c9ab98337f9e31d0392")]
class RuleAttackWithWeaponChaining : RuleAttackWithWeapon
{

    public UnitEntityData Source { get; set; }
    public RuleAttackWithWeaponChaining([NotNull] UnitEntityData attacker, UnitEntityData source, [NotNull] UnitEntityData target, [NotNull] ItemEntityWeapon weapon, int attackBonusPenalty) : base(attacker, target, weapon, attackBonusPenalty)
    {
        this.Source = source;
    }

    public override void OnTrigger(RulebookEventContext context)
    {
        Rulebook.Trigger<RuleCalculateWeaponStats>(this.WeaponStats);
        this.AttackRoll = new RuleAttackRoll(this.Initiator, this.Target, this.WeaponStats, this.AttackBonusPenalty)
        {
            AutoHit = this.AutoHit,
            AutoCriticalThreat = this.AutoCriticalThreat,
            AutoCriticalConfirmation = TacticalCombatHelper.IsActive || this.AutoCriticalConfirmation,
            SuspendCombatLog = this.Weapon.Blueprint.IsRanged,
            RuleAttackWithWeapon = this,
            DoNotProvokeAttacksOfOpportunity = this.IsAttackOfOpportunity,
            ForceFlatFooted = this.ForceFlatFooted
        };
        Rulebook.Trigger<RuleAttackRoll>(this.AttackRoll);
        BlueprintProjectileReference[] projectiles = this.Weapon.Blueprint.VisualParameters.Projectiles;
        if (projectiles.Length != 0)
        {
            this.LaunchProjectilesChain(projectiles);
        }
        else
        {
            RuleAttackWithWeaponResolve evt = new RuleAttackWithWeaponResolve(this, this.Weapon.Blueprint.HasNoDamage ? null : this.CreateRuleDealDamage(true));
            this.MeleeDamage = evt.Damage;
            this.ResolveRules.Add(evt);
            context.Trigger<RuleAttackWithWeaponResolve>(evt);
        }
    }

    private void LaunchProjectilesChain(BlueprintProjectileReference[] projectiles)
    {
        foreach (BlueprintProjectileReference projectile in projectiles)
        {
            if (projectile.Get() != null)
            {
                this.LaunchProjectileChain(projectile.Get(), true);
                if (this.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Bows) && (bool)this.Initiator.Descriptor.State.Features.Manyshot && this.IsFirstAttack && this.IsFullAttack && !(bool)this.Initiator.Descriptor.State.Features.SuppressedManyshot)
                    this.LaunchProjectileChain(projectile.Get(), false);
            }
        }
    }

    private void LaunchProjectileChain(BlueprintProjectile blueprint, bool first)
    {
        Projectile projectile;
        var ssrc = this.Source != null ? this.Source : this.Initiator;
        if (this.AttackRoll.IsHit)
        {
            RuleDealDamage damage = this.Weapon.Blueprint.HasNoDamage ? null : this.CreateRuleDealDamage(TacticalCombatHelper.IsActive | first);
            RuleAttackWithWeaponResolve ruleOnHit = new RuleAttackWithWeaponResolve(this, damage);
            this.ResolveRules.Add(ruleOnHit);
            projectile = Game.Instance.ProjectileController.Launch(ssrc, (TargetWrapper)this.Target, blueprint, this.AttackRoll, ruleOnHit);
            if (damage != null)
                damage.Projectile = projectile;
        }
        else
        {
            RuleAttackWithWeaponResolve ruleOnHit = new RuleAttackWithWeaponResolve(this, null);
            this.ResolveRules.Add(ruleOnHit);
            projectile = Game.Instance.ProjectileController.Launch(ssrc, (TargetWrapper)this.Target, blueprint, this.AttackRoll, ruleOnHit);
            projectile.CalculateMissTarget();
        }
        projectile.IsFromWeapon = true;
        this.LaunchedProjectiles.Add(projectile);
        UnitPartMagus unitPartMagus = this.Initiator.Get<UnitPartMagus>();
        if (!(bool)unitPartMagus || !(unitPartMagus.EldritchArcherSpell != null))
            return;
        RuleCastSpell ruleCastSpell = Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(unitPartMagus.EldritchArcherSpell, (TargetWrapper)this.Target));
        ruleCastSpell.Context.AttackRoll = this.AttackRoll;
        ruleCastSpell.Context.MissTarget = projectile?.MissTarget;
    }
}
