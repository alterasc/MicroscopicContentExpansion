# Microscopic Content Expansion mod for Pathfinder: Wrath of the Righteous 
## Requirements: [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core/releases).

Add Antipaladin class, some feats, spells and ki powers.

## Featuring

### New classes: 

[Antipaladin](https://www.d20pfsrd.com/classes/alternate-classes/antipaladin)   
Archetypes available:  
- [Iron Tyrant](https://www.d20pfsrd.com/classes/alternate-classes/antipaladin/archetypes/paizo-antipaladin-archetypes/iron-tyrant-antipaladin-archetype/)
- [Knight of the Sepulcher](https://www.d20pfsrd.com/classes/alternate-classes/antipaladin/archetypes/paizo-antipaladin-archetypes/knight-of-the-sepulcher)
- [Tyrant](https://www.d20pfsrd.com/classes/alternate-classes/antipaladin/archetypes/paizo-antipaladin-archetypes/tyrant-antipaladin-archetype/)

IMPORTANT: For best experience also install TabletopTweaks-Base. Fiendish template for antipaladin animal companion and Armor Mastery/Shield Mastery feats that can be picked as Iron Tyrant bonus feats are used from TTT. If you don't have TTT-Base, mod will still work, just you will be missing additional possible selections. (No, I won't copypaste those in my mod, don't ask)

WARNING: Alignment restrictions do not have mythic bypass clause. You stop being evil and you lose spellbook and active abilities. This is intended.

#### Antipaladin exclusive companion and mount - Nightmare (flaming horse)

Size: Large    
Speed: 50 ft.    
AC: +8 natural armor    
Attack: bite (1d4), 2 hooves (1d6)    
Ability scores: Str 18, Dex 15, Con 16, Int 13, Wis 13, Cha 12     
Special qualities: Hoof attacks deal additional 1d4 fire damage.    
Nightmares are not animals, but evil outsiders and thus are not affected by abilities targeting specifically animals or not working on outsiders. That means Animal Growth and Legendary Proporions do NOT work on them. 

They do not have any special level where they get upgrade. They receive the usual bonuses from animal level ups.

Can be mounted with character properly (mostly) sitting on the mount. Occasionally it will be wonky, accept it as what it is.

Bardings can be equipped (provided you have proficiency), but they will not show on the model.

They use Horse portrait.

### New feats

- [Unsanctioned Knowledge (paladin and antipaladin versions)](https://www.d20pfsrd.com/feats/general-feats/unsanctioned-knowledge/)
- Snake style feat chain
	- [Snake Style](https://www.d20pfsrd.com/feats/combat-feats/snake-style-combat-style)
	- [Snake Sidewind](https://www.d20pfsrd.com/feats/combat-feats/snake-sidewind-combat)
	- [Snake Fang](https://www.d20pfsrd.com/feats/combat-feats/snake-fang-combat)
- Startoss style feat chain
	- [Startoss Style](https://www.d20pfsrd.com/feats/combat-feats/startoss-style-combat-style)
	- [Startoss Comet](https://www.d20pfsrd.com/feats/combat-feats/startoss-comet-combat)
	- [Startoss Shower](https://www.d20pfsrd.com/feats/combat-feats/startoss-shower-combat)
- Dimensional Savant feat chain
    - [Dimensional Agility](https://www.d20pfsrd.com/feats/general-feats/dimensional-agility)
	- [Dimensional Assault](https://www.d20pfsrd.com/feats/general-feats/dimensional-assault)
	- [Dimensional Dervish](https://www.d20pfsrd.com/feats/general-feats/dimensional-dervish)
- [Flickering Step](https://www.d20pfsrd.com/feats/conduit-feats/flickering-step-conduit)
- [Crusader's Flurry](https://www.d20pfsrd.com/feats/general-feats/crusader-s-flurry)
### New spells

- [Deadly Juggernaut (Cleric 3, Inquisitor 3, Paladin 3, Antipaladin 3, Warpriest 3)](https://www.d20pfsrd.com/magic/all-spells/d/deadly-juggernaut/)   
- [Blade of Dark Triumph (Antipaladin 3)](https://www.d20pfsrd.com/magic/all-spells/b/blade-of-dark-triumph)
- [Blade of Bright Victory (Paladin 3)](https://www.d20pfsrd.com/magic/all-spells/b/blade-of-bright-victory)
- [Widen Auras (Antipaladin 2)](https://www.d20pfsrd.com/magic/all-spells/w/widen-auras/) - note, only Antipaladin, no Paladin version.

### New Ki powers
- [Ki Power: Deadly Juggernaut](https://www.d20pfsrd.com/classes/core-classes/Monk/archetypes/paizo-monk-archetypes/qinggong-monk/#8th-Level_Ki_Powers)
- [Ki Power: Ki Leech](https://www.d20pfsrd.com/classes/core-classes/Monk/archetypes/paizo-monk-archetypes/qinggong-monk/#10th-Level_Ki_Powers)

### New class features
- [Druidic Herbalism](https://www.d20pfsrd.com/classes/Core-classes/druid/#Nature_Bond_Ex)
For simplicity of implementation and usage nerfed compared to tabletop description:   
  Instead of free WIS mod concoctions every day, druid gets to brew any potion for free.
- [Legion's Blessing (Cleric Crusader archetype lvl 8 feature)](https://www.d20pfsrd.com/classes/core-classes/cleric/archetypes/paizo-cleric-archetypes/crusader)
Allows you to sacrifice spell slot (through spontaneous conversion), making your next spell of exactly 3 levels lower with a range of touch to be applied to all allies in a 10 ft radius.
	- properly takes into account metamagic and stuff
	- effects that trigger "on spell cast" will trigger for each target, which may be desired or not.
	- will work on modded spells or spells added through Loremaster. Condition for triggering mass cast simply (range is touch, target is ally).
	- you're your own ally. Mass cast will be applied on you/triggered by casting on yourself
	- sacrifice spell and subsequent mass-cast are only for Crusader spellbook
	- unfortunately this mass cast can't be set up through Bubble Buffs
	- Zippy Magic with duplicate original cast once, but won't trigger any more mass casts
- [Flying Kick (Monk Style Strike)](https://www.d20pfsrd.com/classes/unchained-classes/Monk-unchained/#TOC-Style-Strike-Ex-)
Grants pseudo pounce ability Flying Kick, that can be used to move up to Fast Movement amount of feet (so 10 feet at lvl 5, 60 feet at lvl 18+) and at the end attack with flurry of blows
	- requires you to have Flurry of Blows active
	- requires you to be unarmed
	- To qualify for choosing this style you need to have FoB and Fast Movement, so archetypes that trade it away (Sensei and Sohei) can't take it.
	- visually you jump and hit with your kick (single attack), then performs full attack without first attack, resulting in total in your normal amount of attacks
	- is NOT a charge. Bonuses to charge do not apply
	- range grows with Monk level, from 10 feet to 60 feet
	- Style grants you toggleable ability, that on toggling will give you ability that actually performs jumpkick. Done this way so that you can't have another style enabled until you are allowed to have two active styles


## Thanks to  
-   bubbles and Vek17 specifically   
-   kadyn for his Expanded Content
-   Fumihiko for making of Snake Style for Kingmaker
-   Jarly for Russian localization
-   Pathfinder Wrath of The Righteous Discord channel members
-   PS: Wolfie's [Modding Wiki](https://github.com/WittleWolfie/OwlcatModdingWiki/wiki) is an excellent place to start if you want to start modding on your own.
-   Join our [Discord](https://discord.com/invite/wotr)