using System;
using System.Collections.Generic;
using UnityEngine;

namespace TuxModLoader.Builders
{
    public class ActorBuilder
    {
        private ActorAsset actor;

        public ActorBuilder()
        {
            actor = new ActorAsset();
        }

        public ActorBuilder SetBaseStats(
            float health = 100f,
            float damage = 10f,
            float speed = 40f,
            float accuracy = 90f,
            float attackSpeed = 1f,
            float knockback = 1f,
            float size = 0.5f,
            float range = 1f)
        {
            actor.base_stats[S.health] = health;
            actor.base_stats[S.damage] = damage;
            actor.base_stats[S.speed] = speed;
            actor.base_stats[S.accuracy] = accuracy;
            actor.base_stats[S.attack_speed] = attackSpeed;
            actor.base_stats[S.knockback] = knockback;
            actor.base_stats[S.size] = size;
            actor.base_stats[S.range] = range;
            return this;
        }

        public ActorBuilder SetID(string actorId)
        {
            actor.id = actorId;
            return this;
        }

        public ActorBuilder SetName(string name)
        {
            actor.nameLocale = name;
            return this;
        }

        public ActorBuilder SetRace(string race)
        {
            actor.race = race;
            return this;
        }

        public ActorBuilder SetKingdom(string kingdom)
        {
            actor.kingdom = kingdom;
            return this;
        }

        public ActorBuilder SetSize(ActorSize size)
        {
            actor.actorSize = size;
            return this;
        }

        public ActorBuilder SetSpeedModLiquid(float speedMod)
        {
            actor.speedModLiquid = speedMod;
            return this;
        }

        public ActorBuilder SetDiet(bool dietBerries, bool dietCrops, bool dietFlowers)
        {
            actor.diet_berries = dietBerries;
            actor.diet_crops = dietCrops;
            actor.diet_flowers = dietFlowers;
            return this;
        }

        public ActorBuilder SetSpecialAbilities(bool canAttackBuildings, bool canAttackBrains)
        {
            actor.canAttackBuildings = canAttackBuildings;
            actor.canAttackBrains = canAttackBrains;
            return this;
        }

        public ActorBuilder SetFlags(List<string> flags)
        {
            actor.flags = flags;
            return this;
        }

        public ActorBuilder SetAnimationWalk(string walkAnimation, float speed)
        {
            actor.animation_walk = walkAnimation;
            actor.animation_walk_speed = speed;
            return this;
        }

        public ActorBuilder SetPrefab(string prefab)
        {
            actor.prefab = prefab;
            return this;
        }

        public ActorBuilder SetTexture(string texture)
        {
            actor.texture_path = texture;
            return this;
        }

        public ActorAsset Build()
        {
            AssetManager.actor_library.add(actor);
            return actor;
        }
    }
}