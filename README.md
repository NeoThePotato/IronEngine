Final Project submission as part of Tiltan's C#ISP23 course.
By Nehorai Buader (NeoThePotato).

# How To Play

# Key Bindings
**Arrow Keys**: Move/Navigate
**X/Enter/Spacebar**: Interact
**Z/Backspace**: Back
**C/ESC**: Open/Close Menu

# Goal
The goal of the game is to progress through each level, ideally, while gearing and leveling up along the way.
The end goal is the defeat the final boss, which spawns at levels 15+.
The game is a Rouge-like, as such, there is no meta-progression anywhere present. Once you lose, the game closes and you may start over.

# Overworld
**Movement**: Move your unit on the map, avoid or fight enemies, find chests, and make your way to the exit marked with a red "*XX*".
**Inventory**: Your inventory is accessible from your menu. You can use it to equip Weapons/Shields/Armors (Double-tap a piece of equipment to open its sub-menu). All pieces of equipment can be worn for extra combat effectiveness or destroyed for extra *Exp*.
**Leveling Up**: Once you've gained enough *Exp*, you are given the option to Level Up from your *Stats* menu. Leveling up allows your to raise one attribute of your choice, it also restores you to full HP.

# Objects In The World
**Units**: Marked with "*x*". Sentient entities which roam freely on the map. (Your avatar is one of those, distinguishable with a "*@*".)
**Chests**: Marked with "*CC*". Containers scattered in corners and tight spaces on the map. They contain pieces of equipment you may loot.
**Traps**: Stealthily marked with "*TT*". Hidden from sight until the player steps on them. Upon which, they deal damage. (The player may still attempt an evasion.)
**Portal**: Marked with "*EE*" or "*XX*". These mark the entry and exit points on the map. The player emerges from the green "*EE*", and may exit the level only through the red "*XX*".

# Combat
Combat begins when 2 units come into contact on the level/map.
Both units then participate in combat in a turn-based manner.
Combat ends when either participant's HP is reduces to 0. The victor is rewarded with *Exp*.

The active abilities are:
**Attack**: Utilizes your *Strength* and weapon to deal damage and deplete the other unit's HP pool.
**Defend**: Raise your *Shield* and add its defense value to your total defense, resets after taking a hit.
**Heal**: Utilize your *Intelligence* stat to recover a percentage of your total HP back, beware - as this ability's effectiveness decays with each use.

The passive abilities are:
**Evade**: Utilizes your *Speed* stat to attempt an automatic evasion if you're about to take a hit. Like the *Heal* ability - each successful evasion reduces this skill's effectiveness. Although, it resets after each battle, unlike *Heal*.

# Stats
Each unit has 4 attributes - *Vitality*, *Strength*, *Speed*, and *Intelligence*.
**Vitality**: Affects your total HP pool, as well as your *Heal* ability.
**Strength**: Affects your damage output in combat.
**Speed**: Affects your movement speed on the map, and raises your *Evasion*.
**Intelligence**: Greatly affects your *Heal* ability's potency. (For enemy units this also increases their detection range.)
