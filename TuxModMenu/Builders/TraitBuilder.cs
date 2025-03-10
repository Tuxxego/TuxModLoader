using System;
using UnityEngine;
using System.IO;

namespace TuxModLoader.Builders
{
    public class TraitBuilder
    {
        private ActorTrait trait;

        public TraitBuilder()
        {
            trait = new ActorTrait();
        }

        public TraitBuilder SetID(string traitId)
        {
            trait.id = traitId;
            return this;
        }

        public TraitBuilder SetName(string name)
        {
            // does nothing for now
            return this;
        }

        public TraitBuilder SetIconPath(string path)
        {
            trait.path_icon = Path.Combine("AddonResources", path);
            return this;
        }


        public TraitBuilder SetBirthChance(float birthChance)
        {
            trait.birth = birthChance;
            return this;
        }

        public TraitBuilder SetInheritChance(float inheritChance)
        {
            trait.inherit = inheritChance;
            return this;
        }

        public TraitBuilder SetOpposite(string opposite)
        {
            trait.opposite = opposite;
            return this;
        }

        public TraitBuilder SetOppositeTraits(string[] opposites)
        {
            trait.oppositeArr = opposites;
            return this;
        }

        public TraitBuilder SetBaseStats(BaseStats baseStats)
        {
            trait.base_stats = baseStats;
            return this;
        }

        public TraitBuilder SetTraitModifiers(int sameTraitMod, int oppositeTraitMod)
        {
            trait.sameTraitMod = sameTraitMod;
            trait.oppositeTraitMod = oppositeTraitMod;
            return this;
        }

        public TraitBuilder SetEraFlags(bool onlyActiveOnEra, bool activeMoon, bool activeNight)
        {
            trait.only_active_on_era_flag = onlyActiveOnEra;
            trait.era_active_moon = activeMoon;
            trait.era_active_night = activeNight;
            return this;
        }

        public TraitBuilder SetGroup(string groupId)
        {
            trait.group_id = groupId;
            return this;
        }

        public TraitBuilder SetType(TraitType type)
        {
            trait.type = type;
            return this;
        }

        public TraitBuilder SetSpecialEffectInterval(float interval)
        {
            trait.special_effect_interval = interval;
            return this;
        }

        public TraitBuilder SetWorldActions(WorldAction deathAction, WorldAction specialEffectAction)
        {
            trait.action_death = deathAction;
            trait.action_special_effect = specialEffectAction;
            return this;
        }

        public TraitBuilder SetAttackActions(AttackAction attackAction)
        {
            trait.action_attack_target = attackAction;
            return this;
        }

        public TraitBuilder SetHitAction(GetHitAction hitAction)
        {
            trait.action_get_hit = hitAction;
            return this;
        }

        public TraitBuilder SetExploration(bool needsToBeExplored)
        {
            trait.needs_to_be_explored = needsToBeExplored;
            return this;
        }

        public TraitBuilder SetUnlockWithAchievement(bool unlocked, string achievementId = "")
        {
            trait.unlocked_with_achievement = unlocked;
            trait.achievement_id = achievementId;
            return this;
        }

        public TraitBuilder SetCureAbility(bool canBeCured)
        {
            trait.can_be_cured = canBeCured;
            return this;
        }

        public TraitBuilder SetTraitAvailability(bool canBeGiven, bool canBeRemoved)
        {
            trait.can_be_given = canBeGiven;
            trait.can_be_removed = canBeRemoved;
            return this;
        }

        public ActorTrait Build()
        {
            AssetManager.traits.add(trait);
            PlayerConfig.unlockTrait(trait.id);
            return trait;
        }
    }
}
